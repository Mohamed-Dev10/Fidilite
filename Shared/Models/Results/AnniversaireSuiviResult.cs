using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Results
{
    public class AnniversaireSuiviResult
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Magasin { get; set; }
        public string NomPrenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string TelephoneMobile { get; set; }
        public string Metier { get; set; }
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateCreation { get; set; }
        public string JourAnniversaire { get; set; }
        public string MoisAnniversaire { get; set; }
        public int? AnneeAnniversaire { get; set; }
        public string TypeClient { get; set; }
        public decimal? CaNetHt { get; set; }
        public decimal? CaNetTtc { get; set; }
        public string Carte { get; set; }
        public bool? Envoye { get; set; }
        public DateTime? DateEnvoi { get; set; }
        public bool? Consulte { get; set; }
        public DateTime? DateConsultation { get; set; }
        public DateTime DateCreationSuivi { get; set; }
    }
}

