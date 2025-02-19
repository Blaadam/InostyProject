namespace InostyProject.Models
{
    public class WorkspaceDataModel
    {
        public Workspace workspace;
        public List<MemberDataModel> members;
        public List<Board> boards;

        public WorkspaceDataModel(Workspace workspace, List<MemberDataModel> members, List<Board> boards)
        {
            this.workspace = workspace;
            this.members = members;
            this.boards = boards;
        }
    }
}
