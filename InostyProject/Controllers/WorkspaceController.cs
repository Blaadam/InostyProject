using InostyApp.Data;
using InostyProject.Data;
using InostyProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace InostyProject.Controllers
{
    public class WorkspaceController : Controller
    {
        public InostyContext _context;
        public WorkspaceController(InostyContext context)
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
            //Remove later
            try
            {
                return Json(new { Workspaces = _context.WorkspaceTable.ToList(), MemberLinks = _context.MemberTable.ToList() });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
            //return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Create()
        {
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

            return View();
        }

        [HttpPost]
        public ActionResult Create(Workspace workspaceModel)
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

            // Check if workspace already exists
            var workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceName.ToLower() == workspaceModel.WorkspaceName.ToLower());
            if (workspace != null)
            {
                return BadRequest(new { Message = "A Workspace with this name already exists." });
            }

            // Add workspace to database
            _context.WorkspaceTable.Add(workspaceModel);
            _context.SaveChanges();

            // Get the workspace from the database
            workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceName.ToLower() == workspaceModel.WorkspaceName.ToLower());
            if (workspace == null)
            {
                return BadRequest(new { Message = "An error occurred while creating the workspace." });
            }

            // Add Member Link
            MemberLink memberLink = new MemberLink();
            memberLink.WorkspaceID = workspaceModel.WorkspaceId;
            memberLink.AccountID = user.AccountID;
            memberLink.AccessLevel = "Owner";

            _context.MemberTable.Add(memberLink);
            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        [Route("Workspace/View/{id?}")]
        public ActionResult View(int id, string? sortBy)
        {
            // Check if a user is authenticated
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

            // Check if the workspace exists
            var workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceId == id);
            if (workspace == null)
            {
                return BadRequest(new { Message = "Workspace not found." });
            }

            // Return the workspace page
            Console.WriteLine($"Viewing workspace with ID: {id}"); // Assuming you have a logger
            var members = _context.MemberTable.Where(m => m.WorkspaceID == id).ToList();
            Console.WriteLine($"Found {members.Count} members for workspace ID: {id}");

            List<MemberDataModel> membersModel = new List<MemberDataModel>();
            members.ForEach(m =>
            {
                var account = _context.UserTable.FirstOrDefault(u => u.AccountID == m.AccountID);
                if (account != null)
                {
                    membersModel.Add(new MemberDataModel
                    {
                        AccountID = account.AccountID,
                        AccountName = account.AccountName,
                        WorkspaceID = m.WorkspaceID,
                        AccessLevel = m.AccessLevel ?? "Guest"
                    });
                }
            });

            var sortedMemberModel = SortMembers.Sort(membersModel, sortBy ?? "Descending");

            WorkspaceDataModel workspaceModel = new WorkspaceDataModel(workspace, sortedMemberModel);
            return View(workspaceModel);
        }


        [HttpPost]
        public ActionResult RemoveMember(int workspaceID, int memberID)
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

            // Check if the workspace exists
            var workspace = _context.WorkspaceTable.FirstOrDefault(w => w.WorkspaceId == workspaceID);
            if (workspace == null)
            {
                return BadRequest(new { Message = "Workspace not found." });
            }

            // Check if the workspace exists
            var member = _context.MemberTable.FirstOrDefault(w => w.AccountID == memberID && w.WorkspaceID == workspaceID);
            if (member == null)
            {
                return BadRequest(new { Message = "MemberLink not found." });
            }

            if (memberID == user.AccountID)
            {
                return BadRequest(new { Message = "You cannot remove yourself from the workspace." });
            }

            var userMemberLink = _context.MemberTable.FirstOrDefault(m => m.WorkspaceID == workspaceID && m.AccountID == user.AccountID);
            if (userMemberLink == null || userMemberLink.AccessLevel != "Owner")
            {
                return BadRequest(new { Message = "You do not have permission to remove members." });
            }

            _context.MemberTable.Remove(member);
            _context.SaveChanges();

            return Ok();
        }

    }
}