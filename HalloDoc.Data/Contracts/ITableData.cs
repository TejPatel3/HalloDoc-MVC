using DataModels.DataModels;

namespace Services.Contracts
{
    public interface ITableData
    {
        // Get List Data without filter
        public List<Region> GetRegionList();
        public List<Physician> GetPhysicianList();
        public List<PhysicianNotification> GetPhysicianNotificationList();
        public List<PhysicianLocation> GetPhysicianLocationList();
        public List<AspNetRole> GetAspNetRoleList();
        public List<CaseTag> GetCaseTagList();
        public List<Role> GetRoleList();
        public List<Menu> GetMenuList();
        public List<AspNetUser> GetAspNetUserList();
        public List<Request> GetRequestList();
        public List<AspNetUser> GetAspNetUserListAdminPhysician();
        public List<RequestWiseFile> GetRequestWiseFileList();
        public List<HealthProfessionalType> GetHealthProfessionalTypeList();
        public List<HealthProfessional> GetHealthProfessionalList();
        public List<ShiftDetail> GetShiftDetailList();
        public List<Shift> GetShiftList();



        //Get List Data With filter
        public List<PhysicianRegion> GetPhysicianRegionListByPhysicianId(int physicianId);
        public List<PhysicianRegion> GetPhysicianRegionListByRegionId(int regionId);

        public List<Menu> GetMenuListByAccountType(int accountType);
        public List<RoleMenu> GetRoleMenuListByRoleId(int roleId);
        public List<PhysicianRegion> GetPhysicianRegionListByPhysiianId(int physiianId);
        public List<AdminRegion> GetAdminRegionListByAdminId(int? adminId);
        public List<RequestStatusLog> GetRequestStatusLogListByRequestId(int requestId);


        //Get Data By filter
        public Role GetRoleById(int id);
        public Request GetRequestFirstOrDefault(int id);
        public Physician GetPhysicianFirstOrDefault(int? id);
        public Physician GetPhysicianByAspNetUserId(string aspnetuserid);
        public Physician GetPhysicianByEmail(string email);
        public PhysicianRegion GetPhysicianRegionByPhysicianId(int physicianId);
        public PhysicianNotification GetPhysicianNotificationByPhysicianId(int physicianId);
        public Admin GetAdminByAspNetUserId(string aspnetuserid);
        public Admin GetAdminByAdminId(int? adminId);
        public Admin GetAdminByEmail(string email);
        public AspNetUser GetAspNetUserByAspNetUserId(string aspNetUserId);
        public AspNetUser GetAspNetUserByEmail(string email);
        public AspNetUserRole GetAspNetUserRoleByUserId(string userId);
        public AspNetRole GetAspNetRoleById(string userId);
        public Request GetRequestByConfirmationNumber(string confirmationNumber);
        public RequestClient GetRequestClientByRequestId(int requestId);
        public RequestNote GetRequestNoteByRequestId(int requestId);
        public Encounter GetEncounterByRequestId(int requestId);
        public User GetUserByEmail(string email);
        public Region GetRegionByRegionId(int? regionId);
        public RequestStatusLog GetRequestStatusLogByRequestIdStatus(int requestId, int status);

        public HealthProfessional GetHealthProfessionalByVendorId(int vendorId);
        public RequestWiseFile GetRequestWiseFileById(int id);
        public CaseTag GetCaseTagByName(string name);

        public ShiftDetail GetShiftDetailByShiftDetailId(int shiftDetailId);

    }
}
