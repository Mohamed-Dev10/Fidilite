using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Results
{
    public class UserResult
    {
        public string Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string RefMagasinName { get; set; }
        public DateTime DateCreation { get; set; }
        public string RoleName { get; set; }
    }
}

