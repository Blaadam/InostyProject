using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class Workspace
    {
        [Key]
        public int WorkspaceId { get; set; }

        [Required]
        [MaxLength(64)]
        public string WorkspaceName { get; set; }

        [Required]
        [MaxLength(128)]
        public string WorkspaceDescription { get; set; }
    }
}
