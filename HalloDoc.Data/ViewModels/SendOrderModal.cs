using HalloDoc.DataModels;

namespace Services.ViewModels
{
    public class SendOrderModal
    {

        public List<HealthProfessionalType> Healthprofessionaltypes { get; set; }

        public List<HealthProfessional> Healthprofessionals { get; set; }

        public int requestid { get; set; }

        public string BusinessContact { get; set; }

        public string Email { get; set; }

        public string FaxNumber { get; set; }

        public string OrderDetails { get; set; }

        public DateTime CreatedDate { get; set; }


        public int refill { get; set; }
    }
}
