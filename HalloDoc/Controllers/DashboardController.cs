using HalloDoc.DataContext;
using HalloDoc.DataModels;
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

                model.wiseFiles = _context.RequestWiseFiles.ToList();
                var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
                //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
                model.requestid = reqe.RequestId;

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
                //var req = _context.Requests.FirstOrDefault(m => m.UserId == id);
                model.wiseFiles = (from m in _context.RequestWiseFiles where m.RequestId == requestid select m).ToList();
                //var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
                //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
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
                //var req = _context.Requests.FirstOrDefault(m => m.UserId == id);
                model.wiseFiles = (from m in _context.RequestWiseFiles where m.RequestId == requestid select m).ToList();
                //var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
                //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
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
        public ActionResult redirect(object sender, EventArgs e)
        {
            bool choice = Convert.ToBoolean(Request.Form["choice"]);
            if (choice)
            {
                return RedirectToAction("CreateRequest", "PatientRequest");
            }
            else
            {
                return RedirectToAction("Patient", "PatientRequest");
            }




            //bool choice = Convert.ToBoolean(Request.Form["selectrequesttype"]);
            string selectrequesttypes = Request.Form["selectrequesttype"];
            if (selectrequesttypes == "true")
            {
            }
            if (selectrequesttypes == "false")
            {
            }
            else
            {
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
        }
    }
}
