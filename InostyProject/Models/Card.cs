using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InostyProject.Models
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }

        [ForeignKey("List")]
        public int ListId { get; set; }

        [Required]
        [MaxLength(16)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
