using DataModels.AdminSideViewModels;
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
using System.IdentityModel.Tokens.Jwt;

namespace HalloDoc.Controllers.ProviderSide
{
    [AuthorizationRepository("Admin,Physician")]
    public class ProviderSideController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IAddOrUpdateRequestNotes _addOrUpdateRequestNotes;
        public ProviderSideController(IunitOfWork unit, ApplicationDbContext context, IAddOrUpdateRequestNotes addOrUpdateRequestNotes)
        {
            _unitOfWork = unit;
            _addOrUpdateRequestNotes = addOrUpdateRequestNotes;
        }
        public IActionResult ProviderDashboard()
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var casetag = _unitOfWork.tableData.GetCaseTagList();
            var request = _unitOfWork.AdminDashboard.GetAll().ToList();
            var viewmodel = new AdminDashboardTableDataViewModel();
            var region = _unitOfWork.tableData.GetRegionList();
            var physician = _unitOfWork.tableData.GetPhysicianList();
            AdminRequestViewModel viewModel = new AdminRequestViewModel();
            viewModel.requests = request.Where(m => m.PhysicianId == physicianid).ToList();
            viewModel.regions = region;
            viewModel.caseTags = casetag;
            return View(viewModel);
        }
        public IActionResult New()
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var datalist = _unitOfWork.AdminDashboarDataTable.getallProviderDashboard(1, (int)physicianid);
            return PartialView(datalist);
        }

        public IActionResult Pending(int currentpage)
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var datalist = _unitOfWork.AdminDashboarDataTable.getallProviderDashboard(2, (int)physicianid);
            return PartialView(datalist);
        }

        public IActionResult Active(int currentpage)
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var datalist = _unitOfWork.AdminDashboarDataTable.getallProviderDashboard(4, (int)physicianid).Concat(_unitOfWork.AdminDashboarDataTable.getallProviderDashboard(5, (int)physicianid)).ToList();
            return View(datalist);
        }

        public IActionResult Conclude(int currentpage)
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var datalist = _unitOfWork.AdminDashboarDataTable.getallProviderDashboard(6, (int)physicianid);
            return View(datalist);
        }

        public IActionResult AcceptRequest(int requestid)
        {
            var result = _unitOfWork.UpdateData.UpdateRequestTable(requestid, 2);
            return RedirectToAction("ProviderDashboard");
        }

        public IActionResult DeclineRequest(int requestid)
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var result = _unitOfWork.UpdateData.DeclineRequestTable(requestid, (int)physicianid);
            return RedirectToAction("ProviderDashboard");
        }

        [HttpPost]
        public IActionResult TransferModal(int requestid, AdminRequestViewModel note)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            _unitOfWork.AdminDashboarDataTable.TransferCase(requestid, note, physicianid);
            return RedirectToAction("ProviderDashboard");
        }
        public IActionResult ConcludeCare(int requestid)
        {
            var wisefileslist = _unitOfWork.tableData.GetRequestWiseFileList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();
            var requestclient = _unitOfWork.tableData.GetRequestClientByRequestId(requestid);
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            var requestnote = _unitOfWork.tableData.GetRequestNoteByRequestId(requestid);
            var model = new ViewUploadViewModel
            {
                requestid = requestid,
                FirstName = requestclient.FirstName,
                LastName = requestclient.LastName,
                ConfirmationNumber = request.ConfirmationNumber,
            };
            if (requestnote != null)
            {
                model.Notes = requestnote.PhysicianNotes;
            }
            if (wisefileslist.Count > 0)
            {
                model.wiseFiles = wisefileslist;
            }
            var encounter = _unitOfWork.tableData.GetEncounterByRequestId(requestid);
            if (encounter != null)
            {
                model.isfinalize = encounter.IsFinalized.ToString();
            }
            return View(model);
        }
        public IActionResult ConcludeCareSubmit(int requestid)
        {
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            request.Status = 8;
            _unitOfWork.UpdateData.UpdateRequest(request);
            return RedirectToAction("ProviderDashboard");
        }
        [HttpPost]
        public IActionResult PhysicianNotes(AdminRequestViewModel obj)
        {
            _addOrUpdateRequestNotes.PhysicianRequestNotes(obj);
            return RedirectToAction("ProviderDashboard");
        }
        public IActionResult MyScheduling()
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");

            SchedulingViewModel modal = new SchedulingViewModel();
            var phyregion = _unitOfWork.tableData.GetPhysicianRegionListByPhysicianId(physicianid);
            var list = new List<Region>();
            var repolist = _unitOfWork.tableData.GetRegionList();
            foreach (var item in phyregion)
            {
                var listin = repolist.Where(m => m.RegionId == item.RegionId).First();
                list.Add(listin);
            }
            modal.regions = list;
            return View(modal);
        }

        //Encounter form
        public IActionResult EncounterSubmit(int requestid, string encountervalue)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var requestdata = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            if (encountervalue == "Consult" && requestdata != null)
            {
                requestdata.Status = 6;
                requestdata.CallType = 2;
                RequestStatusLog requeststatuslog = new RequestStatusLog();
                requeststatuslog.Status = requestdata.Status;
                requeststatuslog.RequestId = requestdata.RequestId;
                requeststatuslog.Notes = "Provider choose for consultunt";
                requeststatuslog.CreatedDate = DateTime.Now;
                requeststatuslog.PhysicianId = physicianid;
                _unitOfWork.Add.AddRequestStatusLog(requeststatuslog);
                _unitOfWork.UpdateData.UpdateRequest(requestdata);
                return RedirectToAction("Encounter", new { requestid = requestid });
            }
            else if (encountervalue == "Housecall" && requestdata != null)
            {
                requestdata.Status = 5;
                RequestStatusLog requeststatuslog = new RequestStatusLog();
                requeststatuslog.Status = requestdata.Status;
                requeststatuslog.RequestId = requestdata.RequestId;
                requeststatuslog.Notes = "Provider choose for housecall";
                requeststatuslog.CreatedDate = DateTime.Now;
                requeststatuslog.PhysicianId = physicianid;
                _unitOfWork.Add.AddRequestStatusLog(requeststatuslog);
                _unitOfWork.UpdateData.UpdateRequest(requestdata);
                return Ok();
            }
            return Ok();
        }

        public IActionResult Encounter(int requestid)
        {
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            var requestclients = _unitOfWork.tableData.GetRequestClientByRequestId(requestid);
            var encounter = _unitOfWork.tableData.GetEncounterByRequestId(requestid);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var requestcookie = HttpContext.Request;
            var token = requestcookie.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            if (request.Status == 4 || (request.Status == 5 && request.CallType == 1))
            {
                return PartialView("_SelectCallType", request);
            }
            if (request.Status == 6 && encounter == null)
            {
                EncounterFormViewModel model = new EncounterFormViewModel();
                model.Firstname = requestclients.FirstName;
                model.Lastname = requestclients.LastName;
                model.DOB = new DateTime(Convert.ToInt32(requestclients.IntYear), DateTime.ParseExact(requestclients.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, (int)requestclients.IntDate).ToString("yyyy-MM-dd");
                model.Mobile = requestclients.PhoneNumber;
                model.Email = requestclients.Email;
                model.Location = requestclients.Address;
                model.isFinaled = !fortrue[0];
                model.RequestId = request.RequestId;
                return View(model);
            }
            else if (request.Status == 6 && encounter.IsFinalized[0] != true || role == "Admin")
            {
                EncounterFormViewModel model = new EncounterFormViewModel();
                model.RequestId = request.RequestId;
                model.Firstname = requestclients.FirstName;
                model.Lastname = requestclients.LastName;
                model.DOB = new DateTime(Convert.ToInt32(requestclients.IntYear), DateTime.ParseExact(requestclients.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, (int)requestclients.IntDate).ToString("yyyy-MM-dd");
                model.Mobile = requestclients.PhoneNumber;
                model.Email = requestclients.Email;
                model.Location = requestclients.Address;
                model.isFinaled = !fortrue[0];
                model.HistoryOfIllness = encounter.HistoryIllness;
                model.MedicalHistory = encounter.MedicalHistory;
                model.Medication = encounter.Medications;
                model.Allergies = encounter.Allergies;
                model.Temp = encounter.Temp;
                model.HR = encounter.Hr;
                model.RR = encounter.Rr;
                model.BPs = encounter.BpS;
                model.BPd = encounter.BpD;
                model.O2 = encounter.O2;
                model.Pain = encounter.Pain;
                model.Heent = encounter.Heent;
                model.CV = encounter.Cv;
                model.Chest = encounter.Chest;
                model.ABD = encounter.Abd;
                model.Extr = encounter.Extr;
                model.Skin = encounter.Skin;
                model.Neuro = encounter.Neuro;
                model.Other = encounter.Other;
                model.Diagnosis = encounter.Diagnosis;
                model.TreatmentPlan = encounter.TreatmentPlan;
                model.MedicationsDispended = encounter.MedicationDispensed;
                model.Procedure = encounter.Procedures;
                model.Followup = encounter.FollowUp;
                model.role = role;
                return View(model);
            }
            else
            {
                return PartialView("_SelectCallType", request);
            }
        }

        [HttpPost]
        public IActionResult OnHouseOpenEncounter(int requestid)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(requestid);
            request.Status = 6;
            _unitOfWork.UpdateData.UpdateRequest(request);
            RequestStatusLog requeststatuslog = new RequestStatusLog();
            requeststatuslog.Status = request.Status;
            requeststatuslog.RequestId = requestid;
            requeststatuslog.Notes = "Provider click on housecall";
            requeststatuslog.CreatedDate = DateTime.Now;
            requeststatuslog.PhysicianId = physicianid;
            _unitOfWork.Add.AddRequestStatusLog(requeststatuslog);
            return RedirectToAction("Encounter", new { requestid = requestid });
        }

        [HttpPost]
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var requestclient = _unitOfWork.tableData.GetRequestClientByRequestId(model.RequestId);
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            var request = _unitOfWork.tableData.GetRequestFirstOrDefault(model.RequestId);
            var encounter = _unitOfWork.tableData.GetEncounterByRequestId(request.RequestId);
            if (encounter == null)
            {
                encounter = new Encounter();
            }
            requestclient.FirstName = model.Firstname;
            requestclient.LastName = model.Lastname;
            requestclient.PhoneNumber = model.Mobile;
            requestclient.Email = model.Email;
            requestclient.Address = model.Location;
            encounter.HistoryIllness = model.HistoryOfIllness;
            encounter.MedicalHistory = model.MedicalHistory;
            encounter.Medications = model.Medication;
            encounter.Allergies = model.Allergies;
            encounter.Temp = model.Temp;
            encounter.Hr = model.HR;
            encounter.Rr = model.RR;
            encounter.BpS = model.BPs;
            encounter.BpD = model.BPd;
            encounter.O2 = model.O2;
            encounter.Pain = model.Pain;
            encounter.Heent = model.Heent;
            encounter.Cv = model.CV;
            encounter.Chest = model.Chest;
            encounter.Abd = model.ABD;
            encounter.Extr = model.Extr;
            encounter.Skin = model.Skin;
            encounter.Neuro = model.Neuro;
            encounter.Other = model.Other;
            encounter.Diagnosis = model.Diagnosis;
            encounter.TreatmentPlan = model.TreatmentPlan;
            encounter.MedicationDispensed = model.MedicationsDispended;
            encounter.Procedures = model.Procedure;
            encounter.FollowUp = model.Followup;
            _unitOfWork.UpdateData.UpdateRequest(request);
            if (encounter.RequestId == 0)
            {
                encounter.RequestId = requestclient.RequestId;
                encounter.Date = DateTime.Now;
                _unitOfWork.Add.AddEncounter(encounter);
            }
            else
            {
                _unitOfWork.UpdateData.UpdateEncounter(encounter);
            }
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var requestcookie = HttpContext.Request;
            var token = requestcookie.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else
            {
                return RedirectToAction("ProviderDashboard", "ProviderSide");
            }
        }

        [HttpPost]
        public IActionResult FinalizeEncounter(int requestid)
        {
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            var encounter = _unitOfWork.tableData.GetEncounterByRequestId(requestid);
            if (encounter == null)
            {
                var enounternew = new Encounter();
                enounternew.RequestId = requestid;
                enounternew.IsFinalized = fortrue;
                enounternew.Date = DateTime.Now;
                _unitOfWork.Add.AddEncounter(enounternew);
            }
            else
            {
                encounter.IsFinalized = fortrue;
                _unitOfWork.UpdateData.UpdateEncounter(encounter);
            }
            return Ok();
        }

        public IActionResult MyProfile()
        {
            var physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var physiciandata = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(physiciandata.Id);
            var rolelist = _unitOfWork.tableData.GetAspNetRoleList();
            var regionlist = _unitOfWork.tableData.GetRegionList();
            var selectedregionlist = _unitOfWork.tableData.GetPhysicianRegionListByPhysicianId(physicianid);
            var model = new ProviderDetailsViewModel
            {
                physicianid = physiciandata.PhysicianId,
                username = physiciandata.FirstName + " " + physiciandata.LastName,
                password = aspnetuser.PasswordHash,
                role = rolelist,
                regionlist = _unitOfWork.tableData.GetRegionList(),
                firstname = physiciandata.FirstName,
                lastname = physiciandata.LastName,
                email = physiciandata.Email,
                phonenumber = physiciandata.Mobile,
                address1 = physiciandata.Address1,
                address2 = physiciandata.Address2,
                city = physiciandata.City,
                zip = physiciandata.Zip,
                alterphonenumber = physiciandata.AltPhone,
                selectedregionlist = selectedregionlist,
                businessname = physiciandata.BusinessName,
                businesswebsite = physiciandata.BusinessWebsite,
                photo = physiciandata.Photo,
                signature = physiciandata.Signature,
                adminnote = physiciandata.AdminNotes,
                npinumber = physiciandata.Npinumber,
                medicallicencenumber = physiciandata.MedicalLicense,
                IsAgreementDoc = physiciandata.IsAgreementDoc,
                IsCredentialDoc = physiciandata.IsCredentialDoc,
                IsBackgroundDoc = physiciandata.IsBackgroundDoc,
                IsLicenseDoc = physiciandata.IsLicenseDoc,
                IsNonDisclosureDoc = physiciandata.IsNonDisclosureDoc
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePhysicianInfo(ProviderDetailsViewModel p)
        {
            var physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var phydata = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(phydata.Id);
            aspnetuser.PasswordHash = p.password;
            TempData["success"] = "Password Updated Successfully";
            _unitOfWork.UpdateData.UpdateAspNetUser(aspnetuser);
            return RedirectToAction("MyProfile");
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
            var physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");
            var physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            if (aspuser != null)
            {
                Request requests = new Request
                {
                    FirstName = physician.FirstName,
                    LastName = physician.LastName,
                    Email = physician.Email,
                    CreatedDate = DateTime.Now,
                    RequestTypeId = 1,
                    Status = 1,
                    UserId = user.UserId,
                    PhoneNumber = physician.Mobile,
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
    }
}
