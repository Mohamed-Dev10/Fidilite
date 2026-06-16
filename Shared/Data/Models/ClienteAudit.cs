using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class ClienteAudit
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string ClienteCode { get; set; }
        public string AspNetUsersId { get; set; }
        public DateTime DateCreation { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}

