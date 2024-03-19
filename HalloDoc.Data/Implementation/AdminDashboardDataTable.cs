using HalloDoc.DataContext;
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

        public List<AdminDashboardTableDataViewModel> getallAdminDashboard(string status, int currentpage, int requesttype, string searchkey, int regionid)
        {
            if (requesttype != 0)
            {

                var AdminDashboardDataTableViewModels = from user in _context.Users
                                                        join req in _context.Requests on user.UserId equals req.UserId
                                                        join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                        where req.Status == int.Parse(status) && req.RequestTypeId == requesttype
                                                        orderby req.CreatedDate descending
                                                        select new AdminDashboardTableDataViewModel
                                                        {
                                                            PatientName = reqclient.FirstName + " " + reqclient.LastName,
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
                                                            regionid = reqclient.RegionId,
                                                            PhysicianId = req.PhysicianId,
                                                            regionidtoclose = reqclient.RegionId.ToString(),
                                                        };
                if (!string.IsNullOrWhiteSpace(searchkey))
                {
                    //AdminDashboardDataTableViewModels = AdminDashboardDataTableViewModels.Where(a => a.PatientName.ToLower().Contains(searchkey.ToLower())).ToList();
                    AdminDashboardDataTableViewModels = from check in AdminDashboardDataTableViewModels where check.PatientName.ToLower().Contains(searchkey.ToLower()) select check;
                }
                if (regionid != 0)
                {
                    AdminDashboardDataTableViewModels = from check in AdminDashboardDataTableViewModels where check.regionid == regionid select check;

                }

                return AdminDashboardDataTableViewModels.ToList();
            }
            else
            {
                var AdminDashboardDataTableViewModels = from user in _context.Users
                                                        join req in _context.Requests on user.UserId equals req.UserId
                                                        join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                        where req.Status == int.Parse(status)
                                                        orderby req.RequestTypeId ascending
                                                        select new AdminDashboardTableDataViewModel
                                                        {
                                                            PatientName = reqclient.FirstName + " " + reqclient.LastName,
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
                                                            regionid = reqclient.RegionId,
                                                            PhysicianId = req.PhysicianId,
                                                            regionidtoclose = reqclient.RegionId.ToString(),
                                                        };
                if (!string.IsNullOrWhiteSpace(searchkey))
                {
                    //AdminDashboardDataTableViewModels = AdminDashboardDataTableViewModels.Where(a => a.PatientName.ToLower().Contains(searchkey.ToLower())).ToList();
                    AdminDashboardDataTableViewModels = from check in AdminDashboardDataTableViewModels where check.PatientName.ToLower().Contains(searchkey.ToLower()) select check;
                }
                if (regionid != 0)
                {
                    AdminDashboardDataTableViewModels = from check in AdminDashboardDataTableViewModels where check.regionid == regionid select check;

                }
                return AdminDashboardDataTableViewModels.ToList();
            }
            //.Skip((currentpage - 1) * 5).Take(5).ToList();

        }

        public List<AdminDashboardTableDataViewModel> GetDataForExportAll()
        {
            var AdminDashboardDataTableViewModels = from user in _context.Users
                                                    join req in _context.Requests on user.UserId equals req.UserId
                                                    join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                    orderby req.CreatedDate descending
                                                    select new AdminDashboardTableDataViewModel
                                                    {
                                                        PatientName = reqclient.FirstName + " " + reqclient.LastName,
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
                                                        regionid = reqclient.RegionId,
                                                        PhysicianId = req.PhysicianId,
                                                        regionidtoclose = reqclient.RegionId.ToString(),
                                                    };

            return AdminDashboardDataTableViewModels.ToList();
        }


    }
}
