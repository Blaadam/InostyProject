using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class BoardList
    {
        [Key]
        public int ListID { get; set; }

        [Required]
        public int BoardID { get; set; }

        [MaxLength(16)]
        public string ListName { get; set; }
    }
}
