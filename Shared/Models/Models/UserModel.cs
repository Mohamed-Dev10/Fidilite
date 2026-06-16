using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int? RefMagasinId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}

