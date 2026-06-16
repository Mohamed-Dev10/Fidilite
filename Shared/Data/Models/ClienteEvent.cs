using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class ClienteEvent
    {
        public long Id { get; set; }
        public int NewStatusId { get; set; }
        public int? PreviousStatusId { get; set; }
        public string AspNetUsersId { get; set; }
        public string ClienteCode { get; set; }
        public DateTime DateCreation { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual RefClienteStatut NewStatus { get; set; }
        public virtual RefClienteStatut PreviousStatus { get; set; }
    }
}

