using HalloDoc.Data;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{

    public class PatientRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientRequestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult CreateRequest()
        {
            return View();
        }
        public IActionResult Patient()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Patient([Bind("FirstName")] Request rc)
        {

            if (!ModelState.IsValid)
            {
                _context.Add(rc);
                await _context.SaveChangesAsync();
                return View();
            }
            return View(rc);
        }
        public IActionResult Business()
        {
            return View();
        }
        public IActionResult Concierge()
        {
            return View();
        }

        public IActionResult FamilyFriend()
        {
            return View();
        }



    }



}