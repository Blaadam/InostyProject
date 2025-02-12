using InostyProject.Data;

namespace InostyProject.Models
{
    public class MemberDataModel
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int WorkspaceID { get; set; }
        public string AccessLevel { get; set; }
    }
}
