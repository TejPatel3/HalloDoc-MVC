using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;

namespace HalloDoc.Controllers.Records
{
    [AuthorizationRepository("Admin,Physician")]

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
        public IActionResult GetEmailLogTable(int Role, string ReceiverName, string Email, DateTime CreatedDate, DateTime SentDate)
        {
            var model = _unitOfWork.record.GetEmailLogs(Role, ReceiverName, Email, CreatedDate, SentDate);
            return PartialView("_EmailLogTable", model);
        }
        public IActionResult SMSLog()
        {
            var model = _unitOfWork.record.GetAspNetRoleList();
            return View(model);
        }
        [HttpGet]
        public IActionResult GetSMSLogTable(int Role, string ReceiverName, string Email, DateTime CreatedDate, DateTime SentDate)
        {
            var model = _unitOfWork.record.GetSMSLogs(Role, ReceiverName, Email, CreatedDate, SentDate);
            return PartialView("_SMSLogTable", model);
        }

        //for search recordview
        public IActionResult SearchRecords()
        {
            return View();
        }

        //table search record
        public IActionResult SearchRecordsFilter(
            int reqstatus,
            string patientname,
            int requesttype,
            string fromdateofservice,
            string todateofservice,
            string physicianname,
            string email,
            string phonenumber
            )
        {
            List<RecordViewModel> records = new List<RecordViewModel>();
            records = _unitOfWork.record.GetSearchRecordData(reqstatus, patientname, requesttype, fromdateofservice, todateofservice, physicianname, email, phonenumber);
            return PartialView("_SearchRecordsTable", records);
        }
        //download search record excel file
        public string DownloadSearchRecord(
             int reqstatus,
            string patientname,
            int requesttype,
            string fromdateofservice,
            string todateofservice,
            string physicianname,
            string email,
            string phonenumber)
        {
            List<RecordViewModel> records = new List<RecordViewModel>();
            records = _unitOfWork.record.GetSearchRecordData(reqstatus, patientname, requesttype, fromdateofservice, todateofservice, physicianname, email, phonenumber);
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Sheet1");
            IRow headerRow = sheet1.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Patient Name");
            headerRow.CreateCell(1).SetCellValue("Requestor");
            headerRow.CreateCell(2).SetCellValue("Date Of Service");
            headerRow.CreateCell(3).SetCellValue("Close Case Date");
            headerRow.CreateCell(4).SetCellValue("Email");
            headerRow.CreateCell(5).SetCellValue("Phone Number");
            headerRow.CreateCell(6).SetCellValue("Address");
            headerRow.CreateCell(7).SetCellValue("Zip");
            headerRow.CreateCell(8).SetCellValue("Request Status");
            headerRow.CreateCell(9).SetCellValue("Physician");
            headerRow.CreateCell(10).SetCellValue("Physician Note");
            headerRow.CreateCell(11).SetCellValue("Cancelled By Provider Note");
            headerRow.CreateCell(12).SetCellValue("Admin Note");
            headerRow.CreateCell(13).SetCellValue("Patient Note");

            for (int i = 0; i < records.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(records[i].PatientName);
                row.CreateCell(1).SetCellValue(records[i].Requestor);
                row.CreateCell(2).SetCellValue(records[i].DateOfService);
                row.CreateCell(3).SetCellValue(records[i].CloseCaseDate);
                row.CreateCell(4).SetCellValue(records[i].Email);
                row.CreateCell(5).SetCellValue(records[i].PhoneNumber);
                row.CreateCell(6).SetCellValue(records[i].Address);
                row.CreateCell(7).SetCellValue(records[i].Zip);
                row.CreateCell(8).SetCellValue(records[i].RequestStatus);
                row.CreateCell(9).SetCellValue(records[i].Physician);
                row.CreateCell(10).SetCellValue(records[i].PhysicianNote);
                row.CreateCell(11).SetCellValue(records[i].CancelProviderNote);
                row.CreateCell(12).SetCellValue(records[i].AdminNote);
                row.CreateCell(13).SetCellValue(records[i].PatientNote);
            }

            var date = DateTime.Now.ToString("hh : mm") + "Search Records";
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                var content = stream.ToArray();
                return Convert.ToBase64String(content);
            }
        }

        //delete records from search records
        public IActionResult DeleteRecords(int requestid)
        {
            _unitOfWork.record.DeleteSearchRecord(requestid);
            List<RecordViewModel> records = new List<RecordViewModel>();
            records = _unitOfWork.record.GetSearchRecordData(0, null, 0, null, null, null, null, null); ;
            return PartialView("_SearchRecordsTable", records);
        }

        //block History

        public IActionResult BlockHistory()
        {
            return View();
        }

        //block history search filter
        public IActionResult BlockHistoryFilter(string name, string date, string email, string phonenumber)
        {
            List<RecordViewModel> blockhistorydata = new List<RecordViewModel>();
            blockhistorydata = _unitOfWork.record.GetBlockHistoryFilterData(name, date, email, phonenumber);
            return PartialView("_BlockHistoryTable", blockhistorydata);
        }

        //unblock btn block history
        public IActionResult UnblockbtnBlockHistory(int requestid)
        {
            int adminid = (int)HttpContext.Session.GetInt32("AdminId");
            int yesorno = _unitOfWork.record.UnblockBlockHistory(requestid, adminid);
            if (yesorno != 1)
            {
                TempData["success"] = "Some Error Occured";
            }
            List<RecordViewModel> blockhistorydata = new List<RecordViewModel>();
            blockhistorydata = _unitOfWork.record.GetBlockHistoryFilterData(null, null, null, null);
            return PartialView("_BlockHistoryTable", blockhistorydata);
        }
    }
}
