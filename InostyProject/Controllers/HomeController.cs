using System.Diagnostics;
using InostyApp.Data;
using InostyProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InostyProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public InostyContext _context;

        public HomeController(ILogger<HomeController> logger, InostyContext context)
        {
            _context = context;
            _logger = logger;
        }

        public User? GetUserFromToken(string token)
        {
            User? user = _context.UserTable.FirstOrDefault(u => u.Token == token);
            return user;
        }

        public IActionResult Index()
        {
            // Check if user is authenticated
            string? token = Request.Cookies["token"];
            if (token == null)
            {
                return View(new List<Workspace>());
            }
            var currentUser = GetUserFromToken(token);
            if (currentUser == null)
            {
                return View(new List<Workspace>());
            }

            var workspaceIds = _context.MemberTable
                .Where(u => u.AccountID == currentUser.AccountID)
                .Select(u => u.WorkspaceID)
                .ToList();

            var workspaces = _context.WorkspaceTable
                .Where(w => workspaceIds.Contains(w.WorkspaceId))
                .ToList();

            return View(workspaces);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
