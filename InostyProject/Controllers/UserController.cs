using InostyApp.Data;
using InostyProject.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using InostyProject.Data;

namespace InostyProject.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private PasswordHandler passwordHandler = new PasswordHandler();
        public InostyContext _context;
        public UserController(InostyContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            // Remove later
            //try
            //{
            //    return Json(_context.UserTable.ToList());
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Message = ex.Message });
            //}
            return RedirectToAction("Login", "User");
        }

        public bool userIsAlreadyLoggedIn()
        {
            if (Request.Cookies["token"] != null)
            {
                Console.WriteLine("User is already logged in!");
                return true;
            }

            return false;
        }
        public string generateToken()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (userIsAlreadyLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetCurrentUser()
        {
            string token = Request.Cookies["token"];
            if (token == null)
            {
                return Json(new { });
            }

            var user = _context.UserTable.FirstOrDefault(u => u.Token == token);
            if (user == null)
            {
                return Json(new { });
            }

            var userData = new ClientUserData
            {
                Email = user.Email,
                AccountName = user.AccountName
            };

            return Json(userData);
        }

        [HttpPost]
        public ActionResult Login(User userModel)
        {
            var allUsers = _context.UserTable.ToList();
            User userDetails = null;

            // Check if user exists
            for (int i = 0; i < allUsers.Count; i++)
            {
                var user = allUsers[i];
                if (user == null) continue;
                if (user.Email != userModel.Email) continue;

                bool matches = passwordHandler.checkHashedPassword(user.Password, user.Salt, userModel.Password);
                if (!matches) continue;

                userDetails = user;
            }

            if (userDetails == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            string sessionToken = generateToken();

            userDetails.Token = sessionToken;
            _context.SaveChanges();

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);
            options.HttpOnly = true;
            options.Secure = true;

            Response.Cookies.Append("token", sessionToken, options);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (userIsAlreadyLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Password)) // Crucial check!
            {
                return BadRequest(new { message = "Password is required." });
                //return View(userModel);
            }

            var existingUser = _context.UserTable.FirstOrDefault(user => user.Email == userModel.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            User newUser = new User();
            newUser.Email = userModel.Email;
            newUser.AccountName = userModel.AccountName;

            byte[] salt;
            string password = passwordHandler.hashPassword(userModel.Password, out salt);

            newUser.Salt = Convert.ToBase64String(salt);
            newUser.Password = password;

            _context.UserTable.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok();
        }
    }
}
