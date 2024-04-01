using HalloDoc.DataModels;
namespace Services.ViewModels
{
    public class Scheduling
    {
        public List<Region> regions { get; set; }
        public int Regionid { get; set; }
        public int providerid { get; set; }
        public DateTime shiftdate { get; set; }
        public TimeOnly starttime { get; set; }
        public TimeOnly endtime { get; set; }
        public int repeatcount { get; set; }
        public List<Physician> physicianlist { get; set; }
        public List<Physician> regionlist { get; set; }
    }
    public class DayWiseScheduling
    {
        public List<Physician> physicians { get; set; }
        public DateTime date { get; set; }
        public List<ShiftDetail> shiftdetails { get; set; }
    }
    public class MonthWiseScheduling
    {
        public DateTime date { get; set; }

    }
    public class WeekWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
    }

}
