using HalloDoc.DataContext;
using Services.Contracts;

namespace Services.Implementation
{
    public class UpdateData : IUpdateData
    {
        private readonly ApplicationDbContext _context;
        public UpdateData(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DeclineRequestTable(int requestid, int physicianid)
        {
            var request = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
            if (request != null)
            {
                request.PhysicianId = null;
                request.DeclinedBy = physicianid.ToString();
                _context.Requests.Update(request);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int UpdateRequestTable(int requestid, short status)
        {
            var request = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
            if (request != null)
            {

                request.Status = status;
                _context.Requests.Update(request);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }


    }
}
