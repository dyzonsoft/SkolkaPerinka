using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SkolkaPerinka.Shared.Models
{
    public class User : IdentityUser
    {
        [Required]
        //[StringLength(30, ErrorMessage = "Prosím zkraťte své vlastní jméno na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string FirstName { get; set; }
        [Required]
        //[StringLength(30, ErrorMessage = "Prosím zkraťte své  příjmení na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string LastName { get; set; }
        [Required]
        //[StringLength(60, ErrorMessage = "Prosím zkraťte svoji adresu na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string Address { get; set; }

        //[Required(ErrorMessage = "telefonní číslo na Vás potřebujeme.")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "špatné telefonní číslo.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "neplatné telefonní číslo.")]
        public string Phone { get; set; }
        public string Role { get; set; }

        public bool Access { get; set; } = false;
    }
}
