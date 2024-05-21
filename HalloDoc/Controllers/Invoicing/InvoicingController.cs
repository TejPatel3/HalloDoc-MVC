using DataModels.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Invoicing
{
    public class InvoicingController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public InvoicingController(IunitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public IActionResult Invoicing()
        {
            InvoicingViewAll invoicingViewAll = new InvoicingViewAll
            {
                Physicians = _unitOfWork.tableData.GetPhysicianList().OrderBy(o => o.FirstName).ToList(),
            };
            return View(invoicingViewAll);
        }

        public IActionResult GetTimeSheetData(string monthHalf, int phyId)
        {
            int month = Convert.ToInt32(monthHalf.Substring(0, monthHalf.IndexOf("-")));
            int half = Convert.ToInt32(monthHalf.Substring(monthHalf.IndexOf("-") + 1, 1));

            InvoicingViewAll result = _unitOfWork.ProviderSite.GetAdminTimesheetData(DateTime.Now.Year, month, half, phyId);

            return PartialView("_InvoicingTableData", result);

        }

        public IActionResult ApproveTimesheet(int month, int half, int phyId)
        {
            Timesheet timesheet = _unitOfWork.ProviderSite.GetTimesheet(DateTime.Now.Year, month, half, phyId);
            InvoicingViewAll invoicing = new InvoicingViewAll
            {
                selectedPhyId = phyId,
                Month = month,
                Half = half,
                TimeSheetId = timesheet.TimesheetId,
            };
            return View(invoicing);
        }

        public IActionResult GetAdminApproveSheetData(int month, int half, int phyId)
        {
            InvoicingViewAll data = _unitOfWork.ProviderSite.GetAdminApproveSheetData(DateTime.Now.Year, month, half, phyId);
            return PartialView("_TimesheetApproveTables", data);
        }

        public IActionResult Payrate(int phyId)
        {
            PayrateView model = new PayrateView
            {
                PhysicianId = phyId,
            };
            return View(model);
        }

        public IActionResult GetPayrateDetails(int phyId)
        {
            PayrateView model = _unitOfWork.ProviderSite.GetPayrateData(phyId);
            return PartialView("_Payrate", model);
        }

        [HttpPost]
        public IActionResult UpdatePayrate(int phyId, int payrateValue, string payrateType)
        {
            if (phyId != 0 && !string.IsNullOrEmpty(payrateType))
            {
                _unitOfWork.ProviderSite.UpdatePayrateData(phyId, payrateValue, payrateType);
                PayrateView model = _unitOfWork.ProviderSite.GetPayrateData(phyId);
                return PartialView("_Payrate", model);
            }
            else
            {
                return Json(new { fail = true, message = "Physician Not found" });
            }
        }

        [HttpPost]
        public IActionResult ApproveTimeSheet(InvoicingViewAll model)
        {
            string ApprovedByAspId = HttpContext.Session.GetString("AspUserId");
            model.ApprovedBy = ApprovedByAspId;
            if (_unitOfWork.ProviderSite.ApproveTimeSheet(model))
            {
                TempData["success"] = "Timesheet Approved Successfully";
                return RedirectToAction("Invoicing");
            }
            else
            {
                TempData["error"] = "Failed to approve Timesheet";
                return RedirectToAction("Invoicing");
            }
        }

    }
}
