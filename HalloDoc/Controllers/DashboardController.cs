using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult PatientDashboard()
        {
            return View();
        }
    }
}
