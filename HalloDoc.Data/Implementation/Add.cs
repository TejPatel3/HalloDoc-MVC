using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class Add : Repository<Admin>, IAdd
    {
        private readonly ApplicationDbContext _context;
        public Add(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        //Save chages in database
        public void SaveChangesDB()
        {
            _context.SaveChanges();
        }
        //Direct add data in one table in database
        public int AddRole(Role role)
        {
            if (role != null)
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public int AddRoleMenu(RoleMenu roleMenu)
        {
            if (roleMenu != null)
            {
                _context.RoleMenus.Add(roleMenu);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public void AddPhysician(Physician physician)
        {
            _context.Add(physician);
            _context.SaveChanges();
        }
        public int AddPhysicianRegion(PhysicianRegion physicianRegion)
        {
            if (physicianRegion != null)
            {
                _context.PhysicianRegions.Add(physicianRegion);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public void AddPhysicianNotification(PhysicianNotification physicianNotification)
        {
            _context.Add(physicianNotification);
            _context.SaveChanges();
        }
        public void AddAspNetUser(AspNetUser user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }
        public void AddAspNetUserRole(AspNetUserRole userRole)
        {
            _context.Add(userRole);
            _context.SaveChanges();
        }
        public void AddRequestStatusLog(RequestStatusLog log)
        {
            _context.Add(log);
            _context.SaveChanges();
        }
        public void AddEncounter(Encounter encounter)
        {
            _context.Add(encounter);
            _context.SaveChanges();
        }
        public void AddRequest(Request request)
        {
            _context.Add(request); _context.SaveChanges();
        }
        public void AddRequestClient(RequestClient requestClient)
        {
            _context.Add(requestClient); _context.SaveChanges();
        }
        public void AddRequestWiseFile(RequestWiseFile wiseFile)
        {
            _context.Add(wiseFile); _context.SaveChanges();
        }
        public void AddOrderDetails(OrderDetail orderDetail)
        {
            _context.Add(orderDetail); _context.SaveChanges();
        }
        public void AddAdminRegion(AdminRegion adminRegion)
        {
            _context.Add(adminRegion); _context.SaveChanges();
        }
        public void AddShift(Shift shift)
        {
            _context.Add(shift); _context.SaveChanges();
        }
        public void AddShiftDetail(ShiftDetail shiftDetail)
        {
            _context.Add(shiftDetail); _context.SaveChanges();
        }
        //Add Data in multiple table 

        public void AddAdmin(UserAllDataViewModel obj, int adminid)
        {
            if (obj != null)
            {
                Guid aspnetid = Guid.NewGuid();
                AspNetUser aspnetuser = new AspNetUser
                {
                    Id = aspnetid.ToString(),
                    UserName = obj.UserName,
                    Email = obj.email,
                    PasswordHash = obj.password,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = obj.phonenumber,
                };
                _context.AspNetUsers.Add(aspnetuser);
                _context.SaveChanges();
                var createdbyadmin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);
                var createdbyaspnet = _context.AspNetUsers.FirstOrDefault(m => m.Id == createdbyadmin.AspNetUserId);
                var regionidbyname = _context.Regions.FirstOrDefault(m => m.Name == obj.selectedstate.ToString());
                Admin admin = new Admin
                {
                    AspNetUserId = aspnetid.ToString(),
                    FirstName = obj.firstname,
                    LastName = obj.lastname,
                    Email = obj.email,
                    Mobile = obj.phonenumber,
                    Address1 = obj.address1,
                    Address2 = obj.address2,
                    City = obj.city,
                    Zip = obj.zip,
                    RegionId = regionidbyname.RegionId,
                    CreatedBy = createdbyaspnet.Id,
                    CreatedDate = DateTime.Now,
                };
                _context.Add(admin);
                _context.SaveChanges();
                foreach (var item in obj.selectedregion)
                {
                    AdminRegion adminregion = new AdminRegion();
                    adminregion.AdminId = admin.AdminId;
                    adminregion.RegionId = item;
                    _context.Add(adminregion);
                    _context.SaveChanges();
                }

                var roleidbyname = _context.Roles.FirstOrDefault(m => m.Name == obj.selectedrole);
                AspNetUserRole aspnetuserrole = new AspNetUserRole
                {
                    UserId = aspnetid.ToString(),
                    RoleId = 1.ToString(),
                };
                _context.Add(aspnetuserrole);
                _context.SaveChanges();
            }
        }
    }
}