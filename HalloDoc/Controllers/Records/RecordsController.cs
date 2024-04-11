using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;

namespace HalloDoc.Controllers.Records
{
    public class RecordsController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public RecordsController(IunitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public IActionResult PatientHistory()
        {
            return View();
        }
        public IActionResult GetPatientGistoryTable(string firstname, string lastname, string email, int phone)
        {
            PatientHistoryViewModel viewModel = new PatientHistoryViewModel();
            viewModel.UserList = _unitOfWork.record.GetUserList(firstname, lastname, email, phone);
            return PartialView("_PatientHistoryTable", viewModel);
        }
        public IActionResult PatientRecord(int id)
        {
            List<PatientRecordViewModel> records = new List<PatientRecordViewModel>();
            records = _unitOfWork.record.GetPatientRecordData(id);
            return View(records);
        }
        public IActionResult EmailLog()
        {
            var model = _unitOfWork.record.GetAspNetRoleList();
            return View(model);
        }
        [HttpGet]
        //public IActionResult GetEmailLogTable(int Role, string ReceiverName, string Email, string CreatedDate, string SentData)
        public IActionResult GetEmailLogTable()
        {
            var model = _unitOfWork.record.GetEmailLogs();

            return PartialView("_EmailLogTable", model);
        }
    }
}
