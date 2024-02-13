using HalloDoc.DataModels;

namespace HalloDoc.Models.ViewModel
{
    public class PatientDashboard
    {
        public User users { get; set; }

        public List<Request> requests { get; set; }

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

    }
}
