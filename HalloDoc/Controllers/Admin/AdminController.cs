using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers.Admin
{
    [AuthorizationRepository("Admin")]
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
        private readonly IJwtRepository _jwtRepo;
        public AdminController(
        IAdminLog _admin, IAdminDashboard adminDashboard,
        IAdminDashboardDataTable adminDashboardDataTable,
        IViewCaseRepo viewcase, IBlockCaseRepository block,
        IAddOrUpdateRequestStatusLog addOrUpdateRequestStatusLog,
        IAddOrUpdateRequestNotes addOrUpdateRequestNotes,
        IJwtRepository jwtRepo)
        {
            _context = new ApplicationDbContext();
            adminLog = _admin;
            _adminDashboard = adminDashboard;
            _adminDashboardDataTable = adminDashboardDataTable;
            _viewcase = viewcase;
            _block = block;
            _addOrUpdateRequestNotes = addOrUpdateRequestNotes;
            _addOrUpdateRequestStatusLog = addOrUpdateRequestStatusLog;
            _jwtRepo = jwtRepo;
        }

        byte[] key = { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        byte[] iv = { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        //************************************** Admin Dashboard ************************************//
        public IActionResult AdminDashboard()
        {
            var casetag = _context.CaseTags.ToList();
            var request = _adminDashboard.GetAll().ToList();
            var viewmodel = new AdminDashboardTableDataViewModel();
            var region = _context.Regions.ToList();
            var physician = _context.Physicians.ToList();
            AdminRequestViewModel viewModel = new AdminRequestViewModel();
            viewModel.requests = request;
            viewModel.regions = region;
            viewModel.caseTags = casetag;
            return View(viewModel);
        }

        //*********************************************************************************this is for physician transfer model code
        //[HttpPost]
        //public IActionResult TransferModel(int id, AdminRequestViewModel assignnote, string physicianname)
        //{
        //    var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
        //    var physiciandetail = _context.Physicians.FirstOrDefault(p => p.FirstName + p.LastName == physicianname);
        //    req.PhysicianId = physiciandetail.PhysicianId;
        //    _context.Requests.Update(req);
        //    _context.SaveChanges();
        //    var adminid = HttpContext.Session.GetInt32("AdminId");


        //    _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, assignnote.BlockNotes, physiciandetail.PhysicianId, physiciandetail.PhysicianId);
        //    TempData["success"] = "Request auccessfully Assigned..!";

        //    return RedirectToAction("AdminDashboard");
        //}

        //************************************** 6 Tabs Open controller method ************************************//
        public IActionResult DashboardTabsData(string status, int currentpage, int requesttype, string searchkey, int regionid)
        {
            var data = _adminDashboardDataTable.getallAdminDashboard(status, currentpage, requesttype, searchkey, regionid);
            ViewBag.TotalRowsOfDataContent = data.Count();
            var newdatalist = data.Skip((currentpage - 1) * 5).Take(5).ToList();
            switch (status)
            {
                case "1":
                    return View("New", newdatalist);
                    break;
                case "2":
                    return View("Pending", newdatalist);
                    break;
                case "3":
                    return View("ToClose", newdatalist);
                    break;
                case "4":
                    return View("Active", newdatalist);
                    break;
                case "5":
                    return View("Active", newdatalist);
                    break;
                case "6":
                    return View("Conclude", newdatalist);
                    break;
                case "7":
                    return View("ToClose", newdatalist);
                    break;
                case "8":
                    return View("ToClose", newdatalist);
                    break;
                case "9":
                    return View("Unpaid", newdatalist);
                    break;
                default: return View();
            }
        }

        //public IActionResult New(int currentpage)
        //{

        //    if (currentpage == 0)
        //    {
        //        currentpage = 1;
        //    }
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(1, currentpage);
        //    ViewBag.TotalRowsOfDataContent = datalist.Count();
        //    var newdatalist = datalist.Skip((currentpage - 1) * 5).Take(5).ToList();
        //    return View(newdatalist);
        //}

        //public IActionResult Pending(int currentpage)
        //{
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(2, currentpage);
        //    foreach (var item in datalist)
        //    {
        //        var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
        //        item.PhysicianName = physician.FirstName;
        //    }
        //    return View(datalist);
        //}

        //public IActionResult Active(int currentpage)
        //{
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(4, currentpage).Concat(_adminDashboardDataTable.getallAdminDashboard(5, currentpage)).ToList();
        //    foreach (var item in datalist)
        //    {
        //        var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
        //        item.PhysicianName = physician.FirstName;
        //    }
        //    return View(datalist);
        //}

        //public IActionResult Conclude(int currentpage)
        //{
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(6, currentpage);
        //    foreach (var item in datalist)
        //    {
        //        var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
        //        item.PhysicianName = physician.FirstName;
        //    }
        //    return View(datalist);
        //}

        //public IActionResult ToClose(int currentpage)
        //{
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(7, currentpage).Concat(_adminDashboardDataTable.getallAdminDashboard(3, currentpage)).Concat(_adminDashboardDataTable.getallAdminDashboard(8, currentpage)).ToList();
        //    foreach (var item in datalist)
        //    {
        //        var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
        //        item.PhysicianName = physician.FirstName;
        //    }
        //    return View(datalist);
        //}

        //public IActionResult Unpaid(int currentpage)
        //{
        //    var datalist = _adminDashboardDataTable.getallAdminDashboard(9, currentpage);
        //    foreach (var item in datalist)
        //    {
        //        var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
        //        item.PhysicianName = physician.FirstName;
        //    }
        //    return View(datalist);
        //}


        //************************************** Action DropDown Methods ************************************//

        [HttpGet]
        public IActionResult ViewCase(int id)
        {
            var request = _viewcase.GetViewCaseData(id);
            TempData["RequestIdViewCase"] = request.FirstName + " " + request.LastName;
            return View(request);
        }

        [HttpPost]
        public IActionResult ViewCaseEditData(ViewCaseViewModel request)
        {
            _viewcase.EditInfo(request);
            var req = _context.Requests.FirstOrDefault(m => m.ConfirmationNumber == request.ConfirmationNumber);
            return RedirectToAction("ViewCase", new { id = req.RequestId });
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
            _addOrUpdateRequestNotes.AddOrUpdateRequestNotes(obj);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult ViewUpload(int requestid)
        {
            var wisefileslist = _context.RequestWiseFiles.ToList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();
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

        public IActionResult Order(int requestid)
        {
            var professiontypeList = _context.HealthProfessionalTypes.ToList();
            SendOrderModal orderdata = new SendOrderModal();
            orderdata.requestid = requestid;
            orderdata.Healthprofessionaltypes = professiontypeList;
            return View(orderdata);
        }

        [HttpPost]
        public IActionResult Orders(int requestid, int vendorid, string prescription, int refill)
        {
            var venderDetails = _context.HealthProfessionals.FirstOrDefault(m => m.VendorId == vendorid);
            var order = new OrderDetail
            {
                VendorId = vendorid,
                RequestId = requestid,
                FaxNumber = venderDetails.FaxNumber,
                Email = venderDetails.Email,
                BusinessContact = venderDetails.BusinessContact,
                CreatedDate = DateTime.Now,
                Prescription = prescription,
                NoOfRefill = refill,
                CreatedBy = HttpContext.Session.GetString("AdminName")
            };

            if (order != null)
            {
                _context.OrderDetails.Add(order);
                _context.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }


        //************************************** View Upload page inner method ************************************//

        [HttpPost]
        public IActionResult UploadButton(List<IFormFile> file, int id)
        {
            if (file.Count() == 0)
            {
                TempData["warning"] = "Please Select Document..!";
            }
            else
            {
                uploadFile(file, id);
                TempData["success"] = "Document Upload Successfully..!";
            }
            return RedirectToAction("ViewUpload", new { requestid = id });
        }

        public void uploadFile(List<IFormFile> file, int id)
        {
            foreach (var item in file)
            {
                //string path = _environment.WebRootPath + "/UploadDocument/" + item.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", item.FileName);
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

        [HttpPost]
        public IActionResult DeleteDoc(int wiseid, int reqId)
        {
            var wisefile = _context.RequestWiseFiles.FirstOrDefault(m => m.RequestWiseFileId == wiseid);
            BitArray t = new BitArray(1);
            t[0] = true;
            wisefile.IsDeleted = t;
            _context.RequestWiseFiles.Update(wisefile);
            _context.SaveChanges();
            TempData["success"] = "Document Deleted Successfully..!";
            return RedirectToAction("ViewUpload", new { requestid = reqId });
        }

        [HttpPost]
        public IActionResult SendMail(List<int> wiseFileId, int reqid)
        {
            List<string> filenames = new List<string>();
            foreach (var item in wiseFileId)
            {
                var s = (item);
                var file = _context.RequestWiseFiles.FirstOrDefault(x => x.RequestWiseFileId == s).FileName;
                filenames.Add(file);
            }
            Sendemail("yashsarvaiya40@gmail.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
            TempData["success"] = "Document sent in Email..!";
            return RedirectToAction("ViewUpload", new { requestid = reqid });
        }

        public async Task Sendemail(string email, string subject, string message, List<string> attachmentPaths)
        {
            try
            {
                var mail = "tatva.dotnet.tejpatel@outlook.com";
                var password = "7T6d2P3@K";
                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);

                foreach (var attachmentPath in attachmentPaths)
                {
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await client.SendMailAsync(mailMessage);
                TempData["success"] = "Document sent in Email..!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }


        //************************************** Action DropDown Modals methods ************************************//

        public IActionResult cancelCase(string confirmation)
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
            if (casetag != null)
            {
                req.CaseTag = casetag.CaseTagId.ToString();

            }
            _context.Requests.Update(req);
            _context.SaveChanges();
            var adminid = HttpContext.Session.GetInt32("AdminId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, cancelnote.BlockNotes, adminid);
            TempData["success"] = "Request Canceled Successfully..!";
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public IActionResult AssignCase(int id, AdminRequestViewModel assignnote, string physicianname)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            var physiciandetail = _context.Physicians.FirstOrDefault(p => p.FirstName + p.LastName == physicianname);
            req.Status = 2;
            req.PhysicianId = physiciandetail.PhysicianId;
            _context.Requests.Update(req);
            _context.SaveChanges();
            var adminid = HttpContext.Session.GetInt32("AdminId");

            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, assignnote.BlockNotes, adminid, physiciandetail.PhysicianId);
            TempData["success"] = "Request auccessfully Assigned..!";
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult BlockModal(int id, AdminRequestViewModel blocknote)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            _block.BlockPatient(id, blocknote.BlockNotes);
            TempData["success"] = "Request Blocked Successfully..!";
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult ClearModal(int id)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            if (request != null)
            {
                request.Status = 10;
                _context.Requests.Update(request);
                _context.SaveChanges();
            }
            var adminid = HttpContext.Session.GetInt32("AdminId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, null, adminid);
            return RedirectToAction("AdminDashboard");
        }


        //************************************** Get list of data of spacific table ************************************//

        [HttpGet]
        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            var physician = _context.Physicians.ToList().Where(r => r.RegionId == regionId).ToList();
            return physician;
        }

        [HttpGet]
        public List<HealthProfessional> GetBusiness(int healthprofessionId)
        {
            var businessList = _context.HealthProfessionals.ToList().Where(m => m.Profession == healthprofessionId).ToList();
            return businessList;
        }

        public SendOrderModal GetVendorDetail(int vendorid)
        {
            var vendordetails = _context.HealthProfessionals.FirstOrDefault(m => m.VendorId == vendorid);
            var orderdata = new SendOrderModal();
            orderdata.FaxNumber = vendordetails.FaxNumber;
            orderdata.Email = vendordetails.Email;
            orderdata.BusinessContact = vendordetails.BusinessContact;
            return orderdata;
        }

        public JsonResult GetDataForAgreemenModal(int requestid)
        {
            var requestdata = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            var phonenumber = requestdata.PhoneNumber;
            var email = requestdata.Email;
            var requesttype = requestdata.RequestTypeId;

            var agreementdata = new
            {
                phonenumber = phonenumber,
                email = email,
                requesttype = requesttype,
            };

            return Json(agreementdata);
        }

        [HttpPost]
        public IActionResult SendAgreementModal(int id, string email)
        {

            string AgreementUrl = GenerateAgreementUrl(id);
            SendEmail(email, "Confirm Your Agreement", $"Hello, Click On below Link for COnfirm Agreement: {AgreementUrl}");
            TempData["success"] = "Agreement sent in Email..!";
            return RedirectToAction("AdminDashboard");
        }


        private string GenerateAgreementUrl(int reqid)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var requestid = EncryptionDecryption.EncryptStringToBase64_Aes(reqid.ToString(), key, iv);
            string AgreementPath = Url.Action("ReviewAgreement", "Admin", new { id = requestid });
            return baseUrl + AgreementPath;
        }

        public IActionResult ReviewAgreement(string id)
        {
            var viewModel = new AdminRequestViewModel();
            var requestid = int.Parse(EncryptionDecryption.DecryptStringFromBase64_Aes(id, key, iv));
            var PatienName = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            viewModel.patientName = PatienName.FirstName + " " + PatienName.LastName;
            viewModel.requestid = requestid;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SendAgreement(int reqid)
        {
            return View();
        }
        [HttpPost]

        public IActionResult SendCreatePatientRequestPageLink(string firstname, string phonenumber, string email)
        {
            string CreateRequestUrl = GenerateSendCreateRequestLinkUrl(email, phonenumber, firstname);
            SendEmail(email, "Create A Request", $"Hello, Click On below Link for Creating a request: {CreateRequestUrl}");
            TempData["success"] = "Create Request link sent in Email..!";
            return RedirectToAction("AdminDashboard");
        }
        private string GenerateSendCreateRequestLinkUrl(string email, string phonenumber, string firstname)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string AgreementPath = Url.Action("Patient", "PatientRequest", new { firstname = firstname, phonenumber = phonenumber, email = email });
            return baseUrl + AgreementPath;
        }

        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.tejpatel@outlook.com";
            var password = "7T6d2P3@K";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        public IActionResult IAgreeSendAgreement(int requestid)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            request.Status = 4;
            _context.Requests.Update(request);
            _context.SaveChanges();
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(requestid);
            return RedirectToAction("PatientDashboard", "Dashboard");
        }

        public List<AdminDashboardTableDataViewModel> ExportAllDownload()
        {
            var data = _adminDashboardDataTable.GetDataForExportAll();
            return data;
        }

        [HttpGet]
        public IActionResult CloseCase(int requestid)
        {
            var requestclient = _context.RequestClients.FirstOrDefault(m => m.RequestId == requestid);
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            var wisefiles = _context.RequestWiseFiles.ToList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();

            var datamodel = new ViewUploadViewModel
            {
                FirstName = requestclient.FirstName,
                LastName = requestclient.LastName,
                PhoneNumber = requestclient.PhoneNumber,
                DOB = new DateTime(Convert.ToInt32(requestclient.IntYear), DateTime.ParseExact(requestclient.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(requestclient.IntDate)),
                Email = requestclient.Email,
                ConfirmationNumber = request.ConfirmationNumber,
                wiseFiles = wisefiles,
                requestid = requestid,
            };
            return View(datamodel);
        }
        public IActionResult EditCloseCaseInfo(ViewUploadViewModel model, int requestid)
        {
            if (model.Email != null)
            {
                var requestclient = _context.RequestClients.FirstOrDefault(m => m.RequestId == model.requestid);
                requestclient.Email = model.Email;
                requestclient.PhoneNumber = model.PhoneNumber;
                _context.RequestClients.Update(requestclient);
                _context.SaveChanges();
                return RedirectToAction("CloseCase", new { requestid = model.requestid });
            }
            return RedirectToAction("CloseCase", new { requestid = requestid });

        }
        [HttpPost]
        public IActionResult CloseCasebtn(int requestid)
        {

            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            request.Status = 9;
            var adminid = HttpContext.Session.GetInt32("AdminId");
            //_addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(requestid, null, adminid);
            //_context.Requests.Update(request);
            //_context.SaveChanges();
            return RedirectToAction("AdminDashboard");

        }
        public IActionResult AdminProfile()
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);
            var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Id == admin.AspNetUserId);
            var rolelist = _context.AspNetRoles.ToList();
            var regionlist = _context.Regions.ToList();
            var adminregionlist = _context.AdminRegions.Where(a => a.AdminId == adminid).ToList();
            var model = new UserAllDataViewModel
            {
                UserName = aspnetuser.UserName,
                password = aspnetuser.PasswordHash,
                status = admin.Status,
                role = rolelist,
                firstname = admin.FirstName,
                lastname = admin.LastName,
                email = admin.Email,
                confirmationemail = admin.Email,
                phonenumber = admin.Mobile,
                regionlist = regionlist,
                address1 = admin.Address1,
                address2 = admin.Address2,
                city = admin.City,
                zip = admin.Zip,
                alterphonenumber = admin.AltPhone,
                adminregionlist = adminregionlist,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateAdministrationInfoAdminProfile(UserAllDataViewModel model)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _context.Admins.Include(r => r.AdminRegions).FirstOrDefault(m => m.AdminId == adminid);
            var addadminregion = new AdminRegion();
            List<int> adminRegion = admin.AdminRegions.Select(m => m.RegionId).ToList();
            var RegionToDelete = adminRegion.Except(model.selectedregion);
            foreach (var item in RegionToDelete)
            {
                AdminRegion? adminRegionToDelete = _context.AdminRegions
            .FirstOrDefault(ar => ar.AdminId == adminid && ar.RegionId == item);

                if (adminRegionToDelete != null)
                {
                    _context.AdminRegions.Remove(adminRegionToDelete);
                }
            }
            IEnumerable<int> regionsToAdd = model.selectedregion.Except(adminRegion);

            foreach (int item in regionsToAdd)
            {
                AdminRegion newAdminRegion = new AdminRegion
                {
                    AdminId = (int)adminid,
                    RegionId = item,
                };
                _context.AdminRegions.Add(newAdminRegion);
            }
            _context.SaveChanges();

            if (admin != null)
            {
                admin.FirstName = model.firstname;
                admin.LastName = model.lastname;
                admin.Email = model.email;
                admin.Mobile = model.phonenumber;
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            TempData["success"] = "Profile Updated Successfully...!";

            return RedirectToAction("AdminProfile");
        }
        [HttpPost]
        public IActionResult UpdateMailingInfoAdminProfile(UserAllDataViewModel model)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);
            //var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Id == admin.AspNetUserId);
            //var rolelist = _context.AspNetRoles.ToList();
            //var regionlist = _context.Regions.ToList();
            if (admin != null)
            {
                admin.Address1 = model.address1;
                admin.Address2 = model.address2;
                admin.City = model.city;
                admin.Zip = model.zip;
                admin.AltPhone = model.alterphonenumber;
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("AdminProfile");
        }
        public IActionResult CreatePatientRequest()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatientRequest(patientRequest req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }
            var aspuser = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Email == req.Email);
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.RegionId == user.RegionId);
            var requestcount = (from m in _context.Requests where m.CreatedDate.Date == DateTime.Now.Date select m).ToList();
            string regiondata = _context.Regions.FirstOrDefault(a => a.RegionId == user.RegionId).Abbreviation;
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = await _context.Admins.FirstOrDefaultAsync(m => m.AdminId == adminid);
            if (aspuser != null)
            {
                Request requests = new Request
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    CreatedDate = DateTime.Now,
                    RequestTypeId = 5,
                    Status = 1,
                    UserId = admin.AdminId,
                    PhoneNumber = admin.Mobile,
                    ModifiedDate = DateTime.Now,
                    //ConfirmationNumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    ConfirmationNumber = regiondata + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0')
                           + DateTime.Now.Year.ToString().Substring(2) + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) +                           (requestcount.Count() + 1).ToString().PadLeft(4, '0'),

                    //ConfirmationNumber = $"{req.FirstName.Substring(0, 2)}{req.BirthDate.ToString().Substring(0, 2)}{req.LastName.Substring(0, 2)}{req.BirthDate.ToString().Substring(3, 2)}{req.BirthDate.ToString().Substring(6, 4)}",
                };
                _context.Requests.Add(requests);
                _context.SaveChanges();
                RequestClient requestclients = new RequestClient
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    PhoneNumber = req.PhoneNumber,
                    Street = req.Street,
                    City = req.City,
                    State = req.State,
                    ZipCode = req.ZipCode,
                    RequestId = requests.RequestId,
                    RegionId = 1,
                    Notes = req.Notes,
                    Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
                    IntDate = int.Parse(req.BirthDate?.ToString("dd")),
                    IntYear = int.Parse(req.BirthDate?.ToString("yyyy")),
                    StrMonth = req.BirthDate?.ToString("MMM"),
                };
                _context.RequestClients.Add(requestclients);
                _context.SaveChanges();

                if (req.Upload != null)
                {
                    uploadFile(req.Upload, requests.RequestId);

                }
            }
            return View();

        }
    }
}