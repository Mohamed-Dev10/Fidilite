using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Data.Models.Market;
using BRICOMA.ECOMMERCE.Models.Enum;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BRICOMA.ECOMMERCE.Business.Services
{
    public class ClienteBOService : IClienteBOService
    {
        private readonly IClienteBORepository _clienteBORepository;
        private readonly IMarketBORepository _marketBORepository;
        private readonly IOtpService _otpService;
        private readonly IWhatsAppService _whatsAppService;
        private readonly ICardImageService _cardImageService;
        private readonly ILogger<ClienteBOService> _logger;

        public ClienteBOService(
            IClienteBORepository clienteBORepository,
            IMarketBORepository marketBORepository,
            IOtpService otpService,
            IWhatsAppService whatsAppService,
            ICardImageService cardImageService,
            ILogger<ClienteBOService> logger)
        {
            _clienteBORepository = clienteBORepository;
            _marketBORepository = marketBORepository;
            _otpService = otpService;
            _whatsAppService = whatsAppService;
            _cardImageService = cardImageService;
            _logger = logger;
        }

        public async Task<RESTServiceResponse<string>> InitCreate(ClienteModel model)
        {
            try
            {
                var validation = await Validate(model);
                if (!validation.Success)
                    return new RESTServiceResponse<string>(false, validation.Message, null);

                // Génère et persiste l'OTP (table OtpVerification), renvoie token + code
                var (token, code) = await _otpService.Create(model);

                // Envoi du code OTP par WhatsApp
                var sent = await _whatsAppService.SendMessage(
                    model.Gsm,
                    $"BRICOMA : votre code de vérification est {code}. Il expire dans 10 minutes.");

                if (!sent.Data)
                {
                    await _otpService.Consume(token);
                    return new RESTServiceResponse<string>(false, $"Échec de l'envoi du code OTP : {sent.Message}", null);
                }

                _logger.LogInformation("OTP envoyé - GSM: {Gsm}, Token: {Token}", model.Gsm, token);

                return new RESTServiceResponse<string>(true, $"Un code de vérification a été envoyé au {model.Gsm}.", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur InitCreate - GSM: {Gsm}", model.Gsm);
                return new RESTServiceResponse<string>(false, ex.Message, null);
            }
        }

        public async Task<RESTServiceResponse<string>> ConfirmCreate(string token, string otpCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return new RESTServiceResponse<string>(false, "Demande invalide.", null);

                // Vérifie l'OTP et récupère les données saisies
                var verify = await _otpService.Verify(token, otpCode);
                if (!verify.Success || verify.Data == null)
                    return new RESTServiceResponse<string>(false, verify.Message, null);

                var model = verify.Data;
                var typeId = model.RefCarteTypeId!.Value;

                var carteType = await _clienteBORepository.GetRefCarteTypeById(typeId);
                if (carteType == null)
                    return new RESTServiceResponse<string>(false, "Type de carte invalide.", null);

                // Re-vérification d'unicité (fenêtre entre l'envoi OTP et la confirmation)
                var recheck = await CheckUniqueness(model, typeId, carteType.Name);
                if (!recheck.Success)
                    return new RESTServiceResponse<string>(false, recheck.Message, null);

                // Génération Code client + CodeBarre EAN-13
                var clienteCode = await _clienteBORepository.GenerateClienteCode(model.RefMagasinId);
                var codeBarre = await _clienteBORepository.GenerateBarCode();

                // 1) Création dans BRICOMA.FIDELITE (statut Confirmée)
                var cliente = new Cliente
                {
                    Code = clienteCode,
                    CodeBarre = codeBarre,
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Gsm = model.Gsm,
                    Cin = model.Cin,
                    Nia = model.Nia,
                    Email = model.Email,
                    Adresse = model.Adresse,
                    Fonction = model.Fonction,
                    RaisonSociale = model.RaisonSociale,
                    DateNaissance = model.DateNaissance,
                    RefGenreId = model.RefGenreId,
                    RefVilleId = model.RefVilleId,
                    RefMagasinId = model.RefMagasinId,
                    RefMetierId = model.RefMetierId,
                    RefCarteTypeId = typeId,
                    RefClienteStatutId = (int)ClienteStatut.Confirmee,
                    IsConfirmed = true,
                    IsActif = true,
                    DateCreation = DateTime.Now,
                    DateConfirmation = DateTime.Now
                };

                await _clienteBORepository.Create(cliente);

                // 2) Création dans BRICOMA.MARKET
                var marketClient = await BuildMarketClient(model, typeId, clienteCode, codeBarre);
                await _marketBORepository.CreateClient(marketClient);

                // 3) Génération de l'image de la carte (template + code-barres) + envoi WhatsApp
                var carteImage = await _cardImageService.GenerateCardImage(typeId, clienteCode, codeBarre);

                var carteMessage =
                    $"Bonjour {model.Prenom} {model.Nom},\n" +
                    $"Votre carte {carteType.Name} est créée.\n" +
                    $"Code : {clienteCode}\n" +
                    $"Code-barres : {codeBarre}\n" +
                    $"Présentez-la à chaque passage en caisse BRICOMA.";

                // PublicUrl est null tant que l'URL publique n'est pas configurée → on envoie le texte seul
                await _whatsAppService.SendMessage(model.Gsm, carteMessage, carteImage?.PublicUrl);

                // 4) Consommer l'OTP
                await _otpService.Consume(token);

                _logger.LogInformation("Carte {CarteType} confirmée - Code: {Code}, GSM: {Gsm}", carteType.Name, clienteCode, model.Gsm);

                // Data = chemin local de l'image (pour la voir), même sans envoi WhatsApp
                return new RESTServiceResponse<string>(true, $"Carte {carteType.Name} créée pour {model.Prenom} {model.Nom} (Code {clienteCode}).", carteImage?.RelativeUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur ConfirmCreate - Token: {Token}", token);
                return new RESTServiceResponse<string>(false, ex.Message, null);
            }
        }

        private async Task<RESTServiceResponse<bool>> Validate(ClienteModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nom) || string.IsNullOrWhiteSpace(model.Prenom))
                return new RESTServiceResponse<bool>(false, "Nom et Prénom sont obligatoires.", false);

            model.Nom = model.Nom.Trim();
            model.Prenom = model.Prenom.Trim();

            if (string.IsNullOrWhiteSpace(model.Gsm))
                return new RESTServiceResponse<bool>(false, "Le numéro GSM est obligatoire.", false);

            model.Gsm = model.Gsm.Trim();

            if (!Regex.IsMatch(model.Gsm, @"^\d+$") || model.Gsm.Length != 10)
                return new RESTServiceResponse<bool>(false, "Le GSM doit contenir exactement 10 chiffres.", false);

            if (!model.Gsm.StartsWith("06") && !model.Gsm.StartsWith("07") && !model.Gsm.StartsWith("05"))
                return new RESTServiceResponse<bool>(false, "Le GSM doit commencer par 05, 06 ou 07.", false);

            if (model.RefMagasinId <= 0)
                return new RESTServiceResponse<bool>(false, "Le magasin est obligatoire.", false);

            if (model.RefCarteTypeId == null || model.RefCarteTypeId <= 0)
                return new RESTServiceResponse<bool>(false, "Le type de carte est obligatoire.", false);

            var carteType = await _clienteBORepository.GetRefCarteTypeById(model.RefCarteTypeId.Value);
            if (carteType == null)
                return new RESTServiceResponse<bool>(false, "Type de carte invalide.", false);

            var dateError = ValidateDateNaissance(model.DateNaissance);
            if (dateError != null)
                return dateError;

            var requiredError = ValidateRequiredFields(model, model.RefCarteTypeId.Value);
            if (requiredError != null)
                return requiredError;

            return await CheckUniqueness(model, model.RefCarteTypeId.Value, carteType.Name);
        }

        private async Task<RESTServiceResponse<bool>> CheckUniqueness(ClienteModel model, int typeId, string carteTypeName)
        {
            var gsmExist = await _clienteBORepository.GetByGsmAndType(model.Gsm, typeId);
            if (gsmExist != null)
                return new RESTServiceResponse<bool>(false, $"Ce GSM est déjà utilisé pour une carte {carteTypeName}.", false);

            if (!string.IsNullOrWhiteSpace(model.Cin))
            {
                var cinExist = await _clienteBORepository.GetByCinAndType(model.Cin, typeId);
                if (cinExist != null)
                    return new RESTServiceResponse<bool>(false, $"Cette CIN est déjà utilisée pour une carte {carteTypeName}.", false);
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailExist = await _clienteBORepository.GetByEmailAndType(model.Email, typeId);
                if (emailExist != null)
                    return new RESTServiceResponse<bool>(false, $"Cet email est déjà utilisé pour une carte {carteTypeName}.", false);
            }

            var existsInMarket = await _marketBORepository.CheckExisting(model.Gsm, model.Cin, model.Email);
            if (existsInMarket)
                return new RESTServiceResponse<bool>(false, "Ce client existe déjà dans le système MARKET (GSM, CIN ou Email).", false);

            return new RESTServiceResponse<bool>(true, "OK", true);
        }

        private async Task<Client> BuildMarketClient(ClienteModel model, int typeId, string clienteCode, string codeBarre)
        {
            var magasin = await _clienteBORepository.GetMagasinById(model.RefMagasinId);
            var ville = model.RefVilleId.HasValue ? await _clienteBORepository.GetVilleById(model.RefVilleId.Value) : null;
            var metier = model.RefMetierId.HasValue ? await _clienteBORepository.GetMetierById(model.RefMetierId.Value) : null;

            // CodePri + CodeMet selon le type de carte
            var (codePri, codeMet) = ResolveMarketCodes(typeId, metier);

            return new Client
            {
                CodeMag = magasin?.Code,
                CodeClt = clienteCode,
                Client1 = $"{model.Prenom} {model.Nom}",
                Datecreat = DateOnly.FromDateTime(DateTime.Now),
                CodeCar = codeBarre,
                Cartevalid = DateOnly.FromDateTime(DateTime.Now.AddYears(10)),
                CodePri = codePri,
                Compte = "",
                Tel = "",
                Telmob = model.Gsm,
                Adresse = "",
                CodeVil = ville?.Code,
                Email = model.Email,
                Siteweb = "",
                Statut = 1,
                Echeance = 0,
                Modepaie = "",
                Bloquer = 0,
                Plafond = 0,
                Interne = 0,
                CodeCat = "",
                Idfiscal = "",
                Rc = "",
                Patente = "",
                Solvable = 0,
                CodeVen = "",
                CodeMet = codeMet,
                BloqEdba = 0,
                Datenaiss = DateOnly.FromDateTime(model.DateNaissance),
                Ville = "",
                Cin = model.Cin,
                Nia = model.Nia,
                Actif = true
            };
        }

        private static (string codePri, string codeMet) ResolveMarketCodes(int typeId, RefMetier? metier)
        {
            switch ((CarteType)typeId)
            {
                case CarteType.BRICOMAM3ALEM:
                    return ("03", metier?.Code ?? "09");
                case CarteType.BRICOMAARTISAN:
                    return ("10", metier?.Code ?? "09");
                default: // AMIBRICOMA
                    return ("04", "09");
            }
        }

        public async Task<RESTServiceResponse<Cliente>> GetById(long id)
        {
            try
            {
                var cliente = await _clienteBORepository.GetById(id);
                if (cliente == null)
                    return new RESTServiceResponse<Cliente>(false, "Carte introuvable.", null);

                return new RESTServiceResponse<Cliente>(true, "OK", cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetById - Id: {Id}", id);
                return new RESTServiceResponse<Cliente>(false, ex.Message, null);
            }
        }

        public async Task<RESTServiceResponse<bool>> UpdateCarte(ClienteModel model)
        {
            try
            {
                if (model.Id == null || model.Id <= 0)
                    return new RESTServiceResponse<bool>(false, "Identifiant de carte manquant.", false);

                var cliente = await _clienteBORepository.GetById(model.Id.Value);
                if (cliente == null)
                    return new RESTServiceResponse<bool>(false, "Carte introuvable.", false);

                if (model.RefCarteTypeId == null || model.RefCarteTypeId <= 0)
                    return new RESTServiceResponse<bool>(false, "Le type de carte est obligatoire.", false);

                var typeId = model.RefCarteTypeId.Value;
                var carteType = await _clienteBORepository.GetRefCarteTypeById(typeId);
                if (carteType == null)
                    return new RESTServiceResponse<bool>(false, "Type de carte invalide.", false);

                // Validation des champs + unicité (en excluant la carte courante)
                var validation = await ValidateEdit(model, typeId, carteType.Name);
                if (!validation.Success)
                    return new RESTServiceResponse<bool>(false, validation.Message, false);

                // Code & CodeBarre restent figés (non modifiables)
                cliente.Nom = model.Nom.Trim();
                cliente.Prenom = model.Prenom.Trim();
                cliente.Gsm = model.Gsm.Trim();
                cliente.Cin = model.Cin;
                cliente.Nia = model.Nia;
                cliente.Email = model.Email;
                cliente.Adresse = model.Adresse;
                cliente.Fonction = model.Fonction;
                cliente.RaisonSociale = model.RaisonSociale;
                cliente.DateNaissance = model.DateNaissance;
                cliente.RefGenreId = model.RefGenreId;
                cliente.RefVilleId = model.RefVilleId;
                cliente.RefMagasinId = model.RefMagasinId;
                cliente.RefMetierId = model.RefMetierId;
                cliente.RefCarteTypeId = typeId;

                await _clienteBORepository.Update(cliente);

                // Répercussion sur BRICOMA.MARKET (même CodeClt = Code client)
                var marketClient = await _marketBORepository.GetByCodeClt(cliente.Code);
                if (marketClient != null)
                {
                    var magasin = await _clienteBORepository.GetMagasinById(model.RefMagasinId);
                    var ville = model.RefVilleId.HasValue ? await _clienteBORepository.GetVilleById(model.RefVilleId.Value) : null;
                    var metier = model.RefMetierId.HasValue ? await _clienteBORepository.GetMetierById(model.RefMetierId.Value) : null;
                    var (codePri, codeMet) = ResolveMarketCodes(typeId, metier);

                    marketClient.CodeMag = magasin?.Code;
                    marketClient.Client1 = $"{model.Prenom} {model.Nom}";
                    marketClient.Telmob = model.Gsm;
                    marketClient.Email = model.Email;
                    marketClient.CodeVil = ville?.Code;
                    marketClient.Cin = model.Cin;
                    marketClient.Nia = model.Nia;
                    marketClient.Datenaiss = DateOnly.FromDateTime(model.DateNaissance);
                    marketClient.CodePri = codePri;
                    marketClient.CodeMet = codeMet;

                    await _marketBORepository.UpdateClient(marketClient);
                }
                else
                {
                    _logger.LogWarning("Client MARKET introuvable pour CodeClt {Code} — mise à jour FIDELITE seule.", cliente.Code);
                }

                _logger.LogInformation("Carte modifiée - Id: {Id}, Code: {Code}", cliente.Id, cliente.Code);
                return new RESTServiceResponse<bool>(true, "Carte modifiée avec succès.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpdateCarte - Id: {Id}", model.Id);
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        // Champs obligatoires repris de l'ancien back-office : CIN et Métier pour tous les types,
        // NIA (18 à 20 chiffres) uniquement pour la carte Artisan. L'email reste optionnel.
        private static RESTServiceResponse<bool>? ValidateRequiredFields(ClienteModel model, int typeId)
        {
            if (string.IsNullOrWhiteSpace(model.Cin))
                return new RESTServiceResponse<bool>(false, "La CIN est obligatoire.", false);

            if (model.RefMetierId == null || model.RefMetierId <= 0)
                return new RESTServiceResponse<bool>(false, "Le métier est obligatoire.", false);

            if (typeId == (int)CarteType.BRICOMAARTISAN)
            {
                if (model.Nia == null)
                    return new RESTServiceResponse<bool>(false, "Le NIA est obligatoire pour une carte Artisan.", false);

                var nia = decimal.Truncate(model.Nia.Value).ToString("0", CultureInfo.InvariantCulture);
                if (nia.Length < 18 || nia.Length > 20)
                    return new RESTServiceResponse<bool>(false, "Le NIA doit contenir entre 18 et 20 chiffres.", false);
            }

            return null;
        }

        // La colonne SQL Cliente.DateNaissance est de type datetime (plage 1753-9999) : une date vide
        // (DateTime par défaut = 0001-01-01) ou hors plage fait échouer l'enregistrement.
        private static RESTServiceResponse<bool>? ValidateDateNaissance(DateTime dateNaissance)
        {
            if (dateNaissance == default || dateNaissance.Year < 1900)
                return new RESTServiceResponse<bool>(false, "La date de naissance est obligatoire et doit être valide.", false);

            if (dateNaissance.Date > DateTime.Today)
                return new RESTServiceResponse<bool>(false, "La date de naissance ne peut pas être dans le futur.", false);

            return null;
        }

        private async Task<RESTServiceResponse<bool>> ValidateEdit(ClienteModel model, int typeId, string carteTypeName)
        {
            if (string.IsNullOrWhiteSpace(model.Nom) || string.IsNullOrWhiteSpace(model.Prenom))
                return new RESTServiceResponse<bool>(false, "Nom et Prénom sont obligatoires.", false);

            if (string.IsNullOrWhiteSpace(model.Gsm))
                return new RESTServiceResponse<bool>(false, "Le numéro GSM est obligatoire.", false);

            model.Gsm = model.Gsm.Trim();
            if (!Regex.IsMatch(model.Gsm, @"^\d+$") || model.Gsm.Length != 10)
                return new RESTServiceResponse<bool>(false, "Le GSM doit contenir exactement 10 chiffres.", false);

            if (!model.Gsm.StartsWith("06") && !model.Gsm.StartsWith("07") && !model.Gsm.StartsWith("05"))
                return new RESTServiceResponse<bool>(false, "Le GSM doit commencer par 05, 06 ou 07.", false);

            if (model.RefMagasinId <= 0)
                return new RESTServiceResponse<bool>(false, "Le magasin est obligatoire.", false);

            var dateError = ValidateDateNaissance(model.DateNaissance);
            if (dateError != null)
                return dateError;

            var requiredError = ValidateRequiredFields(model, typeId);
            if (requiredError != null)
                return requiredError;

            // Unicité GSM/CIN/Email pour le type, en excluant la carte courante
            var gsmExist = await _clienteBORepository.GetByGsmAndType(model.Gsm, typeId);
            if (gsmExist != null && gsmExist.Id != model.Id)
                return new RESTServiceResponse<bool>(false, $"Ce GSM est déjà utilisé pour une carte {carteTypeName}.", false);

            if (!string.IsNullOrWhiteSpace(model.Cin))
            {
                var cinExist = await _clienteBORepository.GetByCinAndType(model.Cin, typeId);
                if (cinExist != null && cinExist.Id != model.Id)
                    return new RESTServiceResponse<bool>(false, $"Cette CIN est déjà utilisée pour une carte {carteTypeName}.", false);
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailExist = await _clienteBORepository.GetByEmailAndType(model.Email, typeId);
                if (emailExist != null && emailExist.Id != model.Id)
                    return new RESTServiceResponse<bool>(false, $"Cet email est déjà utilisé pour une carte {carteTypeName}.", false);
            }

            return new RESTServiceResponse<bool>(true, "OK", true);
        }

        public async Task<RESTServiceResponse<PagedResult<Cliente>>> GetList(CarteListFilterModel filter)
        {
            try
            {
                var items = await _clienteBORepository.GetListByType(
                    filter.CarteTypeId,
                    filter.Search, filter.StatutId, filter.MagasinId,
                    filter.Page, filter.PageSize);

                var total = await _clienteBORepository.CountByType(
                    filter.CarteTypeId,
                    filter.Search, filter.StatutId, filter.MagasinId);

                _logger.LogInformation("GetList - TypeId: {TypeId}, Total: {Total}, Page: {Page}", filter.CarteTypeId, total, filter.Page);

                return new RESTServiceResponse<PagedResult<Cliente>>(true, "OK", new PagedResult<Cliente>
                {
                    Items = items,
                    TotalCount = total,
                    Page = filter.Page,
                    PageSize = filter.PageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetList - TypeId: {TypeId}", filter.CarteTypeId);
                return new RESTServiceResponse<PagedResult<Cliente>>(false, ex.Message, new PagedResult<Cliente>());
            }
        }

        public async Task<RESTServiceResponse<DashboardStatsModel>> GetDashboardStats()
        {
            try
            {
                var stats = new DashboardStatsModel
                {
                    TotalCartes   = await _clienteBORepository.CountTotal(),
                    CartesM3alem  = await _clienteBORepository.CountByCarteType((int)CarteType.BRICOMAM3ALEM),
                    CartesArtisan = await _clienteBORepository.CountByCarteType((int)CarteType.BRICOMAARTISAN),
                    CartesCeMois  = await _clienteBORepository.CountCreatedThisMonth(),
                    CartesActives = await _clienteBORepository.CountByActif(true),
                    CartesBloquees = await _clienteBORepository.CountByActif(false),
                    ParMagasin = (await _clienteBORepository.CountGroupedByMagasin())
                        .Select(x => new MagasinStat { Magasin = x.Magasin, Count = x.Count })
                        .ToList()
                };

                return new RESTServiceResponse<DashboardStatsModel>(true, "OK", stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetDashboardStats");
                return new RESTServiceResponse<DashboardStatsModel>(false, ex.Message, new DashboardStatsModel());
            }
        }

        public async Task<RESTServiceResponse<List<RefMagasin>>> GetAllMagasins()
        {
            try
            {
                var list = await _clienteBORepository.GetAllMagasins();
                return new RESTServiceResponse<List<RefMagasin>>(true, "OK", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllMagasins");
                return new RESTServiceResponse<List<RefMagasin>>(false, ex.Message, new List<RefMagasin>());
            }
        }

        public async Task<RESTServiceResponse<List<RefCarteType>>> GetAllRefCarteTypes()
        {
            try
            {
                var list = await _clienteBORepository.GetAllRefCarteTypes();
                return new RESTServiceResponse<List<RefCarteType>>(true, "OK", list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur GetAllRefCarteTypes");
                return new RESTServiceResponse<List<RefCarteType>>(false, ex.Message, new List<RefCarteType>());
            }
        }

        public async Task<RESTServiceResponse<bool>> CreateRefCarteType(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return new RESTServiceResponse<bool>(false, "Le nom est obligatoire.", false);

                await _clienteBORepository.CreateRefCarteType(new RefCarteType { Name = name.Trim() });
                _logger.LogInformation("RefCarteType créé : {Name}", name);
                return new RESTServiceResponse<bool>(true, "Type de carte créé.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur CreateRefCarteType");
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> UpdateRefCarteType(int id, string name)
        {
            try
            {
                var entity = await _clienteBORepository.GetRefCarteTypeById2(id);
                if (entity == null)
                    return new RESTServiceResponse<bool>(false, "Type introuvable.", false);

                entity.Name = name.Trim();
                await _clienteBORepository.UpdateRefCarteType(entity);
                _logger.LogInformation("RefCarteType modifié : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Type de carte modifié.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur UpdateRefCarteType");
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<List<RefGenre>>> GetAllGenres()
        {
            try { return new RESTServiceResponse<List<RefGenre>>(true, "OK", await _clienteBORepository.GetAllGenres()); }
            catch (Exception ex) { _logger.LogError(ex, "Erreur GetAllGenres"); return new RESTServiceResponse<List<RefGenre>>(false, ex.Message, new List<RefGenre>()); }
        }

        public async Task<RESTServiceResponse<List<RefMetier>>> GetAllMetiers()
        {
            try { return new RESTServiceResponse<List<RefMetier>>(true, "OK", await _clienteBORepository.GetAllMetiers()); }
            catch (Exception ex) { _logger.LogError(ex, "Erreur GetAllMetiers"); return new RESTServiceResponse<List<RefMetier>>(false, ex.Message, new List<RefMetier>()); }
        }

        public async Task<RESTServiceResponse<List<RefVille>>> GetAllVilles()
        {
            try { return new RESTServiceResponse<List<RefVille>>(true, "OK", await _clienteBORepository.GetAllVilles()); }
            catch (Exception ex) { _logger.LogError(ex, "Erreur GetAllVilles"); return new RESTServiceResponse<List<RefVille>>(false, ex.Message, new List<RefVille>()); }
        }

        public async Task<RESTServiceResponse<bool>> DeleteRefCarteType(int id)
        {
            try
            {
                var entity = await _clienteBORepository.GetRefCarteTypeById2(id);
                if (entity == null)
                    return new RESTServiceResponse<bool>(false, "Type introuvable.", false);

                await _clienteBORepository.DeleteRefCarteType(entity);
                _logger.LogInformation("RefCarteType supprimé : {Id}", id);
                return new RESTServiceResponse<bool>(true, "Type de carte supprimé.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur DeleteRefCarteType");
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }
    }
}
