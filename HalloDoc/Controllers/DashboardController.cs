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



        public async Task<IActionResult> PatientDashboard(int id)
        {
            PatientDashboard model = new PatientDashboard();
            model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
            model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
            var users = _context.Users.FirstOrDefault(m => m.UserId == id);
            TempData["user"] = users.FirstName;

            return View(model);
        }


    }
}
