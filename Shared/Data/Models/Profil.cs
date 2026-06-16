using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class Profil
    {
        public Profil()
        {
            AppFileProfil = new HashSet<AppFile>();
            AppFileUpdatedByNavigation = new HashSet<AppFile>();
            Cliente = new HashSet<Cliente>();
        }

        public string Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public int? RefMagasinId { get; set; }
        public DateTime DateCreation { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual RefMagasin RefMagasin { get; set; }
        public virtual ICollection<AppFile> AppFileProfil { get; set; }
        public virtual ICollection<AppFile> AppFileUpdatedByNavigation { get; set; }
        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}

