using System.ComponentModel.DataAnnotations;

namespace SkolkaPerinka.Shared.Models
{
    public class User
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(80, ErrorMessage = "Your password must be between {2} and {1} characters.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
