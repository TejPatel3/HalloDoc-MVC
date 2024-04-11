using HalloDoc.DataModels;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IRecordRepository
    {
        public List<User> GetUserList(string firstname, string lastname, string email, int phone);
        public List<PatientRecordViewModel> GetPatientRecordData(int userid);
        public List<EmailLogViewModel> GetEmailLogs();
        public EmailLogViewModel GetAspNetRoleList();


    }
}
