using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class PayRateViewModel
    {
        public string FieldCheck { get; set; }
        public int PhysicianId { get; set; }
        [Required]
        public int? NightShiftWeekEnd { get; set; }
        [Required]
        public int? Shift { get; set; }
        [Required]
        public int? HouseCallNightWeekEnd { get; set; }
        [Required]
        public int? PhoneConsult { get; set; }
        [Required]
        public int? PhoneConsultNightWeekEnd { get; set; }
        [Required]
        public int? BatchTesting { get; set; }
        [Required]
        public int? HouseCalls { get; set; }
    }
}
