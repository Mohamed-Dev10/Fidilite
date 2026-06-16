using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class BirthdayTracker
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NomPrenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string TelephoneMobile { get; set; }
        public string Metier { get; set; }
        public string DateNaissance { get; set; }
        public string DateCreation { get; set; }
        public string JourAnniversaire { get; set; }
        public string MoisAnniversaire { get; set; }
        public string AnneeAnniversaire { get; set; }
        public string TypeClient { get; set; }
        public string CaNetHt { get; set; }
        public string CaNetTtc { get; set; }
        public string Carte { get; set; }
        public bool? Envoye { get; set; }
        public DateTime? DateEnvoi { get; set; }
        public bool? Consulte { get; set; }
        public DateTime? DateConsultation { get; set; }
    }
}

