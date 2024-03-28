using HalloDoc.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class ProviderDetailsViewModel
    {
        public List<Physician> physician { get; set; }
        [Required]
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string medicallicencenumber { get; set; }
        public string npinumber { get; set; }

        public List<Region> regionlist { get; set; }
        public List<PhysicianRegion> selectedregionlist { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string alterphonenumber { get; set; }
        public string businessname { get; set; }
        public string businesswebsite { get; set; }
        public string photo { get; set; }
        public string signature { get; set; }
        public string adminnote { get; set; }
        public int physicianid { get; set; }
        public bool IsAgreementDoc { get; set; }
        public bool IsBackgroundDoc { get; set; }
        public bool IsCredentialDoc { get; set; }
        public bool IsNonDisclosureDoc { get; set; }
        public bool IsLicenseDoc { get; set; }

        public int[] selectedregion { get; set; }
        public string ContactMessage { get; set; }
        public List<AspNetRole> role { get; set; }

    }
}





