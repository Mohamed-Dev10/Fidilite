using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class RefClienteStatut
    {
        public RefClienteStatut()
        {
            Cliente = new HashSet<Cliente>();
            ClienteEventNewStatus = new HashSet<ClienteEvent>();
            ClienteEventPreviousStatus = new HashSet<ClienteEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<ClienteEvent> ClienteEventNewStatus { get; set; }
        public virtual ICollection<ClienteEvent> ClienteEventPreviousStatus { get; set; }
    }
}

