using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class ClientUserData
    {
        [MaxLength(30)]
        public string AccountName { get; set; }

        [Required]
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}