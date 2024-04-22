using HalloDoc.DataModels;

namespace Services.Contracts
{
    public interface IUpdateData
    {
        //update data in single table
        public int UpdateRole(Role role);
        public int UpdateAspNetUser(AspNetUser aspNetUser);
        public int UpdatePhysician(Physician physician);
        public int UpdateRequest(Request request);
        public int UpdateEncounter(Encounter encounter);
        public int UpdateRequestWiseFile(RequestWiseFile wiseFile);
        public int UpdateRequestClient(RequestClient requestClient);
        public int UpdateAdmin(Admin admin);
        public int UpdateShiftDetail(ShiftDetail shiftDetail);

        public int UpdateRequestTable(int requestid, short status);
        public int DeclineRequestTable(int requestid, int physicianid);


        //update data in multiple table


    }
}
