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
    }
}
