using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return _context.Users != null ?
                        View(await _context.Requests.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

    }
}
