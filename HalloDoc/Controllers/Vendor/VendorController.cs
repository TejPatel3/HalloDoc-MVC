using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Vendor
{
    public class VendorController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public VendorController(IunitOfWork unit, IVendorRepository Vendor)
        {
            _unitOfWork = unit;
        }

        public IActionResult Vendor()
        {
            VendorViewModel modal = new VendorViewModel();
            modal = _unitOfWork.vendor.getVendorData();
            return View(modal);
        }

        public IActionResult VendorFilter(int professionid, string search, int vendorid)
        {
            VendorViewModel modal = new VendorViewModel();
            if (professionid == 0 && search == null && vendorid == 0)
            {
                modal = _unitOfWork.vendor.getVendorData();
            }
            else if (professionid != 0 || search != null || vendorid != 0)
            {
                modal = _unitOfWork.vendor.getFilteredVendorData(professionid, search, vendorid);
            }
            return PartialView("_VendorTable", modal);
        }
        public IActionResult AddVendorAccount()
        {
            VendorViewModel modal = new VendorViewModel();
            modal.healthProfessionalTypelist = _unitOfWork.vendor.GetProfessionList();
            return View(modal);
        }
        public IActionResult EditVendorAccount(int vendorid)
        {
            VendorViewModel modal = new VendorViewModel();
            if (vendorid != 0)
            {
                modal = _unitOfWork.vendor.EditVendorData(vendorid);
            }
            return View(modal);
        }
        [HttpPost]
        public IActionResult EditVendorAccountSubmit(VendorViewModel model)
        {
            int check = _unitOfWork.vendor.EditVendorDataSubmit(model);
            if (check == 0)
            {
                TempData["VendorDataUpdatederror"] = "Something went wrong";
            }
            else
            {
                TempData["VendorDataUpdated"] = "Update successfully";
            }
            return RedirectToAction("Vendor");
        }
        [HttpPost]
        public IActionResult AddVendorAccount(VendorViewModel model)
        {
            bool check = _unitOfWork.vendor.AddVendorAccount(model);
            if (check)
            {
                TempData["vendoraccountcreated"] = "Account Created Successfully";
            }
            else
            {
                TempData["vendoraccountcreatederror"] = "Something went wrong try again";

            }
            return RedirectToAction("Vendor");
        }
    }
}
