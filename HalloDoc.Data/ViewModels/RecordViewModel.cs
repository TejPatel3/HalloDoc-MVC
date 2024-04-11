using HalloDoc.DataModels;

namespace Services.ViewModels
{
    public class RecordViewModel
    {

    }
    public class PatientHistoryViewModel
    {
        public List<User> UserList { get; set; }
    }
    public class PatientRecordViewModel
    {
        public string ClientName { get; set; }
        public string CreatedDate { get; set; }
        public string Confirmation { get; set; }
        public string ProviderName { get; set; }
        public string ConcludedDate { get; set; }
        public string Status { get; set; }
        public int RequestId { get; set; }
    }

    public class EmailLogViewModel
    {
        public string Recipint { get; set; }
        public string Action { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string Sent { get; set; }
        public string SentTries { get; set; }
        public string ConfirmationNumber { get; set; }
        public List<EmailLog> EmailLogList { get; set; }
        public List<AspNetRole> AspNetRoleList { get; set; }
    }
}
