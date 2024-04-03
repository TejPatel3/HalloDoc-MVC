using HalloDoc.DataModels;
namespace Services.ViewModels
{
    public class SchedulingViewModel
    {

        public List<Region> regions { get; set; }
        public List<PhysicianRegion> physicianregionlist { get; set; }
        public int regionid { get; set; }
        public int providerid { get; set; }
        public DateOnly shiftdateviewshift { get; set; }
        public DateTime shiftdate { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int repeatcount { get; set; }
        public int shiftid { get; set; }
        public int shiftdetailid { get; set; }
        public string physicianname { get; set; }
        public string regionname { get; set; }

    }
    public class DayWiseScheduling
    {
        public int shiftid { get; set; }
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
        public List<ShiftDetail> shiftdetails { get; set; }
    }
    public class MonthWiseScheduling
    {
        public DateTime date { get; set; }
        public List<ShiftDetail> shiftdetails { get; set; }
        public List<Physician> physicians { get; set; }

    }
    public class WeekWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }

        public List<ShiftDetail> shiftdetails { get; set; }

    }

}
