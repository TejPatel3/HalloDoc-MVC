using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminLog adminLog;
        private readonly IAdminDashboard _adminDashboard;


        public AdminController(IAdminLog _admin, IAdminDashboard adminDashboard)
        {
            _context = new ApplicationDbContext();
            adminLog = _admin;
            _adminDashboard = adminDashboard;
        }
        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            var request = _adminDashboard.GetAll().ToList();
            AdminRequestViewModel viewModel = new AdminRequestViewModel();
            viewModel.requests = request;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AspNetUser req)
        {

            int num = adminLog.AdminLogin(req);

            if (num == 1)
            {
                TempData["email"] = "Email Not Exist";
                return View(req);
            }

            else if (num == 2)
            {
                TempData["email"] = "You Are not Admin";
                return View(req);
            }

            else if (num == 3)
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(req);
            }

            else if (num == 0)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.Email == req.Email);
                HttpContext.Session.SetInt32("UserId", admin.AdminId);
                TempData["success"] = "Login Successful...!";
                TempData["user"] = admin.FirstName;
                return RedirectToAction("AdminDashboard");
            }
            return View(req);
        }
        public IActionResult AdminForgotPassword()
        {
            return View();
        }





        //public IActionResult AdminDashboard()
        //{
        //    if (HttpContext.Session.GetInt32("UserId") != null)
        //    {

        //        int id = (int)HttpContext.Session.GetInt32("UserId");
        //        var users = _context.Users.FirstOrDefault(m => m.UserId == id);
        //        var admin = _context.Admins.FirstOrDefault(m => m.AdminId == id);
        //        var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
        //        //model.wiseFiles = _context.RequestWiseFiles.ToList();
        //        //model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
        //        //model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
        //        //PatientDashboard model = new PatientDashboard();
        //        //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
        //        //model.requestid = reqe.RequestId;
        //        //model.DOB = new DateTime(Convert.ToInt32(users.IntYear), DateTime.ParseExact(users.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(users.IntDate));
        //        //TempData["birth"] = model.DOB;
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Admin", "Login");
        //    }
        //}

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("AdminLogin", "Admin");
        }
    }
}
