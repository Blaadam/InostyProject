using System.ComponentModel.DataAnnotations;

namespace InostyProject.Data
{
    public class User
    {
        [Key]
        public int AccountID { get; set; }
        [MaxLength(30)]
        public string AccountName { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
