using DataModels.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IProviderSiteRepository
    {


        public IEnumerable<InvoicingView> GetMonthHalfTimeSheetData(int year, int month, int half, int physicianId);

        public IEnumerable<ReceiptView> GetMonthHalfReceiptData(int year, int month, int half, int physicianId);

        public IEnumerable<ReceiptView> GetMonthHalfTimesheetBillData(int year, int month, int half, int physicianId);

        public void AddUpdateTimesheet(IEnumerable<InvoicingView> model);

        public void AddTimesheetBill(TimesheetBill model);

        public void DeleteTimesheetBill(int timesheetBillId);

        public bool IsTimeSheetFinalized(int year, int month, int half, int physicianId);

        public bool FinalizeTimeSheet(int timeSheetId);

        public bool IsExistsTimesheetBillByDetailId(int timeSheetDetailId);

        public void UpdateTimeSheetBill(TimesheetBill model);

        public InvoicingViewAll GetAdminTimesheetData(int year, int month, int half, int physicianId);

        public InvoicingViewAll GetAdminApproveSheetData(int year, int month, int half, int physicianId);

        public PayrateView GetPayrateData(int physicianId);

        public void UpdatePayrateData(int phyId, int payrateValue, string payrateType);

        public Timesheet GetTimesheet(int year, int month, int half, int physicianId);

        public bool ApproveTimeSheet(InvoicingViewAll model);

        public void SaveChanges();
        //public IEnumerable<InvoicingView> GetMonthHalfTimeSheetData(int year, int month, int half, int physicianId);

        //public IEnumerable<ReceiptView> GetMonthHalfReceiptData(int year, int month, int half, int physicianId);

        //public IEnumerable<ReceiptView> GetMonthHalfTimesheetBillData(int year, int month, int half, int physicianId);

        //public void AddUpdateTimesheet(IEnumerable<InvoicingView> model);

        //public void AddTimesheetBill(TimesheetBill model);

        //public void DeleteTimesheetBill(int timesheetBillId);

        //public bool IsTimeSheetFinalized(int year, int month, int half, int physicianId);

        //public bool FinalizeTimeSheet(int timeSheetId);

        //public bool IsExistsTimesheetBillByDetailId(int timeSheetDetailId);

        //public void UpdateTimeSheetBill(TimesheetBill model);

        //public InvoicingViewAll GetAdminTimesheetData(int year, int month, int half, int physicianId);
    }
}
