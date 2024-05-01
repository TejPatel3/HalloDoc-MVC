using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
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

        public List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status)
        {
            if (status == 1)
            {
                var AdminDashboardDataTableViewModels = from user in _context.Users
                                                        join req in _context.Requests on user.UserId equals req.UserId
                                                        join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                        where req.Status == status
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
                                                            status = req.Status,

                                                        };

                return AdminDashboardDataTableViewModels.ToList();
            }
            else
            {
                var AdminDashboardDataTableViewModels = from user in _context.Users
                                                        join req in _context.Requests on user.UserId equals req.UserId
                                                        join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                        join phy in _context.Physicians on req.PhysicianId equals phy.PhysicianId
                                                        where req.Status == status
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
                                                            status = req.Status,
                                                            PhysicianName = phy.FirstName + phy.LastName

                                                        };

                return AdminDashboardDataTableViewModels.ToList();
            }

        }

        public List<AdminDashboardTableDataViewModel> getallProviderDashboard(int status, int physicianid)
        {


            var AdminDashboardDataTableViewModels = from user in _context.Users
                                                    join req in _context.Requests on user.UserId equals req.UserId
                                                    join reqclient in _context.RequestClients on req.RequestId equals reqclient.RequestId
                                                    where req.Status == status && req.PhysicianId == physicianid
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
                                                        status = req.Status,

                                                    };


            return AdminDashboardDataTableViewModels.ToList();
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
                                                        status = req.Status,

                                                    };

            return AdminDashboardDataTableViewModels.ToList();
        }

        public void TransferCase(int requestid, AdminRequestViewModel note, int physicianid)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            request.Status = 1;
            request.PhysicianId = null;
            var statuslog = new RequestStatusLog();
            statuslog.Status = 1;
            statuslog.RequestId = requestid;
            statuslog.PhysicianId = physicianid;
            statuslog.Notes = note.BlockNotes;
            statuslog.CreatedDate = DateTime.Now;
            statuslog.TransToAdmin = new BitArray(new[] { true });
            _context.Requests.Update(request);
            _context.RequestStatusLogs.Add(statuslog);
            _context.SaveChanges();
        }
    }
}
