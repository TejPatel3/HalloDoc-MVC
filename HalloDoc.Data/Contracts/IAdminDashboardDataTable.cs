using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminDashboardDataTable
    {
        List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status);
        List<AdminDashboardTableDataViewModel> GetDataForExportAll();
        public List<AdminDashboardTableDataViewModel> getallProviderDashboard(int status, int physicianid);
        public void TransferCase(int requestid, AdminRequestViewModel note, int physicianid);

    }
}
