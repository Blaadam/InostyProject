using System.ComponentModel.DataAnnotations;

namespace InostyProject.Models
{
    public class MemberLink
    {
        [Key]
        public int MemberLinkID { get; set; }
        public int WorkspaceID { get; set; }
        public int AccountID { get; set; }
        public string AccessLevel { get; set; }
    }
}
