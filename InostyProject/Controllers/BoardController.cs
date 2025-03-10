using InostyApp.Data;
using InostyProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace InostyProject.Controllers
{
    public class BoardController : Controller
    {
        public InostyContext _context;
        public BoardController(InostyContext context)
        {
            _context = context;
        }
        public User? GetUserFromToken(string token)
        {
            User? user = _context.UserTable.FirstOrDefault(u => u.Token == token);
            return user;
        }

        public ActionResult Index()
        {
            return Json(_context.BoardTable.ToList());
        }

        [HttpGet]
        [Route("Board/Create/{id?}")]
        public ActionResult Create(int id)
        {
            Board boardModel = new Board();
            boardModel.WorkspaceId = id;

            return View(boardModel);
        }

        [HttpPost]
        public ActionResult Create(Board boardModel)
        {
            _context.BoardTable.Add(boardModel);
            _context.SaveChanges();
            return RedirectToAction("View", "Workspace", new { id = boardModel.WorkspaceId });
        }

        [HttpGet]
        [Route("Board/View/{id?}")]
        public ActionResult View(int id)
        {
            // Check if user is authenticated
            string? token = Request.Cookies["token"];
            if (token == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = GetUserFromToken(token);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            BoardViewModel boardModel = new BoardViewModel();
            var board = _context.BoardTable.FirstOrDefault(b => b.BoardID == id);

            if (board == null)
            {
                return NotFound(new { message = $"BoardId {id} could not be found." });
            }

            var workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceId == board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound(new { message = $"WorkspaceId {board.WorkspaceId} could not be found." });
            }

            var lists = _context.ListTable.Where(l => l.BoardID == board.BoardID).ToList();

            boardModel.board = board;
            boardModel.workspace = workspace;
            boardModel.lists = lists;

            return View(boardModel);
        }

        [HttpGet]
        [Route("Board/CreateList/{boardId?}")]
        public ActionResult CreateList(int boardId)
        {
            BoardList list = new BoardList();
            list.BoardID = boardId;
            return View(list);
        }

        [HttpPost]
        public ActionResult CreateList(BoardList list)
        {
            _context.ListTable.Add(list);
            _context.SaveChanges();
            return RedirectToAction("View", "Board", new { id = list.BoardID });
        }

        [HttpDelete]
        public ActionResult DeleteList(int id)
        {
            var list = _context.ListTable.FirstOrDefault(l => l.ListID == id);
            if (list == null)
            {
                return BadRequest(new { message = $"ListId {id} could not be found." });
            }

            var boardId = list.BoardID;
            _context.ListTable.Remove(list);
            _context.SaveChanges();

            return Ok(new {message = $"Successfully removed {list.ListName}"});
        }

        [HttpGet]
        [Route("Board/CreateCard/{boardId?}")]
        public ActionResult CreateCard(int boardId)
        {
            BoardList list = new BoardList();
            list.BoardID = boardId;
            return View(list);
        }

        [HttpPost]
        public ActionResult CreateCard(BoardList list)
        {
            _context.ListTable.Add(list);
            _context.SaveChanges();
            return RedirectToAction("View", "Board", new { id = list.BoardID });
        }

    }
}
