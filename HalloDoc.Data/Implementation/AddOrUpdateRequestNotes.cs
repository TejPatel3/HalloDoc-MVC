using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class AddOrUpdateRequestNotes : Repository<RequestNote>, IAddOrUpdateRequestNotes
    {
        private readonly ApplicationDbContext _context;
        public AddOrUpdateRequestNotes(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void IAddOrUpdateRequestNotes.AddOrUpdateRequestNotes(AdminRequestViewModel obj)
        {
            var requestnote = new RequestNote();
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == obj.requestid);
            var existrequestnote = _context.RequestNotes.FirstOrDefault(m => m.RequestId == obj.requestid);
            if (existrequestnote != null)
            {
                var requestnoteupdate = _context.RequestNotes.FirstOrDefault(m => m.RequestId == obj.requestid);
                requestnoteupdate.ModifiedDate = DateTime.Now;
                requestnoteupdate.AdminNotes = obj.BlockNotes;
                _context.RequestNotes.Update(requestnoteupdate);
                _context.SaveChanges();
            }
            else
            {
                requestnote.RequestId = obj.requestid;
                requestnote.CreatedDate = DateTime.Now;
                requestnote.CreatedBy = "1";
                requestnote.AdminNotes = obj.BlockNotes;
                _context.RequestNotes.Add(requestnote);
                _context.SaveChanges();
            }
        }

        void IAddOrUpdateRequestNotes.PhysicianRequestNotes(AdminRequestViewModel obj)
        {
            var requestnote = new RequestNote();
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == obj.requestid);
            var existrequestnote = _context.RequestNotes.FirstOrDefault(m => m.RequestId == obj.requestid);
            if (existrequestnote != null)
            {
                var requestnoteupdate = _context.RequestNotes.FirstOrDefault(m => m.RequestId == obj.requestid);
                requestnoteupdate.ModifiedDate = DateTime.Now;
                requestnoteupdate.PhysicianNotes = obj.BlockNotes;
                _context.RequestNotes.Update(requestnoteupdate);
                _context.SaveChanges();
            }
            else
            {
                requestnote.RequestId = obj.requestid;
                requestnote.CreatedDate = DateTime.Now;
                requestnote.CreatedBy = "1";
                requestnote.PhysicianNotes = obj.BlockNotes;
                _context.RequestNotes.Add(requestnote);
                _context.SaveChanges();
            }
        }
    }
}
