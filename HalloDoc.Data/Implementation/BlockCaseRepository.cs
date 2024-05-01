using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class BlockCaseRepository : Repository<BlockRequest>, IBlockCaseRepository
    {
        private readonly ApplicationDbContext _context;
        public BlockCaseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void BlockPatient(int requestid, String blocknote)
        {
            var requestdata = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);

            if (requestdata != null)
            {
                var blockdata = new BlockRequest();
                blockdata.RequestId = requestid.ToString();
                blockdata.PhoneNumber = requestdata.PhoneNumber;
                blockdata.Email = requestdata.Email;
                blockdata.Reason = blocknote;
                blockdata.CreatedDate = DateTime.Now;
                requestdata.Status = 11;

                if (blockdata != null)
                {
                    _context.BlockRequests.Add(blockdata);
                    _context.SaveChanges();
                    _context.Requests.Update(requestdata);
                    _context.SaveChanges();
                }
            }
        }
    }
}
