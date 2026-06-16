using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Data.Models;
using BRICOMA.ECOMMERCE.Models.Enum;
using BRICOMA.ECOMMERCE.Models.Helpers;
using BRICOMA.ECOMMERCE.Models.Models;

namespace BRICOMA.ECOMMERCE.Business.Services
{
    public class CarteService : ICarteService
    {
        private readonly ICarteRepository _carteRepository;
        private readonly BRICOMAFIDELITEContext _context;

        public CarteService(ICarteRepository carteRepository, BRICOMAFIDELITEContext context)
        {
            _carteRepository = carteRepository;
            _context = context;
        }

        public async Task<RESTServiceResponse<bool>> CreateM3alem(ClienteModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Validation Nom + Prenom
                if (string.IsNullOrWhiteSpace(model.Nom) || string.IsNullOrWhiteSpace(model.Prenom))
                    return new RESTServiceResponse<bool>(false, "Nom et Prénom sont obligatoires.", false);

                // 2. GSM obligatoire
                if (string.IsNullOrWhiteSpace(model.Gsm))
                    return new RESTServiceResponse<bool>(false, "Le numéro GSM est obligatoire.", false);

                // 3. Vérif doublon CIN
                if (!string.IsNullOrWhiteSpace(model.Cin))
                {
                    var cinExist = await _carteRepository.GetByCinAndType(model.Cin, (int)CarteType.BRICOMAM3ALEM);
                    if (cinExist != null)
                        return new RESTServiceResponse<bool>(false, "CIN déjà utilisée pour une carte M3alem.", false);
                }

                // 4. Vérif doublon GSM
                var gsmExist = await _carteRepository.GetByGsmAndType(model.Gsm, (int)CarteType.BRICOMAM3ALEM);
                if (gsmExist != null)
                    return new RESTServiceResponse<bool>(false, "Ce GSM est déjà utilisé pour une carte M3alem.", false);

                // 5. Vérif doublon Email (seulement si saisi)
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    var emailExist = await _carteRepository.GetByEmailAndType(model.Email, (int)CarteType.BRICOMAM3ALEM);
                    if (emailExist != null)
                        return new RESTServiceResponse<bool>(false, "Cet email est déjà utilisé pour une carte M3alem.", false);
                }

                // 6. TODO: Appel MARKET API CheckExisting
                // var checkMarket = await _marketApi.CheckExisting(model.Gsm, model.Cin, model.Email);
                // if (checkMarket.Data) return new RESTServiceResponse<bool>(false, checkMarket.Message, false);

                // 7. Créer la carte
                var carte = new Cliente
                {
                    Nom = model.Nom.Trim(),
                    Prenom = model.Prenom.Trim(),
                    Gsm = model.Gsm,
                    Cin = model.Cin,
                    Email = model.Email,
                    Adresse = model.Adresse,
                    RaisonSociale = model.RaisonSociale,
                    DateNaissance = model.DateNaissance,
                    RefVilleId = model.RefVilleId,
                    RefMagasinId = model.RefMagasinId,
                    RefMetierId = model.RefMetierId,
                    RefCarteTypeId = (int)CarteType.BRICOMAM3ALEM,
                    RefClienteStatutId = (int)ClienteStatut.Creer,
                    RefGenreId = 1,
                    IsConfirmed = false,
                    IsActif = true,
                    DateCreation = DateTime.Now
                };

                await _carteRepository.Create(carte);

                // 8. TODO: Envoyer OTP par SMS/WhatsApp
                // await _smsService.SendOTP(model.Gsm);

                await transaction.CommitAsync();
                return new RESTServiceResponse<bool>(true, "Carte M3alem créée. OTP envoyé au client.", true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> CreateArtisan(ClienteModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Validation Nom + Prenom
                if (string.IsNullOrWhiteSpace(model.Nom) || string.IsNullOrWhiteSpace(model.Prenom))
                    return new RESTServiceResponse<bool>(false, "Nom et Prénom sont obligatoires.", false);

                // 2. GSM obligatoire
                if (string.IsNullOrWhiteSpace(model.Gsm))
                    return new RESTServiceResponse<bool>(false, "Le numéro GSM est obligatoire.", false);

                // 3. Vérif doublon CIN
                if (!string.IsNullOrWhiteSpace(model.Cin))
                {
                    var cinExist = await _carteRepository.GetByCinAndType(model.Cin, (int)CarteType.BRICOMAARTISAN);
                    if (cinExist != null)
                        return new RESTServiceResponse<bool>(false, "CIN déjà utilisée pour une carte Artisan.", false);
                }

                // 4. Vérif doublon GSM
                var gsmExist = await _carteRepository.GetByGsmAndType(model.Gsm, (int)CarteType.BRICOMAARTISAN);
                if (gsmExist != null)
                    return new RESTServiceResponse<bool>(false, "Ce GSM est déjà utilisé pour une carte Artisan.", false);

                // 5. Vérif doublon Email (seulement si saisi)
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    var emailExist = await _carteRepository.GetByEmailAndType(model.Email, (int)CarteType.BRICOMAARTISAN);
                    if (emailExist != null)
                        return new RESTServiceResponse<bool>(false, "Cet email est déjà utilisé pour une carte Artisan.", false);
                }

                // 6. TODO: Appel MARKET API CheckExisting
                // var checkMarket = await _marketApi.CheckExisting(model.Gsm, model.Cin, model.Email);
                // if (checkMarket.Data) return new RESTServiceResponse<bool>(false, checkMarket.Message, false);

                // 7. Créer la carte
                var carte = new Cliente
                {
                    Nom = model.Nom.Trim(),
                    Prenom = model.Prenom.Trim(),
                    Gsm = model.Gsm,
                    Cin = model.Cin,
                    Email = model.Email,
                    Adresse = model.Adresse,
                    RaisonSociale = model.RaisonSociale,
                    DateNaissance = model.DateNaissance,
                    RefVilleId = model.RefVilleId,
                    RefMagasinId = model.RefMagasinId,
                    RefMetierId = model.RefMetierId,
                    RefCarteTypeId = (int)CarteType.BRICOMAARTISAN,
                    RefClienteStatutId = (int)ClienteStatut.Creer,
                    RefGenreId = 1,
                    IsConfirmed = false,
                    IsActif = true,
                    DateCreation = DateTime.Now
                };

                await _carteRepository.Create(carte);

                // 8. TODO: Envoyer OTP par SMS/WhatsApp
                // await _smsService.SendOTP(model.Gsm);

                await transaction.CommitAsync();
                return new RESTServiceResponse<bool>(true, "Carte Artisan créée. OTP envoyé au client.", true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new RESTServiceResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<RESTServiceResponse<bool>> ConfirmationM3alem(string otpCode, long clienteId)
        {
            // TODO: implémenter après choix service OTP
            throw new NotImplementedException();
        }

        public async Task<RESTServiceResponse<bool>> ConfirmationArtisan(string otpCode, long clienteId)
        {
            // TODO: implémenter après choix service OTP
            throw new NotImplementedException();
        }
    }
}
