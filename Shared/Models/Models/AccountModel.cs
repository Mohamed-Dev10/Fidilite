using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Models
{
    public abstract class BasePasswordModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoginViewModel : BasePasswordModel
    {

    }

    public class RegisterViewModel : BasePasswordModel
    {
        [Required]
        public string Prenom { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int? RefMagasinId { get; set; }
    }
}

