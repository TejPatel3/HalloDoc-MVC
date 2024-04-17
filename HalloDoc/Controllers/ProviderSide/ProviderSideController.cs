using DataModels.AdminSideViewModels;
using HalloDoc.DataContext;
using HalloDoc.DataModels;
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
        private readonly ApplicationDbContext _context;
        private readonly IAddOrUpdateRequestNotes _addOrUpdateRequestNotes;

        public ProviderSideController(IunitOfWork unit, ApplicationDbContext context, IAddOrUpdateRequestNotes addOrUpdateRequestNotes)
        {
            _unitOfWork = unit;
            _context = context;
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
            var wisefileslist = _context.RequestWiseFiles.ToList().Where(m => m.IsDeleted == null && m.RequestId == requestid).ToList();
            var requestclient = _context.RequestClients.FirstOrDefault(m => m.RequestId == requestid);
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            var requestnote = _context.RequestNotes.FirstOrDefault(m => m.RequestId == requestid);
            var model = new ViewUploadViewModel
            {
                wiseFiles = wisefileslist,
                requestid = requestid,
                FirstName = requestclient.FirstName,
                LastName = requestclient.LastName,
                ConfirmationNumber = request.ConfirmationNumber,
                Notes = requestnote.PhysicianNotes,
            };
            var encounter = _context.Encounters.FirstOrDefault(m => m.RequestId == requestid);
            if (encounter != null)
            {
                model.isfinalize = encounter.IsFinalized.ToString();
            }
            return View(model);
        }
        public IActionResult ConcludeCareSubmit(int requestid)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            request.Status = 8;
            _context.Requests.Update(request);
            _context.SaveChanges();
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
            var phyregion = _context.PhysicianRegions.Where(m => m.PhysicianId == physicianid).ToList();
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

            var requestdata = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
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
                _context.RequestStatusLogs.Add(requeststatuslog);
                _context.Requests.Update(requestdata);
                _context.SaveChanges();
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
                _context.RequestStatusLogs.Add(requeststatuslog);
                _context.Requests.Update(requestdata);
                _context.SaveChanges();
                return Ok();
            }
            return Ok();
        }

        public IActionResult Encounter(int requestid)
        {
            var request = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
            var requestclients = _context.RequestClients.FirstOrDefault(x => x.RequestId == requestid);

            var encounter = _context.Encounters.FirstOrDefault(x => x.RequestId == requestid);
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
            else if (request.Status == 6 && encounter.IsFinalized[0] != true)
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
            //else if(request.Status == 6 && encounter.IsFinalized[0] == true)
            //{
            //    return BadRequest("Already Finalized");
            //}
            //else if ((request.Status == 6 || request.Status == 7 || request.Status == 8 || request.Status == 3) && encounter.IsFinalized != fortrue)
            //{
            //    return PartialView("_DownLoadEncounter", new { requestid = request.Requestid, role = role });
            //}
            else
            {
                return PartialView("_SelectCallType", request);
            }
        }

        [HttpPost]
        public IActionResult OnHouseOpenEncounter(int requestid)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");

            var request = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
            request.Status = 6;
            _context.Requests.Update(request);
            RequestStatusLog requeststatuslog = new RequestStatusLog();
            requeststatuslog.Status = request.Status;
            requeststatuslog.RequestId = requestid;
            requeststatuslog.Notes = "Provider click on housecall";
            requeststatuslog.CreatedDate = DateTime.Now;
            requeststatuslog.PhysicianId = physicianid;
            _context.RequestStatusLogs.Add(requeststatuslog);
            _context.SaveChanges();
            return RedirectToAction("Encounter", new { requestid = requestid });
        }

        [HttpPost]
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");

            var requestclient = _context.RequestClients.FirstOrDefault(x => x.RequestId == model.RequestId);

            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            var request = _context.Requests.FirstOrDefault(x => x.RequestId == model.RequestId);

            var encounter = _context.Encounters.FirstOrDefault(x => x.RequestId == request.RequestId);
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
            _context.Requests.Update(request);
            if (encounter.RequestId == 0)
            {
                encounter.RequestId = requestclient.RequestId;
                encounter.Date = DateTime.Now;
                //encounter. = _context.Physicians.FirstOrDefault(x => x.Physicianid == physicianid).Aspnetuserid;
                _context.Encounters.Add(encounter);
            }
            else
            {
                _context.Encounters.Update(encounter);
                //encounter.Modifieddate = DateTime.Now;
                //if (physicianid != -1)
                //{
                //    encounter.Modifiedby = _context.Physicians.FirstOrDefault(x => x.Physicianid == physicianid).Aspnetuserid;
                //}
                //if (adminid != -1)
                //{
                //    encounter.Modifiedby = _context.Admins.FirstOrDefault(x => x.Adminid == adminid).Aspnetuserid;
                //}
            }
            _context.SaveChanges();
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
            var encounter = _context.Encounters.FirstOrDefault(x => x.RequestId == requestid);
            if (encounter == null)
            {
                var enounternew = new Encounter();
                enounternew.RequestId = requestid;
                enounternew.IsFinalized = fortrue;
                enounternew.Date = DateTime.Now;

                _context.Encounters.Add(enounternew);
                _context.SaveChanges();
            }
            else
            {
                encounter.IsFinalized = fortrue;
                _context.Encounters.Update(encounter);
                _context.SaveChanges();
            }
            return Ok();
        }

        public IActionResult MyProfile()
        {
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var physiciandata = _context.Physicians.FirstOrDefault(p => p.PhysicianId == physicianid);

            var aspnetuser = _context.AspNetUsers.FirstOrDefault(x => x.Id == physiciandata.Id);
            var rolelist = _context.AspNetRoles.ToList();
            var regionlist = _context.Regions.ToList();
            var selectedregionlist = _context.PhysicianRegions.ToList().Where(a => a.PhysicianId == physicianid).ToList();
            var model = new ProviderDetailsViewModel
            {
                physicianid = physiciandata.PhysicianId,
                username = physiciandata.FirstName + " " + physiciandata.LastName,
                password = aspnetuser.PasswordHash,
                role = rolelist,
                regionlist = _context.Regions.ToList(),
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
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            var phydata = _context.Physicians.FirstOrDefault(x => x.PhysicianId == physicianid);

            var aspnetuser = _context.AspNetUsers.FirstOrDefault(x => x.Id == phydata.Id);

            aspnetuser.PasswordHash = p.password;

            TempData["success"] = "Password Updated Successfully";
            _context.AspNetUsers.Update(aspnetuser);
            _context.SaveChanges();

            return RedirectToAction("MyProfile");
        }

    }
}
