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
            return RedirectToAction("View", "Workspace", boardModel.WorkspaceId);
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
                return NotFound(new {message = $"BoardId ({board.BoardID}) could not be found."});
            }

            var workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceId == board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound(new { message = $"WorkspaceId ({board.WorkspaceId}) could not be found." });
            }

            boardModel.board = board;
            boardModel.workspace = workspace;

            return Json(boardModel);
        }

    }
}
