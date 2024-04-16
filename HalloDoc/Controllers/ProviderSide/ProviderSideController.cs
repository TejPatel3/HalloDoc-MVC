using HalloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.ProviderSide
{
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

        [HttpPost]        public IActionResult TransferModal(int requestid, AdminRequestViewModel note)        {            int physicianid = (int)HttpContext.Session.GetInt32("PhysicianId");            _unitOfWork.AdminDashboarDataTable.TransferCase(requestid, note, physicianid);            return RedirectToAction("ProviderDashboard");        }
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
                Notes = requestnote.PhysicianNotes
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult PhysicianNotes(AdminRequestViewModel obj)
        {
            _addOrUpdateRequestNotes.PhysicianRequestNotes(obj);
            return RedirectToAction("ProviderDashboard");
        }
    }
}
