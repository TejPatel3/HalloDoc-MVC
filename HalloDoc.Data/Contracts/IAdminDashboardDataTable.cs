using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdminDashboardDataTable
    {
        List<AdminDashboardTableDataViewModel> getallAdminDashboard(string status, int currentpage, int requesttype, string searchkey, int regionid);
        List<AdminDashboardTableDataViewModel> GetDataForExportAll();
    }
}
