using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminDashboardDataTable
    {
        List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status);
        List<AdminDashboardTableDataViewModel> GetDataForExportAll();
    }
}
