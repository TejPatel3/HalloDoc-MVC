using DataModels.DataModels;

namespace Services.Contracts
{
    public interface IRemoveData
    {
        public void RemoveRoleMenu(RoleMenu roleMenu);
        public void RemoveRole(Role role);
        public void RemovePhysicianRegion(PhysicianRegion physicianRegion);
        public void RemovePhysicianNotification(PhysicianNotification physicianNotification);
        public void RemoveAdminRegion(AdminRegion adminRegion);


    }
}
