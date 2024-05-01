using DataModels.DataContext;
using DataModels.DataModels;
using Microsoft.EntityFrameworkCore;
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

        //Get list Data without filter
        public List<Region> GetRegionList()
        {
            List<Region> regions = _context.Regions.ToList();
            return regions;
        }
        public List<Physician> GetPhysicianList()
        {
            List<Physician> physicians = _context.Physicians.Where(m => m.IsDeleted == new BitArray(new[] { false })).Include(m => m.PhysicianRegions).ToList();
            return physicians;
        }
        public List<AspNetUser> GetAspNetUserListAdminPhysician()
        {
            return _context.AspNetUsers.Include(m => m.AspNetUserRoles).Where(m => m.AspNetUserRoles.FirstOrDefault().RoleId == "1" || m.AspNetUserRoles.FirstOrDefault().RoleId == "2").ToList();
        }
        public List<AspNetRole> GetAspNetRoleList()
        {
            return _context.AspNetRoles.ToList();
        }
        public List<AspNetUser> GetAspNetUserList()
        {
            return _context.AspNetUsers.Include(m => m.AspNetUserRoles).ToList();
        }
        public List<CaseTag> GetCaseTagList()
        {
            return _context.CaseTags.ToList();
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
        public List<Role> GetRoleList()
        {
            return _context.Roles.ToList();
        }
        public List<Menu> GetMenuList()
        {
            return _context.Menus.ToList();
        }
        public List<Request> GetRequestList()
        {
            return _context.Requests.ToList();
        }
        public List<RequestWiseFile> GetRequestWiseFileList()
        {
            return _context.RequestWiseFiles.ToList();
        }
        public List<HealthProfessional> GetHealthProfessionalList()
        {
            return _context.HealthProfessionals.ToList();
        }
        public List<HealthProfessionalType> GetHealthProfessionalTypeList()
        {
            return _context.HealthProfessionalTypes.ToList();
        }

        public List<ShiftDetail> GetShiftDetailList()
        {
            return _context.ShiftDetails.Include(m => m.Shift).Where(m => m.IsDeleted != new BitArray(new[] { true })).ToList();
        }
        public List<Shift> GetShiftList()
        {
            return _context.Shifts.ToList();
        }


        //Get List Data with filter
        public List<PhysicianRegion> GetPhysicianRegionListByPhysicianId(int physicianId)
        {
            return _context.PhysicianRegions.Include(m => m.Physician).Where(m => m.PhysicianId == physicianId).ToList();
        }
        public List<PhysicianRegion> GetPhysicianRegionListByRegionId(int regionId)
        {
            return _context.PhysicianRegions.Include(m => m.Physician).Where(m => m.RegionId == regionId).ToList();
        }
        public List<Menu> GetMenuListByAccountType(int accountType)
        {
            var accounttypemenulist = _context.Menus.Where(m => m.AccountType == accountType).ToList();
            return accounttypemenulist;
        }
        public List<RoleMenu> GetRoleMenuListByRoleId(int roleId)
        {
            return _context.RoleMenus.Where(m => m.RoleId == roleId).ToList();
        }
        public List<PhysicianRegion> GetPhysicianRegionListByPhysiianId(int physiianId)
        {
            return _context.PhysicianRegions.Where(m => m.PhysicianId == physiianId).ToList();
        }
        public List<AdminRegion> GetAdminRegionListByAdminId(int? adminId)
        {
            return _context.AdminRegions.Where(m => m.AdminId == adminId).ToList();
        }
        public List<RequestStatusLog> GetRequestStatusLogListByRequestId(int requestId)
        {
            return _context.RequestStatusLogs.Where(m => m.RequestId == requestId).Include(m => m.Physician).Include(m => m.Physician).ToList();
        }
        //Get Data with filter
        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(m => m.RoleId == id);
        }
        public Request GetRequestFirstOrDefault(int id)
        {
            return _context.Requests.Include(m => m.RequestClients).Include(m => m.User).FirstOrDefault(m => m.RequestId == id);
        }
        public Physician GetPhysicianFirstOrDefault(int? id)
        {
            return _context.Physicians.Include(m => m.PhysicianRegions).FirstOrDefault(m => m.PhysicianId == id);
        }
        public PhysicianRegion GetPhysicianRegionByPhysicianId(int physicianId)
        {
            return _context.PhysicianRegions.FirstOrDefault(m => m.PhysicianId == physicianId);
        }
        public Physician GetPhysicianByAspNetUserId(string aspnetuserid)
        {
            return _context.Physicians.FirstOrDefault(m => m.Id == aspnetuserid);
        }
        public Physician GetPhysicianByEmail(string email)
        {
            return _context.Physicians.FirstOrDefault(m => m.Email == email);
        }
        public PhysicianNotification GetPhysicianNotificationByPhysicianId(int physicianId)
        {
            return _context.PhysicianNotifications.FirstOrDefault(m => m.PhysicianId == physicianId);
        }
        public Admin GetAdminByAspNetUserId(string aspnetuserid)
        {
            return _context.Admins.FirstOrDefault(m => m.AspNetUserId == aspnetuserid);
        }
        public Admin GetAdminByAdminId(int? adminId)
        {
            return _context.Admins.Include(m => m.AdminRegions).FirstOrDefault(m => m.AdminId == adminId);
        }
        public Admin GetAdminByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(m => m.Email == email);
        }
        public AspNetUser GetAspNetUserByAspNetUserId(string aspNetUserId)
        {
            return _context.AspNetUsers.FirstOrDefault(m => m.Id == aspNetUserId);
        }
        public AspNetUser GetAspNetUserByEmail(string email)
        {
            return _context.AspNetUsers.FirstOrDefault(m => m.Email == email);
        }
        public AspNetUserRole GetAspNetUserRoleByUserId(string userId)
        {
            return _context.AspNetUserRoles.FirstOrDefault(m => m.UserId == userId);
        }
        public AspNetRole GetAspNetRoleById(string userId)
        {
            return _context.AspNetRoles.FirstOrDefault(m => m.Id == userId);
        }
        public RequestClient GetRequestClientByRequestId(int requestId)
        {
            return _context.RequestClients.FirstOrDefault(m => m.RequestId == requestId);
        }
        public RequestNote GetRequestNoteByRequestId(int requestId)
        {
            return _context.RequestNotes.FirstOrDefault(m => m.RequestId == requestId);
        }
        public RequestWiseFile GetRequestWiseFileById(int id)
        {
            return _context.RequestWiseFiles.FirstOrDefault(m => m.RequestWiseFileId == id);
        }
        public Request GetRequestByConfirmationNumber(string confirmationNumber)
        {
            return _context.Requests.FirstOrDefault(m => m.ConfirmationNumber == confirmationNumber);
        }
        public Encounter GetEncounterByRequestId(int requestId)
        {
            return _context.Encounters.FirstOrDefault(m => m.RequestId == requestId);
        }
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(m => m.Email == email);
        }
        public Region GetRegionByRegionId(int? regionId)
        {
            return _context.Regions.FirstOrDefault(m => m.RegionId == regionId);
        }
        public RequestStatusLog GetRequestStatusLogByRequestIdStatus(int requestId, int status)
        {
            return _context.RequestStatusLogs.FirstOrDefault(m => m.RequestId == requestId && m.Status == status);
        }
        public HealthProfessional GetHealthProfessionalByVendorId(int vendorId)
        {
            return _context.HealthProfessionals.FirstOrDefault(m => m.VendorId == vendorId);
        }
        public CaseTag GetCaseTagByName(string name)
        {
            return _context.CaseTags.FirstOrDefault(m => m.Name == name);
        }
        public ShiftDetail GetShiftDetailByShiftDetailId(int shiftDetailId)
        {
            return _context.ShiftDetails.Include(m => m.Shift).FirstOrDefault(m => m.ShiftDetailId == shiftDetailId);
        }
    }
}
