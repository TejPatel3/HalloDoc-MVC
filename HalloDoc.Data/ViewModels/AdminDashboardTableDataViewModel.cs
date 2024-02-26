namespace Services.ViewModels
{
    public class AdminDashboardTableDataViewModel
    {
        public string PatientName { get; set; }
        public DateOnly PatientDOB { get; set; }
        public string RequestorName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string PatientPhone { get; set; }
        public string RequestorPhone { get; set; }
        public string RequestorEmail { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string ProviderEmail { get; set; }
        public string PatientEmail { get; set; }
        public int RequestType { get; set; }

        public int requestid { get; set; }

        public enum Requestby
        {
            first,
            Patient,
            Friend_Family,
            Concierge,
            Business_Partner
        }
        public string RequestTypeName(int by)
        {
            string By = ((Requestby)by).ToString();
            return By;
        }
    }
}
