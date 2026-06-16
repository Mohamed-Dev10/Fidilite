using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class ClienteModel
    {
        public long? Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomPrenom { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string Cin { get; set; }
        public decimal? Nia { get; set; }
        public string Adresse { get; set; }
        public string RaisonSociale { get; set; }
        public string Code { get; set; }
        public string CodeBarre { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Fonction { get; set; }
        public int RefGenreId { get; set; }
        public string RefGenreName { get; set; }
        public int? RefVilleId { get; set; }
        public string RefVilleName { get; set; }
        public int RefMagasinId { get; set; }
        public string RefMagasinName { get; set; }
        public int RefClienteStatutId { get; set; }
        public string RefClienteStatutName { get; set; }
        public int? RefCarteTypeId { get; set; }
        public int? RefMetierId { get; set; }
        public DateTime DateStatut { get; set; }
        public DateTime DateCreation { get; set; }
        public bool IsActif { get; set; }
        public bool? IsBackOffice { get; set; }
        public string Remarque { get; set; }
        public string CurrentUserId { get; set; }
        public IFormFile File { get; set; }

    }
}

