using HalloDoc.DataContext;
using HalloDoc.DataModels;
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
        // direct update in one table
        public int UpdateRole(Role role)
        {
            if (role != null)
            {
                _context.Update(role);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateAspNetUser(AspNetUser aspNetUser)
        {
            if (aspNetUser != null)
            {
                _context.Update(aspNetUser);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdatePhysician(Physician physician)
        {
            if (physician != null)
            {
                _context.Update(physician);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateRequest(Request request)
        {
            if (request != null)
            {
                _context.Update(request);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateEncounter(Encounter encounter)
        {
            if (encounter != null)
            {
                _context.Update(encounter);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateRequestWiseFile(RequestWiseFile wiseFile)
        {
            if (wiseFile != null)
            {
                _context.Update(wiseFile);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateRequestClient(RequestClient requestClient)
        {
            if (requestClient != null)
            {
                _context.Update(requestClient);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateAdmin(Admin admin)
        {
            if (admin != null)
            {
                _context.Update(admin);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
        }
        public int UpdateShiftDetail(ShiftDetail shiftDetail)
        {
            if (shiftDetail != null)
            {
                _context.Update(shiftDetail);
                _context.SaveChanges();
                return 1;
            }
            else { return 0; }
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

        //update data in multiple table

    }
}
