namespace Services.ViewModels
{
    public class PayRateViewModel
    {
        public string FieldCheck { get; set; }
        public int PhysicianId { get; set; }
        public int? NightShiftWeekEnd { get; set; }
        public int? Shift { get; set; }
        public int? HouseCallNightWeekEnd { get; set; }
        public int? PhoneConsult { get; set; }
        public int? PhoneConsultNightWeekEnd { get; set; }
        public int? BatchTesting { get; set; }
        public int? HouseCalls { get; set; }
        public int? PayrateValue { get; set; }
    }
}
