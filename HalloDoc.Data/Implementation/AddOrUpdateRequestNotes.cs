using HalloDoc.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class AddOrUpdateRequestNotes : Repository<RequestNote>, IAddOrUpdateRequestNotes
    {
        private readonly ApplicationDbContext _context;
        public AddOrUpdateRequestNotes(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        void IAddOrUpdateRequestNotes.AddOrUpdateRequestNotes(int requestid)
        {
            var requestnote = new RequestNote();
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
            var existrequestnote = _context.RequestNotes.FirstOrDefault(m => m.RequestId == requestid);
            if (existrequestnote != null)
            {
            }
        }
    }
}
