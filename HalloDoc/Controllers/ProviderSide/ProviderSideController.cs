using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.ProviderSide
{
    public class ProviderSideController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public ProviderSideController(IunitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public IActionResult ProviderDashboard()
        {
            var casetag = _unitOfWork.tableData.GetCaseTagList();
            var request = _unitOfWork.AdminDashboard.GetAll().ToList();
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
            var datalist = _unitOfWork.AdminDashboarDataTable.getallAdminDashboard(1);
            return PartialView(datalist);
        }

        public IActionResult Pending(int currentpage)
        {
            var datalist = _unitOfWork.AdminDashboarDataTable.getallAdminDashboard(2);

            return PartialView(datalist);
        }

        public IActionResult Active(int currentpage)
        {
            var datalist = _unitOfWork.AdminDashboarDataTable.getallAdminDashboard(4).Concat(_unitOfWork.AdminDashboarDataTable.getallAdminDashboard(5)).ToList();
            return View(datalist);
        }

        public IActionResult Conclude(int currentpage)
        {
            var datalist = _unitOfWork.AdminDashboarDataTable.getallAdminDashboard(6);

            return View(datalist);
        }
    }
}
