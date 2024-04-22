using Services.Contracts;

namespace Services.Implementation
{
    public class unitOfWork : IunitOfWork
    {
        public IVendorRepository vendor { get; private set; }
        public ISchedulingRepository scheduling { get; private set; }
        public IRecordRepository record { get; private set; }
        public ITableData tableData { get; private set; }
        public IAdminDashboard AdminDashboard { get; private set; }
        public IAdminDashboardDataTable AdminDashboarDataTable { get; private set; }
        public IUpdateData UpdateData { get; private set; }
        public IAddOrUpdateRequestStatusLog AddOrUpdateRequestStatusLog { get; private set; }
        public IAccessRepository Access { get; private set; }
        public IAdd Add { get; }
        public IRemoveData RemoveData { get; }


        public unitOfWork(IVendorRepository Vendor, ISchedulingRepository scheduling, IRecordRepository record, ITableData tableData, IAdminDashboard adminDashboard, IAdminDashboardDataTable adminDashboarDataTable, IUpdateData updateData, IAddOrUpdateRequestStatusLog addOrUpdateRequestStatusLog, IAccessRepository access, IAdd add, IRemoveData removeData)
        {
            vendor = Vendor;
            this.scheduling = scheduling;
            this.record = record;
            this.tableData = tableData;
            AdminDashboard = adminDashboard;
            AdminDashboarDataTable = adminDashboarDataTable;
            UpdateData = updateData;
            AddOrUpdateRequestStatusLog = addOrUpdateRequestStatusLog;
            Access = access;
            Add = add;
            RemoveData = removeData;
        }
    }
}
