using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class AddOrUpdateRequestStatusLog : Repository<RequestStatusLog>, IAddOrUpdateRequestStatusLog
    {
        private readonly ApplicationDbContext _context;
        public AddOrUpdateRequestStatusLog(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        void IAddOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(int requestid, string? cancelnote, int? AdminId = null, int? trnastophyid = null, int? PhysicianId = null)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            if (request != null)
            {
                var data = new RequestStatusLog();
                if (AdminId != null)
                {
                    data.RequestId = requestid;
                    data.Status = request.Status;
                    data.AdminId = AdminId;
                    data.Notes = cancelnote;
                    data.CreatedDate = DateTime.Now;
                }
                else if (PhysicianId != null)
                {
                    data.RequestId = requestid;
                    data.Status = request.Status;
                    data.AdminId = AdminId;
                    data.Notes = cancelnote;
                    data.CreatedDate = DateTime.Now;
                }
                if (trnastophyid != null)
                {
                    data.TransToPhysicianId = trnastophyid;
                };
                _context.RequestStatusLogs.Add(data);
                _context.SaveChanges();
            }
        }
    }
}
