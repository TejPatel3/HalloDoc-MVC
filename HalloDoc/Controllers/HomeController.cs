using HalloDoc.DataModels;
using HalloDoc.Models;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(registrationViewModel obj)
        {
            Guid id = Guid.NewGuid();
            AspNetUser user = _context.AspNetUsers.FirstOrDefault(m => m.Email == obj.Email);
            user.PasswordHash = obj.PasswordHash;
            _context.AspNetUsers.Add(user);
            _context.SaveChanges();
            TempData["success"] = "Your Account Created Successful";
            return RedirectToAction("PatientDashboard", "Dashboard");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AspNetUser obj)
        {
            if (_context.AspNetUsers.Where
                (m => m.Email == obj.Email).Any() && _context.AspNetUsers.Where(user => user.PasswordHash == obj.PasswordHash).Any())
            {
                var user = _context.Users.FirstOrDefault(m => m.Email == obj.Email);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                TempData["success"] = "Login Successful...!";
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            else if (!_context.AspNetUsers.Where(m => m.Email == obj.Email).Any())
            {
                TempData["email"] = "Email Does Not Exist";
                return View(obj);
            }
            else
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(obj);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult resetPassword()
        {
            return View();
        }

        public IActionResult ForgotPassword(AspNetUser user)
        {
            return View(user);
        }
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("resetPassword", "Home", new { id = userId });
            return baseUrl + resetPasswordPath;
        }
        public IActionResult PatientResetPasswordEmail(AspNetUser user)
        {
            string Id = (_context.AspNetUsers.FirstOrDefault(x => x.Email == user.Email)).Id;
            string resetPasswordUrl = GenerateResetPasswordUrl(Id);
            SendEmail(user.Email, "Reset Your Password", $"Hello, Click On below Link for Reset Your Password: {resetPasswordUrl}");

            TempData["success"] = "Reset Password Link sent Successful";
            return RedirectToAction("Login", "Home");
        }


        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.tejpatel@outlook.com";
            var password = "7T6d2P3@K";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        [HttpPost]
        public IActionResult resetPassword(AspNetUser aspnetuser)
        {
            var aspuser = _context.AspNetUsers.FirstOrDefault(x => x.Email == aspnetuser.Email);
            aspuser.PasswordHash = aspnetuser.PasswordHash;
            _context.AspNetUsers.Update(aspuser);
            _context.SaveChanges();
            TempData["success"] = "Your Password Reset Successful";
            return RedirectToAction("Login");
        }

    }
}

