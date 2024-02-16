using HalloDoc.DataModels;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.Models.ViewModel
{
    public class PatientDashboard
    {
        public User users { get; set; }

        public List<Request> requests { get; set; }

        public List<RequestWiseFile> wiseFiles { get; set; }

        enum statusName
        {
            january,
            Unassigned,
            Cancelled,
            MdEnRoute,
            MdOnSite,
            Closed,
            Clear,
            Unpaid
        }

        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }
        public List<IFormFile?> Upload { get; set; }
        public int requestid { get; set; }
        public DateTime DOB { get; set; }
    }
}
