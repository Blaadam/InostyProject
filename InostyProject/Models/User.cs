using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class User
    {
        [Key]
        public int AccountID { get; set; }

        [MaxLength(30)]
        public string AccountName { get; set; }

        [Required]
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Salt { get; set; }

        [MaxLength(256)]
        public string? Token { get; set; }
    }
}
