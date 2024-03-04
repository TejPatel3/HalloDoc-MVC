using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminLog adminLog;
        private readonly IAdminDashboard _adminDashboard;
        private readonly IAdminDashboardDataTable _adminDashboardDataTable;
        private readonly IViewCaseRepo _viewcase;
        private readonly IBlockCaseRepository _block;
        private readonly IAddOrUpdateRequestNotes _addOrUpdateRequestNotes;
        private readonly IAddOrUpdateRequestStatusLog _addOrUpdateRequestStatusLog;



        public AdminController(IAdminLog _admin, IAdminDashboard adminDashboard,
            IAdminDashboardDataTable adminDashboardDataTable, IViewCaseRepo viewcase, IBlockCaseRepository block
            , IAddOrUpdateRequestStatusLog addOrUpdateRequestStatusLog, IAddOrUpdateRequestNotes addOrUpdateRequestNotes)
        {
            _context = new ApplicationDbContext();
            adminLog = _admin;
            _adminDashboard = adminDashboard;
            _adminDashboardDataTable = adminDashboardDataTable;
            _viewcase = viewcase;
            _block = block;
            _addOrUpdateRequestNotes = addOrUpdateRequestNotes;
            _addOrUpdateRequestStatusLog = addOrUpdateRequestStatusLog;
        }
        public IActionResult AdminLogin()
        {

            return View();
        }

        public IActionResult AdminDashboard()
        {
            var casetag = _context.CaseTags.ToList();
            var request = _adminDashboard.GetAll().ToList();
            var region = _context.Regions.ToList();
            var physician = _context.Physicians.ToList();
            AdminRequestViewModel viewModel = new AdminRequestViewModel();
            viewModel.requests = request;
            viewModel.regions = region;
            viewModel.caseTags = casetag;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AspNetUser req)
        {

            int num = adminLog.AdminLogin(req);

            if (num == 1)
            {
                TempData["email"] = "Email Not Exist";
                return View(req);
            }

            else if (num == 2)
            {
                TempData["email"] = "You Are not Admin";
                return View(req);
            }

            else if (num == 3)
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(req);
            }

            else if (num == 0)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.Email == req.Email);
                HttpContext.Session.SetInt32("UserId", admin.AdminId);
                HttpContext.Session.SetString("AdminName", $"{admin.FirstName} {admin.LastName}");
                TempData["success"] = "Login Successful...!";
                TempData["user"] = admin.FirstName;
                return RedirectToAction("AdminDashboard");
            }
            return View(req);
        }
        public IActionResult AdminForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            var physician = _context.Physicians.ToList().Where(r => r.RegionId == regionId).ToList();


            return physician;
        }




        //public IActionResult AdminDashboard()
        //{
        //    if (HttpContext.Session.GetInt32("UserId") != null)
        //    {

        //        int id = (int)HttpContext.Session.GetInt32("UserId");
        //        var users = _context.Users.FirstOrDefault(m => m.UserId == id);
        //        var admin = _context.Admins.FirstOrDefault(m => m.AdminId == id);
        //        var reqe = _context.Requests.FirstOrDefault(m => m.UserId == id);
        //        //model.wiseFiles = _context.RequestWiseFiles.ToList();
        //        //model.users = _context.Users.FirstOrDefault(m => m.UserId == id);
        //        //model.requests = (from m in _context.Requests where m.UserId == id select m).ToList();
        //        //PatientDashboard model = new PatientDashboard();
        //        //var confirmationNumber =  _context.Requests.FirstOrDefault(x => x.RequestId == (Model.requests.FirstOrDefault(m => m.UserId == Model.
        //        //model.requestid = reqe.RequestId;
        //        //model.DOB = new DateTime(Convert.ToInt32(users.IntYear), DateTime.ParseExact(users.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(users.IntDate));
        //        //TempData["birth"] = model.DOB;
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Admin", "Login");
        //    }
        //}

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("AdminLogin", "Admin");
        }
        public IActionResult New()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(1);
            return View(datalist);
        }
        public IActionResult Pending()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(2);
            return View(datalist);
        }
        public IActionResult Active()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(4).Concat(_adminDashboardDataTable.getallAdminDashboard(5)).ToList();
            return View(datalist);
        }
        public IActionResult Conclude()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(6);
            return View(datalist);
        }
        public IActionResult ToClose()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(7).Concat(_adminDashboardDataTable.getallAdminDashboard(3)).Concat(_adminDashboardDataTable.getallAdminDashboard(8)).ToList();
            return View(datalist);
        }
        public IActionResult Unpaid()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(9);
            return View(datalist);
        }
        [HttpGet]
        public IActionResult ViewCase(int id)
        {
            var request = _viewcase.GetViewCaseData(id);
            TempData["RequestIdViewCase"] = request.FirstName + " " + request.LastName;
            return View(request);
        }
        [HttpPost]
        public IActionResult Edit(ViewCaseViewModel request)
        {
            _viewcase.EditInfo(request);
            var req = _context.Requests.FirstOrDefault(m => m.ConfirmationNumber == request.ConfirmationNumber);
            return RedirectToAction("ViewCase", new { id = req.RequestId });
        }
        public IActionResult CancleCase(string confirmation)
        {
            var req = _context.Requests.FirstOrDefault(m => m.ConfirmationNumber == confirmation);
            req.Status = 3;
            _context.Requests.Update(req);
            _context.SaveChanges();
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public IActionResult cancelCaseModal(int id, AdminRequestViewModel cancelnote, string casetagname)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            req.Status = 3;
            var casetag = _context.CaseTags.FirstOrDefault(t => t.Name == casetagname);
            req.CaseTag = casetag.CaseTagId.ToString();
            _context.Requests.Update(req);
            _context.SaveChanges();
            var adminid = HttpContext.Session.GetInt32("UserId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, adminid, cancelnote.BlockNotes);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public IActionResult AssignCase(int id, AdminRequestViewModel assignnote, string physicianname)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            var physiciandetail = _context.Physicians.FirstOrDefault(p => p.FirstName + p.LastName == physicianname);
            req.Status = 2;
            _context.Requests.Update(req);
            _context.SaveChanges();
            var adminid = HttpContext.Session.GetInt32("UserId");


            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, adminid, assignnote.BlockNotes, physiciandetail.PhysicianId);

            return RedirectToAction("AdminDashboard");
        }

        public IActionResult BlockModal(int id, AdminRequestViewModel blocknote)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            _block.BlockPatient(id, blocknote.BlockNotes);
            return RedirectToAction("AdminDashboard");
        }
        public IActionResult ViewNotes(int reqid)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == reqid);
            var requestnote = _context.RequestNotes.FirstOrDefault(m => m.RequestId == reqid);

            var viewnote = new AdminRequestViewModel();
            if (requestnote != null)
            {

                viewnote.BlockNotes = requestnote.AdminNotes;
            }

            var adminname = HttpContext.Session.GetString("AdminName");
            viewnote.requestid = reqid;
            var transfernotedetail = _context.RequestStatusLogs.FirstOrDefault(m => m.RequestId == reqid && m.Status == 2);
            if (transfernotedetail != null)
            {
                var physicianname = _context.Physicians.FirstOrDefault(m => m.PhysicianId == transfernotedetail.TransToPhysicianId);
                viewnote.PhsysicianNameViewNotes = physicianname.FirstName;
                viewnote.AdminNameViewNotes = adminname;
                viewnote.assigntime = transfernotedetail.CreatedDate;

            }
            return View(viewnote);
        }

        [HttpPost]
        public IActionResult viewNotes(AdminRequestViewModel obj)
        {
            //var request = _context.Requests.FirstOrDefault(m => m.RequestId == obj.requestid);
            _addOrUpdateRequestNotes.AddOrUpdateRequestNotes(obj);
            return RedirectToAction("AdminDashboard");

        }

        public IActionResult ViewUpload(int requestid)
        {
            var wisefileslist = _context.RequestWiseFiles.ToList().Where(m => m.RequestId == requestid).ToList();
            var requestclient = _context.RequestClients.FirstOrDefault(m => m.RequestId == requestid);
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            var model = new ViewUploadViewModel
            {
                wiseFiles = wisefileslist,
                requestid = requestid,
                FirstName = requestclient.FirstName,
                LastName = requestclient.LastName,
                ConfirmationNumber = request.ConfirmationNumber,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadButton(ViewUploadViewModel doc)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                ViewUploadViewModel model = new ViewUploadViewModel();

                model.wiseFiles = (from m in _context.RequestWiseFiles where m.RequestId == doc.requestid select m).ToList();
                model.requestid = doc.requestid;
                if (doc.Upload != null)
                {
                    uploadFile(doc.Upload, doc.requestid);
                }
            }

            return View();
        }
        public void uploadFile(List<IFormFile> file, int id)
        {
            foreach (var item in file)

            {
                //string path = _environment.WebRootPath + "/UploadDocument/" + item.FileName;
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HalloDoc Request Documents", item.FileName);
                ////string path = "D:\Project\HalloDoc-Images/" + item.FileName;
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
    }
}
