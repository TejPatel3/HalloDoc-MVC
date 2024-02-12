using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;



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
            //return _context.AspNetUsers != null ?
            //             View(await _context.AspNetUsers.ToListAsync()) :
            //             Problem("Entity set 'ApplicationDbContext.Users'  is null.");

        }

        public IActionResult ForgotPassword()
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
            if (ModelState.IsValid)
            {
                Guid id = Guid.NewGuid();
                AspNetUser user = new AspNetUser();


                user.Id = id.ToString();
                user.Email = obj.Email;
                user.UserName = obj.Email;
                user.PasswordHash = obj.PasswordHash;
                _context.AspNetUsers.Add(user);
                _context.SaveChanges();
                return RedirectToAction("CreateRequest", "PatientRequest");

            }
            else
            {
                return View(obj);
            }



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
                return RedirectToAction("Dashboard", "PatientDashboard");
            }

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }



}