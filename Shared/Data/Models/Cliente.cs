using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            MapClienteAppFile = new HashSet<MapClienteAppFile>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Cin { get; set; }
        public decimal? Nia { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string Adresse { get; set; }
        public string CodeBarre { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Fonction { get; set; }
        public string RaisonSociale { get; set; }
        public int RefGenreId { get; set; }
        public int? RefVilleId { get; set; }
        public int RefMagasinId { get; set; }
        public int? RefCarteTypeId { get; set; }
        public int? RefMetierId { get; set; }
        public int RefClienteStatutId { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? IsActif { get; set; }
        public string RemarqueDeactivation { get; set; }
        public string ProfilDeactivation { get; set; }
        public DateTime? DateDeactivation { get; set; }
        public DateTime? DateConfirmation { get; set; }
        public DateTime DateCreation { get; set; }
        

        public virtual Profil ProfilDeactivationNavigation { get; set; }
        public virtual RefCarteType RefCarteType { get; set; }
        public virtual RefClienteStatut RefClienteStatut { get; set; }
        public virtual RefGenre RefGenre { get; set; }
        public virtual RefMagasin RefMagasin { get; set; }
        public virtual RefMetier RefMetier { get; set; }
        public virtual RefVille RefVille { get; set; }
        public virtual ICollection<MapClienteAppFile> MapClienteAppFile { get; set; }
    }
}

