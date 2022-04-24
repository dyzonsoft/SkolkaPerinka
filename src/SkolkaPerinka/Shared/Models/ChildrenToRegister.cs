using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolkaPerinka.Shared.Models
{
    public class ChildrenToRegister
    {
        [Required]
        [StringLength(30, ErrorMessage = "Prosím zkraťte křestní jméno na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Prosím zkraťte své  příjmení na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "Prosím zkraťte svoji adresu na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string Address { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public string ParentEmail { get; set; }
        public bool IsChecked { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
