using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using System.Globalization;

namespace Services.Implementation
{
    public class SchedulingRepository : ISchedulingRepository
    {
        private readonly ApplicationDbContext _context;
        public SchedulingRepository()
        {
            _context = new ApplicationDbContext();
        }
        public int totalpagecount(string currentPartial, string date, int regionid)
        {
            int count = 0;
            DateOnly dateOnly = DateOnly.ParseExact(date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture); ;
            BitArray bitset = new BitArray(1);
            bitset[0] = false;
            if (currentPartial == "_WeekWise")
            {
                DateTime weekdate = DateTime.Parse(date);
                int daysToSubtract = weekdate.DayOfWeek - DayOfWeek.Sunday;
                if (daysToSubtract < 0)
                {
                    daysToSubtract += 7;
                }
                DateTime previousSunday = weekdate.AddDays(-daysToSubtract);
                DateOnly startOfWeek = DateOnly.FromDateTime(previousSunday);
                DateOnly endOfWeek = startOfWeek.AddDays(6);
                if (regionid != 0)
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate >= startOfWeek && m.ShiftDate <= endOfWeek && m.IsDeleted[0] == false && m.RegionId == regionid && m.Status == 1).Count();
                else
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate >= startOfWeek && m.ShiftDate <= endOfWeek && m.IsDeleted[0] == false && m.Status == 1).Count();
            }
            else if (currentPartial == "_MonthWise")
            {

                // Get the start and end dates of the month for the given date
                DateTime daten = dateOnly.ToDateTime(TimeOnly.MinValue);
                DateTime startOfMonth = new DateTime(daten.Year, daten.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                // Get the count of shift details for the month of the given date
                if (regionid != 0)
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate.Month == dateOnly.Month && m.ShiftDate.Year == dateOnly.Year && m.IsDeleted[0] == false && m.RegionId == regionid && m.Status == 1).Count();
                else
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate.Month == dateOnly.Month && m.ShiftDate.Year == dateOnly.Year && m.IsDeleted[0] == false && m.Status == 1).Count();
            }
            else
            {
                if (regionid != 0)
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate == dateOnly && m.IsDeleted[0] == false && m.Status == 1 && m.RegionId == regionid).Count();
                else
                    count = _context.ShiftDetails.AsEnumerable().Where(m => m.ShiftDate == dateOnly && m.IsDeleted[0] == false && m.Status == 1).Count();
            }
            return count;
        }
        public SchiftsForReviewViewModel getShiftData(string currentPartial, string date, int regionid, int pagesize, int currentpage)
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = false;
            DateOnly dateOnly = DateOnly.ParseExact(date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            SchiftsForReviewViewModel model = new SchiftsForReviewViewModel();
            var alldata = new List<ShiftDetail>();
            if (currentPartial == "_DayWise")
            {
                if (regionid != 0)
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate == dateOnly && m.IsDeleted == bitset && m.Status == 1 && m.RegionId == regionid).ToList();

                else
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate == dateOnly && m.IsDeleted == bitset && m.Status == 1).ToList();
            }
            if (currentPartial == "_WeekWise")
            {
                //DateTime weekdate = DateTime.Parse(date);
                int daysToSubtract = dateOnly.DayOfWeek - DayOfWeek.Sunday;
                if (daysToSubtract < 0)
                {
                    daysToSubtract += 7;
                }

                DateOnly startOfWeek = dateOnly.AddDays(-daysToSubtract);
                DateOnly endOfWeek = startOfWeek.AddDays(6);
                dateOnly = DateOnly.ParseExact(date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (regionid != 0)
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate >= startOfWeek && m.ShiftDate <= endOfWeek && m.Status == 1 && m.IsDeleted == bitset && m.RegionId == regionid).ToList();
                else
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate >= startOfWeek && m.ShiftDate <= endOfWeek && m.Status == 1 && m.IsDeleted == bitset).ToList();
            }
            if (currentPartial == "_MonthWise")
            {
                DateTime daten = dateOnly.ToDateTime(TimeOnly.MinValue);
                DateTime startOfMonth = new DateTime(daten.Year, daten.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                if (regionid != 0)
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate.Month == dateOnly.Month && m.Status == 1 && m.ShiftDate.Year == dateOnly.Year && m.IsDeleted == bitset && m.RegionId == regionid).ToList();
                else
                    alldata = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.ShiftDate.Month == dateOnly.Month && m.Status == 1 && m.ShiftDate.Year == dateOnly.Year && m.IsDeleted == bitset).ToList();
            }
            var skip = (currentpage - 1) * pagesize;
            model.shiftDetaillist = alldata.Skip(skip).Take(pagesize).ToList();
            model.regionlist = _context.Regions.ToList();
            model.physicians = _context.Physicians.ToList();
            return model;
        }

    }
}
