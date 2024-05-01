using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class RemoveData : IRemoveData
    {
        private readonly ApplicationDbContext _context;
        public RemoveData()
        {
            _context = new ApplicationDbContext();
        }
        public void RemoveRoleMenu(RoleMenu roleMenu)
        {
            _context.Remove(roleMenu);
            _context.SaveChanges();
        }
        public void RemoveRole(Role role)
        {
            _context.Remove(role);
            _context.SaveChanges();
        }
        public void RemovePhysicianRegion(PhysicianRegion physicianRegion)
        {
            _context.Remove(physicianRegion);
            _context.SaveChanges();
        }
        public void RemovePhysicianNotification(PhysicianNotification physicianNotification)
        {
            _context.Remove(physicianNotification);
            _context.SaveChanges();
        }
        public void RemoveAdminRegion(AdminRegion adminRegion)
        {
            _context.Remove(adminRegion); _context.SaveChanges();
        }
    }
}
