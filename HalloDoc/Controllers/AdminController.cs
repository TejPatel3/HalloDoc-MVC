using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(registrationViewModel req)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            if (_context.AspNetUsers.Where(m => m.Email == req.Email).Any() && _context.AspNetUsers.Where(user => user.PasswordHash == req.PasswordHash).Any())
            {
                var admin = _context.Admins.FirstOrDefault(m => m.Email == req.Email);
                HttpContext.Session.SetInt32("UserId", admin.AdminId);
                TempData["success"] = "Login Successful...!";
                return RedirectToAction("AdminDashboard");
            }
            else if (_context.AspNetUsers.Where(m => m.Email != req.Email).Any())
            {
                TempData["email"] = "Enter Correct Email";
                return View(req);
            }
            else
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(req);
            }
        }
        public IActionResult AdminForgotPassword()
        {
            return View();
        }
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboard model = new PatientDashboard();
                var users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
                Admin admin = _context.Admins.FirstOrDefault(m => m.AdminId == id);
                TempData["user"] = admin.FirstName;
                model.wiseFiles = _context.RequestWiseFiles.ToList();
                var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
                //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
                //model.requestid = reqe.RequestId;
                model.DOB = new DateTime(Convert.ToInt32(users.IntYear), DateTime.ParseExact(users.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(users.IntDate));
                //TempData["birth"] = model.DOB;
                return View(model);
            }
            else
            {
                return RedirectToAction("Admin", "Login");
            }
        }
    }
}
