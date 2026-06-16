using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Results
{
    public class MARKETClienteResult
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NomPrenom { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string Cin { get; set; }
        public string Adresse { get; set; }
        public string CodeBarre { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string Fonction { get; set; }
        public int RefMagasinId { get; set; }
        public string RefMagasinName { get; set; }
        public int RefGenreId { get; set; }
        public string RefGenreName { get; set; }
        public int RefVilleId { get; set; }
        public string RefVilleCode { get; set; }
        public string RefVilleName { get; set; }
        public int RefClienteStatutId { get; set; }
        public string RefClienteStatutName { get; set; }
        public DateTime? DateStatut { get; set; }
        public DateTime? DateCreation { get; set; }
    }
}

