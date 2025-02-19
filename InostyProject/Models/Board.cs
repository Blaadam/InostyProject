using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InostyProject.Models
{
    public class Board
    {
        [Key]
        public int BoardID { get; set; }
        [Required]
        [ForeignKey("Workspace")]
        public int WorkspaceId { get; set; }
        [Required]
        [MaxLength(64)]
        [MinLength(3)]
        public string BoardName { get; set; }
    }
}
