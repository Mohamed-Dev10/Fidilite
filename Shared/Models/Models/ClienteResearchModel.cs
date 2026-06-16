using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class ClienteResearchModel
    {
        public string Keyword { get; set; }
        public int? RefClienteStatut { get; set; }
        public int? RefMetier { get; set; }
        public int? CurrentMagasinId { get; set; }
        public int RefCarteTypeId { get; set; }
        public string CurrentMagasinCode { get; set; }
    }
}

