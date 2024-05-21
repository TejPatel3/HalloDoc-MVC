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

        public int? PayTotalHours { get; set; }

        public int? PayWeekend { get; set; }

        public int? PayHouseCalls { get; set; }

        public int? PayPhoneConcults { get; set; }

        public int? SumTotalHours { get; set; }

        public int? SumWeekend { get; set; }

        public int? SumHouseCalls { get; set; }

        public int? SumPhoneConcults { get; set; }

        public int? InvoiceTotal { get; set; }

        public int? InvoiceTotalSubmit { get; set; }

        [Range(minimum: 0, maximum: 100000, ErrorMessage = "Enter bomus between 0 to 100k")]
        public int? Bonus { get; set; }

        //public int? BonusSubmit { get; set; }
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Admin Description")]
        public string? AdminDescription { get; set; }

        //public string? AdminDescriptionSubmit { get; set; }

        public string? ApprovedBy { get; set; }
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

    public class PayrateView
    {
        public int PayrateId { get; set; }

        public int? PhysicianId { get; set; }

        public string? Category { get; set; }

        public int? NightShiftWeekend { get; set; }

        public int? Shift { get; set; }

        public int? HouseCallNightWeekend { get; set; }

        public int? PhoneConsult { get; set; }

        public int? PhoneConsultNightWeekend { get; set; }

        public int? BatchTesting { get; set; }

        public int? HouseCalls { get; set; }

        public bool isExists { get; set; }
    }


}
