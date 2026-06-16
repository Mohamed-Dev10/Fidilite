using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class AppFile
    {
        public AppFile()
        {
            MapClienteAppFile = new HashSet<MapClienteAppFile>();
        }

        public int Id { get; set; }
        public string ProfilId { get; set; }
        public string NomPrenom { get; set; }
        public string Url { get; set; }
        public DateTime? DateCreation { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateModification { get; set; }

        public virtual Profil Profil { get; set; }
        public virtual Profil UpdatedByNavigation { get; set; }
        public virtual ICollection<MapClienteAppFile> MapClienteAppFile { get; set; }
    }
}

