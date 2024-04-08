using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Vendor
{
    public class VendorController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public VendorController(IunitOfWork unit, IVendor Vendor)
        {
            _unitOfWork = unit;
        }
        public IActionResult Vendor()
        {
            VendorViewModel viewModel = new VendorViewModel();
            viewModel = _unitOfWork.vendor.getVendorData();
            return View(viewModel);
        }
        public IActionResult AddVendorAccount()
        {
            return View();
        }
    }
}
