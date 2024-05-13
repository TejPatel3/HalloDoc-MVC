using DataModels.DataModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class InvoicingView
    {
        public int physicianId { get; set; }

        public int TimeSheetDetailId { get; set; }

        public int TimeSheetId { get; set; }

        public string? MonthHalf { get; set; }

        public DateTime Date { get; set; }

        public int? onCallHours { get; set; }

        [Range(0, 24, ErrorMessage = "Total hours can not be greater than 24")]
        public int? totalHours { get; set; }

        public bool? isWeekend { get; set; }

        public int? noOfHouseCalls { get; set; }

        public int? noOfPhoneConsults { get; set; }

        public string? ItemName { get; set; }

        public int? Amount { get; set; }

        public string? BillName { get; set; }

        public bool? holiday { get; set; }
    }

    public class InvoicingViewAll
    {
        public IEnumerable<InvoicingView> Invoicing { get; set; }

        public IEnumerable<ReceiptView> Receipt { get; set; }

        public List<Physician> Physicians { get; set; }

        public int? selectedPhyId { get; set; }

        public string? phyName { get; set; }

        public int TimeSheetId { get; set; }

        public string? MonthHalf { get; set; }

        public int Month { get; set; }

        public int? Half { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Status { get; set; }

        public bool IsFinalized { get; set; }

        public bool IsApproved { get; set; }
    }

    public class ReceiptView
    {
        public int physicianId { get; set; }

        public string? MonthHalf { get; set; }

        public DateTime Date { get; set; }

        public string? Item { get; set; }

        public int? Amount { get; set; }

        public string? FileName { get; set; }

        public IFormFile? File { get; set; }

        public int? TimeSheetBillId { get; set; }

        public int TimeSheetDetailId { get; set; }

        public bool? IsDeleted { get; set; }
    }

}
