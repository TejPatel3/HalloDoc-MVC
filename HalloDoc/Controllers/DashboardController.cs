using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
                return RedirectToAction("Login", "Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ViewDocument(int requestid)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboard model = new PatientDashboard();
                var users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
                TempData["user"] = users.FirstName;
                TempData["RequestId"] = requestid;
                model.wiseFiles = (from m in _context.RequestWiseFiles where m.RequestId == requestid select m).ToList();
                model.requestid = requestid;
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public IActionResult UploadButton(PatientDashboard req, int requestid)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboard model = new PatientDashboard();
                var users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
                model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
                TempData["user"] = users.FirstName;
                TempData["RequestId"] = requestid;
                model.wiseFiles = (from m in _context.RequestWiseFiles where m.RequestId == requestid select m).ToList();
                model.requestid = requestid;
                if (req.Upload != null)
                {
                    uploadFile(req.Upload, requestid);
                }
                return RedirectToAction("ViewDocument", model);
            }
            else
            {
                return RedirectToAction("ViewDocument", requestid);

            }
        }
        public void uploadFile(List<IFormFile> file, int id)
        {
            foreach (var item in file)

            {
                //string path = _environment.WebRootPath + "/UploadDocument/" + item.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", item.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(fileStream);
                }
                RequestWiseFile requestWiseFiles = new RequestWiseFile
                {
                    RequestId = id,
                    FileName = path,
                    CreatedDate = DateTime.Now,
                };
                _context.RequestWiseFiles.Add(requestWiseFiles);
                _context.SaveChanges();
            }
        }
        [HttpPost]
        public IActionResult redirect()
        {
            var radio = Request.Form["radiobtn"];
            if (radio == "me")
            {
                return RedirectToAction("Me");
            }
            if (radio == "someone")
            {
                return RedirectToAction("SomeOneElse");
            }
            else
            {
                return RedirectToAction("PatientDashboard");
            }
        }

        public IActionResult Me()
        {

            int id = (int)HttpContext.Session.GetInt32("UserId");
            var users = _context.Users.FirstOrDefault(m => m.UserId == id);
            TempData["user"] = users.FirstName;
            patientRequest detail = new patientRequest();
            detail.FirstName = users.FirstName;
            detail.LastName = users.LastName;
            detail.Email = users.Email;
            return View(detail);
        }
        public IActionResult SomeOneElse()
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var users = _context.Users.FirstOrDefault(m => m.UserId == id);
            TempData["user"] = users.FirstName;
            return View();
        }

        public IActionResult edit(PatientDashboard req)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var users = _context.Users.FirstOrDefault(m => m.UserId == id);
            users.FirstName = req.users.FirstName;
            users.LastName = req.users.LastName;
            users.City = req.users.City;
            users.State = req.users.State;
            users.ZipCode = req.users.ZipCode;
            users.Street = req.users.Street;
            _context.Users.Update(users);
            _context.SaveChanges();
            return RedirectToAction("PatientDashboard", "Dashboard");
        }
    }
}
