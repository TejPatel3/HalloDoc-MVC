using DataModels.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IAdd
    {
        //save changes in database
        public void SaveChangesDB();

        //Direct add data in one table in database
        public int AddRole(Role role);
        public int AddRoleMenu(RoleMenu roleMenu);
        public int AddPhysicianRegion(PhysicianRegion physicianRegion);
        public void AddPhysicianNotification(PhysicianNotification physicianNotification);
        public void AddPhysician(Physician physician);
        public void AddAspNetUser(AspNetUser user);
        public void AddAspNetUserRole(AspNetUserRole userRole);
        public void AddRequestStatusLog(RequestStatusLog log);
        public void AddEncounter(Encounter encounter);
        public void AddRequest(Request request);
        public void AddRequestClient(RequestClient requestClient);

        public void AddRequestWiseFile(RequestWiseFile wiseFile);
        public void AddOrderDetails(OrderDetail orderDetail);
        public void AddAdminRegion(AdminRegion adminRegion);
        public void AddShift(Shift shift);
        public void AddShiftDetail(ShiftDetail shiftDetail);

        //Add Data in multiple table 
        public void AddAdmin(UserAllDataViewModel obj, int adminid);
    }
}
