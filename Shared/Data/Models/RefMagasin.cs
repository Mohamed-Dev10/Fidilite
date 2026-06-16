using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class RefMagasin
    {
        public RefMagasin()
        {
            Cliente = new HashSet<Cliente>();
            Profil = new HashSet<Profil>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<Profil> Profil { get; set; }
    }
}

