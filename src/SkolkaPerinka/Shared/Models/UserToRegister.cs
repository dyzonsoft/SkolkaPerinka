using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolkaPerinka.Shared.Models
{
    public class UserToRegister
    {
        [Required]
        [StringLength(30, ErrorMessage = "Prosím zkraťte své vlastní jméno na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Prosím zkraťte své  příjmení na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "Prosím zkraťte svoji adresu na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string Address { get; set; }

        [Required(ErrorMessage = "telefonní číslo na Vás potřebujeme.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "špatné telefonní číslo.")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
