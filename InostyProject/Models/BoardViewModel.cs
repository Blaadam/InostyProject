namespace InostyProject.Models
{
    public class BoardViewModel
    {
        public Board board { get; set; }
        public Workspace workspace { get; set; }
        public List<BoardList> lists { get; set; }
        public bool canEdit { get; set; }
    }
}
