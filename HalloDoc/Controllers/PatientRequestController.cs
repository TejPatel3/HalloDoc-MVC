using HalloDoc.Data;
using HalloDoc.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Patient(request req)
        {
            var Asp = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            User dUser = new User
            {
                Email = req.Email,
                Id = Asp.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                CreatedBy = Asp.Id,
                CreatedDate = DateTime.Now,
                ModifiedBy = Asp.Id,
                UserId = 7,
            };
            _context.Users.Add(dUser);
            _context.SaveChanges();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == req.Email);

            Request r = new Request
            {
                UserId = user.UserId,
                RequestTypeId = 1,
                Status = 1,
                Email = user.Email,
            };
            _context.Requests.Add(r);
            _context.SaveChanges();
            var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == user.Email);
            RequestClient rc = new RequestClient();
            rc.FirstName = req.FirstName;
            rc.LastName = req.LastName;
            rc.Email = req.Email;
            rc.PhoneNumber = req.PhoneNumber;
            rc.Street = req.Street;
            rc.City = req.City;
            rc.State = req.State;
            rc.ZipCode = req.ZipCode;
            rc.RequestId = requestdata.RequestId;
            rc.RegionId = 1;
            _context.RequestClients.Add(rc);
            _context.SaveChanges();
            return RedirectToAction("CreateRequest");

        }




        public IActionResult Business()
        {
            return View();
        }
        public IActionResult Concierge()
        {
            return View();
        }

        [Route("/Patient/PatientInfoPage/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.AspNetUsers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }


        public IActionResult FamilyFriend()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FamilyFriend(request req)
        {
            var Asp = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            User dUser = new User
            {
                Email = req.Email,
                Id = Asp.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                CreatedBy = Asp.Id,
                CreatedDate = DateTime.Now,
                ModifiedBy = Asp.Id,
                UserId = 7,
            };
            _context.Users.Add(dUser);
            _context.SaveChanges();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == req.Email);

            Request r = new Request
            {
                UserId = user.UserId,
                RequestTypeId = 1,
                Status = 1,
                Email = req.Email,
            };
            _context.Requests.Add(r);
            _context.SaveChanges();
            var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == user.Email);
            RequestClient rc = new RequestClient();
            rc.FirstName = req.FirstName;
            rc.LastName = req.LastName;
            rc.Email = req.Email;
            rc.PhoneNumber = req.PhoneNumber;
            rc.Street = req.Street;
            rc.City = req.City;
            rc.State = req.State;
            rc.ZipCode = req.ZipCode;
            rc.RequestId = requestdata.RequestId;
            rc.RegionId = 1;
            _context.RequestClients.Add(rc);
            _context.SaveChanges();
            return RedirectToAction("CreateRequest");
        }



    }



}