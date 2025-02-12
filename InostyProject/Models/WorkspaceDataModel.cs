namespace InostyProject.Models
{
    public class WorkspaceDataModel
    {
        public Workspace workspace;
        public List<MemberDataModel> members;

        public WorkspaceDataModel(Workspace workspace, List<MemberDataModel> members)
        {
            this.workspace = workspace;
            this.members = members;
        }
    }
}
