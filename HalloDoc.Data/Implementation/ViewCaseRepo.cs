using HalloDoc.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class ViewCaseRepo : Repository<RequestClient>, IViewCaseRepo
    {
        private readonly ApplicationDbContext _context;

        public ViewCaseRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public RequestClient GetViewCaseData(int reqid)
        {
            var model = _context.RequestClients.FirstOrDefault(m => m.RequestId == reqid);
            return model;
        }

    }
}
