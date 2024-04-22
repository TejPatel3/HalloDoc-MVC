namespace Services.Contracts
{
    public interface IunitOfWork
    {
        public IVendorRepository vendor { get; }
        public ISchedulingRepository scheduling { get; }
        public IRecordRepository record { get; }
        public ITableData tableData { get; }
        public IAdminDashboard AdminDashboard { get; }
        public IAdminDashboardDataTable AdminDashboarDataTable { get; }
        public IUpdateData UpdateData { get; }
        public IAddOrUpdateRequestStatusLog AddOrUpdateRequestStatusLog { get; }
        public IAccessRepository Access { get; }
        public IAdd Add { get; }
        public IRemoveData RemoveData { get; }
    }
}
