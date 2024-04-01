using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace HalloDoc.Controllers.Provider
{
    public class SchedulingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SchedulingController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Scheduling()
        {
            //DayWiseScheduling dayWiseScheduling = new DayWiseScheduling();
            //dayWiseScheduling.physicians = _context.Physicians.ToList();
            //return View(dayWiseScheduling);
            Scheduling scheduling = new Scheduling();
            scheduling.physicianlist = _context.Physicians.ToList();
            return View(scheduling);
        }
    }
}
