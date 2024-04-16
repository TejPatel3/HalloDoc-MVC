using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;
using System.Collections;

namespace Services.Implementation
{
    public class TableData : ITableData
    {

        private readonly ApplicationDbContext _context;
        public TableData()
        {
            _context = new ApplicationDbContext();
        }

        //All List Method
        public List<Region> GetRegionList()
        {
            List<Region> regions = _context.Regions.ToList();
            return regions;
        }
        public List<Physician> GetPhysicianList()
        {
            List<Physician> physicians = _context.Physicians.Where(m => m.IsDeleted == new BitArray(new[] { false })).ToList();
            return physicians;
        }
        public List<PhysicianNotification> GetPhysicianNotificationList()
        {
            List<PhysicianNotification> physiciansnoti = _context.PhysicianNotifications.ToList();
            return physiciansnoti;
        }
        public List<PhysicianLocation> GetPhysicianLocationList()
        {
            List<PhysicianLocation> list = _context.PhysicianLocations.ToList();
            return list;
        }
        public List<AspNetRole> GetAspNetRoleList()
        {
            return _context.AspNetRoles.ToList();
        }
        public List<CaseTag> GetCaseTagList()
        {
            return _context.CaseTags.ToList();
        }
        public List<Role> GetRoleList()
        {
            return _context.Roles.ToList();
        }

        //Take Data according id 
        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(m => m.RoleId == id);
        }
        public Request GetRequestFirstOrDefault(int id)
        {
            return _context.Requests.FirstOrDefault(m => m.RequestId == id);
        }
        public Physician GetPhysicianFirstOrDefault(int id)
        {
            return _context.Physicians.FirstOrDefault(m => m.PhysicianId == id);
        }
    }
}
