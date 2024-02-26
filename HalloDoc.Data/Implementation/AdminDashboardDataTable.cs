using HalloDoc.DataModels;
using Services.Contracts;
using Services.ViewModels;
using System.Globalization;

namespace Services.Implementation
{
    public class AdminDashboardDataTable : Repository<Request>, IAdminDashboardDataTable
    {
        private readonly ApplicationDbContext _context;
        public AdminDashboardDataTable(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // AdminDashboardTableDataViewModel viewCase(int reqid)
        //{

        //    var request = _context.RequestClients.FirstOrDefault(m => m.RequestClientId == reqid);
        //    return request;


        //}
        public List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status)
        {
            var AdminDashboardDataTableViewModels = from user in _context.Users
                                                    join req in _context.Requests on user.UserId equals req.UserId
                                                    where req.Status == status
                                                    orderby req.CreatedDate descending
                                                    select new AdminDashboardTableDataViewModel
                                                    {
                                                        PatientName = user.FirstName + " " + user.LastName,
                                                        PatientDOB = new DateOnly(Convert.ToInt32(user.IntYear), DateTime.ParseExact(user.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.IntDate)),
                                                        RequestorName = req.FirstName + " " + req.LastName,
                                                        RequestedDate = req.CreatedDate,
                                                        PatientPhone = user.Mobile,
                                                        RequestorPhone = req.PhoneNumber,
                                                        Address = req.RequestClients.FirstOrDefault(x => x.RequestId == req.RequestId).Address,
                                                        Notes = req.RequestClients.FirstOrDefault(x => x.RequestId == req.RequestId).Notes,
                                                        ProviderEmail = _context.Physicians.FirstOrDefault(x => x.PhysicianId == req.PhysicianId).Email,
                                                        PatientEmail = user.Email,
                                                        RequestorEmail = req.Email,
                                                        RequestType = req.RequestTypeId,
                                                        requestid = req.RequestId,
                                                    };
            return AdminDashboardDataTableViewModels.ToList();

        }

    }
}
