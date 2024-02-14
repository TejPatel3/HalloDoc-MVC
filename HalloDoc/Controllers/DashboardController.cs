using HalloDoc.DataContext;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> PatientDashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboard model = new PatientDashboard();
                var users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
                TempData["user"] = users.FirstName;

                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ViewDocument()
        {
            return View();
        }

    }
}
