using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface ITableData
    {
        public List<Region> GetRegionList();
        public List<Physician> GetPhysicianList();
        public List<PhysicianNotification> GetPhysicianNotificationList();
        public List<PhysicianLocation> GetPhysicianLocationList();
        public List<AspNetRole> GetAspNetRoleList();
        public List<CaseTag> GetCaseTagList();
        public List<Role> GetRoleList();


        public Role GetRoleById(int id);
        public Request GetRequestFirstOrDefault(int id);
        public Physician GetPhysicianFirstOrDefault(int id);



    }
}
