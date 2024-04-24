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
    [AuthorizationRepository("Admin,Physician")]
    public class AdminController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IAdminLog adminLog;
        private readonly IAdminDashboard _adminDashboard;
        private readonly IAdminDashboardDataTable _adminDashboardDataTable;
        private readonly IViewCaseRepo _viewcase;
        private readonly IBlockCaseRepository _block;
        private readonly IAddOrUpdateRequestNotes _addOrUpdateRequestNotes;
        private readonly IAddOrUpdateRequestStatusLog _addOrUpdateRequestStatusLog;
        private readonly IJwtRepository _jwtRepo;
        private readonly IAdd _add;
        public AdminController(
        IAdminLog _admin, IAdminDashboard adminDashboard,
        IAdminDashboardDataTable adminDashboardDataTable,
        IViewCaseRepo viewcase, IBlockCaseRepository block,
        IAddOrUpdateRequestStatusLog addOrUpdateRequestStatusLog,
        IAddOrUpdateRequestNotes addOrUpdateRequestNotes,
        IJwtRepository jwtRepo, IAdd add, IunitOfWork unitOfWork)
        {
            adminLog = _admin;
            _adminDashboard = adminDashboard;
            _adminDashboardDataTable = adminDashboardDataTable;
            _viewcase = viewcase;
            _block = block;
            _addOrUpdateRequestNotes = addOrUpdateRequestNotes;
            _addOrUpdateRequestStatusLog = addOrUpdateRequestStatusLog;
            _jwtRepo = jwtRepo;
            _add = add;
            _unitOfWork = unitOfWork;
        }


        //************************************** Admin Dashboard ************************************//
        public IActionResult AdminDashboard()
        {
            var casetag = _unitOfWork.tableData.GetCaseTagList();
            var request = _adminDashboard.GetAll().ToList();
            var viewmodel = new AdminDashboardTableDataViewModel();
            var region = _unitOfWork.tableData.GetRegionList();
            var physician = _unitOfWork.tableData.GetPhysicianList();
            AdminRequestViewModel viewModel = new AdminRequestViewModel();
            viewModel.requests = request;
            viewModel.regions = region;
            viewModel.caseTags = casetag;
            return View(viewModel);
        }

        public IActionResult New()
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(1);
            return PartialView(datalist);
        }

        public IActionResult Pending(int currentpage)
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(2);
            return PartialView(datalist);
        }

        public IActionResult Active(int currentpage)
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(4).Concat(_adminDashboardDataTable.getallAdminDashboard(5)).ToList();
            return View(datalist);
        }

        public IActionResult Conclude(int currentpage)
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(6);
            return View(datalist);
        }

        public IActionResult ToClose(int currentpage)
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(7).Concat(_adminDashboardDataTable.getallAdminDashboard(3)).Concat(_adminDashboardDataTable.getallAdminDashboard(8)).ToList();
            return View(datalist);
        }

        public IActionResult Unpaid(int currentpage)
        {
            var datalist = _adminDashboardDataTable.getallAdminDashboard(9);
            return View(datalist);
        }


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
            var req = _unitOfWork.tableData.GetRequestByConfirmationNumber(request.ConfirmationNumber);
            TempData["success"] = "Updated Successfully..!";
            return RedirectToAction("ViewCase", new { id = req.RequestId });
        }

        public IActionResult ViewNotes(int reqid)
        {
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(reqid);
            var requestnote = _unitOfWork.tableData.GetRequestNoteByRequestId(reqid);
            var viewnote = new AdminRequestViewModel();
            if (requestnote != null)
            {
                viewnote.BlockNotes = requestnote.AdminNotes;
            }
            var adminname = HttpContext.Session.GetString("AdminName");
            viewnote.requestid = reqid;
            var transfernotedetail = _unitOfWork.tableData.GetRequestStatusLogByRequestIdStatus(reqid, 2);
            if (transfernotedetail != null)
            {
                var physicianname = _unitOfWork.tableData.GetPhysicianFirstOrDefault(transfernotedetail.TransToPhysicianId);
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
            var wisefileslist = _unitOfWork.tableData.GetRequestWiseFileList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();
            var requestclient = _unitOfWork.tableData.GetRequestClientByRequestId(requestid);
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
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
            var professiontypeList = _unitOfWork.tableData.GetHealthProfessionalTypeList();
            SendOrderModal orderdata = new SendOrderModal();
            orderdata.requestid = requestid;
            orderdata.Healthprofessionaltypes = professiontypeList;
            return View(orderdata);
        }

        [HttpPost]
        public IActionResult Orders(int requestid, int vendorid, string prescription, int refill)
        {
            var venderDetails = _unitOfWork.tableData.GetHealthProfessionalByVendorId(vendorid);
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
                _unitOfWork.Add.AddOrderDetails(order);
            }
            TempData["success"] = "Your order placed successfully..!";
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                return RedirectToAction("ProviderDashboard", "ProviderSide");
            }
            else
            {
                return RedirectToAction("AdminDashboard");
            }
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
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                return RedirectToAction("ConcludeCare", "ProviderSide", new { requestid = id });
            }
            else
            {
                return RedirectToAction("ViewUpload", new { requestid = id });
            }
        }

        public void uploadFile(List<IFormFile> file, int id)
        {
            foreach (var item in file)
            {
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
                _unitOfWork.Add.AddRequestWiseFile(requestWiseFiles);
            }
        }

        [HttpPost]
        public IActionResult DeleteDoc(int wiseid, int reqId)
        {
            var wisefile = _unitOfWork.tableData.GetRequestWiseFileById(wiseid);
            BitArray t = new BitArray(1);
            t[0] = true;
            wisefile.IsDeleted = t;
            _unitOfWork.UpdateData.UpdateRequestWiseFile(wisefile);
            TempData["success"] = "Document Deleted Successfully..!";
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                return RedirectToAction("ConcludeCare", "ProviderSide", new { requestid = reqId });
            }
            else
            {
                return RedirectToAction("ViewUpload", new { requestid = reqId });
            }
        }

        [HttpPost]
        public IActionResult SendMail(List<int> wiseFileId, int reqid)
        {
            List<string> filenames = new List<string>();
            foreach (var item in wiseFileId)
            {
                var file = _unitOfWork.tableData.GetRequestWiseFileById(item).FileName;
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
                    IsBodyHtml = true
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
            var req = _unitOfWork.tableData.GetRequestByConfirmationNumber(confirmation);
            req.Status = 3;
            _unitOfWork.UpdateData.UpdateRequest(req);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public IActionResult cancelCaseModal(int id, AdminRequestViewModel cancelnote, string casetagname)
        {
            var req = _unitOfWork.tableData.GetRequestFirstOrDefault(id);
            req.Status = 3;
            var casetag = _unitOfWork.tableData.GetCaseTagByName(casetagname);
            if (casetag != null)
            {
                req.CaseTag = casetag.CaseTagId.ToString();
            }
            _unitOfWork.UpdateData.UpdateRequest(req);
            var adminid = HttpContext.Session.GetInt32("AdminId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, cancelnote.BlockNotes, adminid);
            TempData["success"] = "Request Canceled Successfully..!";
            return RedirectToAction("AdminDashboard");
        }


        [HttpPost]
        public IActionResult AssignCase(int id, AdminRequestViewModel assignnote, string physicianid)
        {
            var req = _unitOfWork.tableData.GetRequestFirstOrDefault(id);
            var physiciandetail = _unitOfWork.tableData.GetPhysicianFirstOrDefault(int.Parse(physicianid));
            req.PhysicianId = physiciandetail.PhysicianId;
            //req.Status = 2;
            if (req.DeclinedBy != null)
            {
                req.DeclinedBy = null;
            }
            _unitOfWork.UpdateData.UpdateRequest(req);
            var adminid = HttpContext.Session.GetInt32("AdminId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, assignnote.BlockNotes, adminid, physiciandetail.PhysicianId);
            TempData["success"] = "Request successfully Assigned..!";
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult BlockModal(int id, AdminRequestViewModel blocknote)
        {
            var req = _unitOfWork.tableData.GetRequestFirstOrDefault(id);
            _block.BlockPatient(id, blocknote.BlockNotes);
            TempData["success"] = "Request Blocked Successfully..!";
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult ClearModal(int id)
        {
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(id);
            if (request != null)
            {
                request.Status = 10;
                _unitOfWork.UpdateData.UpdateRequest(request);
            }
            var adminid = HttpContext.Session.GetInt32("AdminId");
            _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, null, adminid);
            return RedirectToAction("AdminDashboard");
        }


        //************************************** Get list of data of spacific table ************************************//

        [HttpGet]
        public List<Physician> GetPhysicianByRegionId(int regionId)
        {
            var physician = _unitOfWork.tableData.GetPhysicianList().Where(r => r.RegionId == regionId).ToList();
            return physician;
        }

        [HttpGet]
        public List<HealthProfessional> GetBusiness(int healthprofessionId)
        {
            var businessList = _unitOfWork.tableData.GetHealthProfessionalList().Where(m => m.Profession == healthprofessionId).ToList();
            return businessList;
        }

        public SendOrderModal GetVendorDetail(int vendorid)
        {
            var vendordetails = _unitOfWork.tableData.GetHealthProfessionalByVendorId(vendorid);
            var orderdata = new SendOrderModal();
            orderdata.FaxNumber = vendordetails.FaxNumber;
            orderdata.Email = vendordetails.Email;
            orderdata.BusinessContact = vendordetails.BusinessContact;
            return orderdata;
        }

        public JsonResult GetDataForAgreemenModal(int requestid)
        {
            var requestdata = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
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
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                return RedirectToAction("ProviderDashboard", "ProviderSide");
            }
            else
            {
                return RedirectToAction("AdminDashboard");
            }
        }

        private string GenerateAgreementUrl(int reqid)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var requestid = EncryptionDecryption.EncryptStringToBase64_Aes(reqid.ToString());
            string AgreementPath = Url.Action("ReviewAgreement", "Home", new { id = requestid });
            return baseUrl + AgreementPath;
        }

        [HttpPost]
        public IActionResult SendAgreement(int reqid)
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendCreatePatientRequestPageLink(string firstname, string phonenumber, string email)
        {
            if (firstname == null || phonenumber == null || email == null)
            {
                TempData["error"] = "Something Went wrong try again..!";
            }
            else
            {
                string CreateRequestUrl = GenerateSendCreateRequestLinkUrl(email, phonenumber, firstname);
                SendEmail(email, "Create A Request", $"Hello, Click On below Link for Creating a request: {CreateRequestUrl}");
                TempData["success"] = "Create Request link sent in Email..!";
            }
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                return RedirectToAction("ProviderDashboard", "ProviderSide");
            }
            else
            {
                return RedirectToAction("AdminDashboard");
            }
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

        public bool IAgreeSendAgreement(int requestid)
        {
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            bool check = true;
            if (request.Status == 3 || request.Status == 4)
            {
                check = false;
                return check;
            }
            else
            {
                request.Status = 4;
                _unitOfWork.UpdateData.UpdateRequest(request);
                _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(requestid);
                return check;
            }
        }
        [HttpGet]
        public List<AdminDashboardTableDataViewModel> ExportAllDownload()
        {
            var data = _adminDashboardDataTable.GetDataForExportAll();
            return data;
        }

        [HttpGet]
        public IActionResult CloseCase(int requestid)
        {
            var requestclient = _unitOfWork.tableData.GetRequestClientByRequestId(requestid);
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            var wisefiles = _unitOfWork.tableData.GetRequestWiseFileList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();
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
                var requestclient = _unitOfWork.tableData.GetRequestClientByRequestId(model.requestid);
                requestclient.Email = model.Email;
                requestclient.PhoneNumber = model.PhoneNumber;
                _unitOfWork.UpdateData.UpdateRequestClient(requestclient);
                return RedirectToAction("CloseCase", new { requestid = model.requestid });
            }
            return RedirectToAction("CloseCase", new { requestid = requestid });

        }
        [HttpPost]
        public IActionResult CloseCasebtn(int requestid)
        {

            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            request.Status = 9;
            _unitOfWork.UpdateData.UpdateRequest(request);
            var adminid = HttpContext.Session.GetInt32("AdminId");
            return RedirectToAction("AdminDashboard");

        }
        public IActionResult AdminProfile()
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(admin.AspNetUserId);
            var rolelist = _unitOfWork.tableData.GetAspNetRoleList();
            var regionlist = _unitOfWork.tableData.GetRegionList();
            var adminregionlist = _unitOfWork.tableData.GetAdminRegionListByAdminId(adminid);
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
                check = true,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateAdministrationInfoAdminProfile(UserAllDataViewModel model)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
            var addadminregion = new AdminRegion();
            List<int> adminRegion = admin.AdminRegions.Select(m => m.RegionId).ToList();
            var RegionToDelete = adminRegion.Except(model.selectedregion);
            foreach (var item in RegionToDelete)
            {
                AdminRegion? adminRegionToDelete = _unitOfWork.tableData.GetAdminRegionListByAdminId(adminid).FirstOrDefault(ar => ar.RegionId == item);
                if (adminRegionToDelete != null)
                {
                    _unitOfWork.RemoveData.RemoveAdminRegion(adminRegionToDelete);
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
                _unitOfWork.Add.AddAdminRegion(newAdminRegion);
            }
            if (admin != null)
            {
                admin.FirstName = model.firstname;
                admin.LastName = model.lastname;
                admin.Email = model.email;
                admin.Mobile = model.phonenumber;
            }
            _unitOfWork.UpdateData.UpdateAdmin(admin);
            TempData["success"] = "Profile Updated Successfully...!";
            return RedirectToAction("AdminProfile");
        }
        [HttpPost]
        public IActionResult UpdateMailingInfoAdminProfile(UserAllDataViewModel model)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
            if (admin != null)
            {
                admin.Address1 = model.address1;
                admin.Address2 = model.address2;
                admin.City = model.city;
                admin.Zip = model.zip;
                admin.AltPhone = model.alterphonenumber;
            }
            _unitOfWork.UpdateData.UpdateAdmin(admin);
            TempData["success"] = "Profile Updated Successfully...!";
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
            var aspuser = _unitOfWork.tableData.GetAspNetUserByEmail(req.Email);
            var user = _unitOfWork.tableData.GetUserByEmail(req.Email);
            var region = _unitOfWork.tableData.GetRegionByRegionId(user.RegionId);
            var requestcount = _unitOfWork.tableData.GetRequestList().Where(m => m.CreatedDate.Date == DateTime.Now.Date).ToList();
            string regiondata = _unitOfWork.tableData.GetRegionByRegionId(user.RegionId).Abbreviation;
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
            if (aspuser != null)
            {
                Request requests = new Request
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    CreatedDate = DateTime.Now,
                    RequestTypeId = 1,
                    Status = 1,
                    UserId = user.UserId,
                    PhoneNumber = admin.Mobile,
                    ModifiedDate = DateTime.Now,
                    ConfirmationNumber = regiondata + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0')
                           + DateTime.Now.Year.ToString().Substring(2) + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) +
                           (requestcount.Count() + 1).ToString().PadLeft(4, '0'),
                };
                _unitOfWork.Add.AddRequest(requests);

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
                _unitOfWork.Add.AddRequestClient(requestclients);
                if (req.Upload != null)
                {
                    uploadFile(req.Upload, requests.RequestId);
                }
            }
            TempData["success"] = "Request created successfully";
            return RedirectToAction("AdminDashboard");

        }
        public IActionResult CreateAdmin()
        {
            UserAllDataViewModel model = new UserAllDataViewModel();
            model.regionlist = _unitOfWork.tableData.GetRegionList();
            var rolelist = _unitOfWork.tableData.GetRoleList().Where(m => m.AccountType == 0 || m.AccountType == 1).ToList();
            model.rolelist = rolelist;
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateAdminAccount(UserAllDataViewModel obj)
        {
            int adminid = (int)HttpContext.Session.GetInt32("AdminId");
            _add.AddAdmin(obj, adminid);
            TempData["success"] = "Admin Account created successfully";
            return RedirectToAction("AdminDashboard");
        }
    }
}