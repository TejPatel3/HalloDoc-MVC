using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface ITableData
    {
        public List<Region> GetRegionList();
        public List<Physician> GetPhysicianList();
        public List<PhysicianNotification> GetPhysicianNotificationList();
        public List<PhysicianLocation> GetPhysicianLocationList();

    }
}
