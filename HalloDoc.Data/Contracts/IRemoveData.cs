using HalloDoc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
