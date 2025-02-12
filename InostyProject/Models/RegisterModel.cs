using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(30)]
        public string AccountName { get; set; }

        [Required]
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(30)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
