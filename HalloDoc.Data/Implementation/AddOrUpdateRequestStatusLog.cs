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

        void IAddOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(int requestid, int? APId, string cancelnote, int? transtophyid = null)
        {
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            if (request != null)
            {
                var data = new RequestStatusLog
                {
                    RequestId = requestid,
                    Status = request.Status,
                    AdminId = APId,
                    Notes = cancelnote,
                    CreatedDate = DateTime.Now
                };
                if (transtophyid != null)
                {
                    data.TransToPhysicianId = transtophyid;
                };
                _context.RequestStatusLogs.Add(data);
                _context.SaveChanges();
            }

        }
    }
}
