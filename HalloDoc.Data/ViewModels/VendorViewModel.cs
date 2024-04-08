using HalloDoc.DataModels;

namespace Services.ViewModels
{
    public class VendorViewModel
    {
        public string profession { get; set; }
        public string businessName { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string phone { get; set; }
        public List<Region> regionlist { get; set; }
        public List<HealthProfessional> healthProfessionallist { get; set; }
        public List<HealthProfessionalType> healthProfessionalTypelist { get; set; }
    }
}
