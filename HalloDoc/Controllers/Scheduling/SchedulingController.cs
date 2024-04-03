using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System.Collections;

namespace HelloDoc.Controllers.Scheduling
{
    public class SchedulingController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SchedulingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Scheduling()
        {
            SchedulingViewModel modal = new SchedulingViewModel();
            modal.regions = _context.Regions.ToList();
            return View(modal);
        }


        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _context.PhysicianRegions.Include(u => u.Physician).Where(u => u.RegionId == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _context.Physicians.ToList();
            }

            switch (PartialName)
            {

                case "_DayWise":
                    DayWiseScheduling day = new DayWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.ShiftDetails.Include(u => u.Shift).ToList()
                    };
                    return PartialView("_DayWise", day);

                case "_WeekWise":
                    WeekWiseScheduling week = new WeekWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.ShiftDetails.Include(u => u.Shift).ToList()

                    };
                    return PartialView("_WeekWise", week);

                case "_MonthWise":
                    MonthWiseScheduling month = new MonthWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.ShiftDetails.Include(u => u.Shift).ToList()
                    };
                    return PartialView("_MonthWise", month);

                default:
                    return PartialView("_DayWise");
            }
        }

        public List<Physician> filterregion(string regionid)
        {
            List<Physician> physicians = _context.PhysicianRegions.Where(u => u.RegionId.ToString() == regionid).Select(y => y.Physician).ToList();
            return physicians;
        }

        public IActionResult AddShift(SchedulingViewModel model)
        {
            int adminid = (int)HttpContext.Session.GetInt32("AdminId");
            var admin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);
            AspNetUser aspnetadmin = _context.AspNetUsers.FirstOrDefault(m => m.Id == admin.AspNetUserId);
            var chk = Request.Form["repeatdays"].ToList();
            var shiftid = _context.Shifts.Where(u => u.PhysicianId == model.providerid).Select(u => u.ShiftId).ToList();
            if (shiftid.Count() > 0)
            {
                foreach (var obj in shiftid)
                {
                    //var shiftdetailchk = _context.ShiftDetails.Where(u => u.ShiftId == obj && u.ShiftDate == model.shiftdate).ToList();
                    var shiftdetailchk = _context.ShiftDetails.Where(u => u.ShiftId == obj && u.ShiftDate == DateOnly.FromDateTime(model.shiftdate)).ToList();
                    if (shiftdetailchk.Count() > 0)
                    {
                        foreach (var item in shiftdetailchk)
                        {
                            if ((model.starttime >= DateTime.Parse(item.StartTime.ToString()) && model.starttime <= DateTime.Parse(item.EndTime.ToString())) || (model.endtime >= DateTime.Parse(item.StartTime.ToString()) && model.endtime <= DateTime.Parse(item.EndTime.ToString())))
                            {
                                TempData["error"] = "Shift is already assigned in this time";
                                return RedirectToAction("Scheduling");
                            }
                        }
                    }
                }
            }
            Shift shift = new Shift
            {
                PhysicianId = model.providerid,
                StartDate = DateOnly.FromDateTime(model.shiftdate),
                RepeatUpto = model.repeatcount,
                CreatedDate = DateTime.Now,
                CreatedBy = aspnetadmin.Id
            };
            foreach (var obj in chk)
            {
                shift.WeekDays += obj;
            }
            if (model.repeatcount > 0)
            {
                shift.IsRepeat = new BitArray(new[] { true });
            }
            else
            {
                shift.IsRepeat = new BitArray(new[] { false });
            }
            _context.Shifts.Add(shift);
            _context.SaveChanges();
            DateTime curdate = model.shiftdate;
            ShiftDetail shiftdetail = new ShiftDetail();
            shiftdetail.ShiftId = shift.ShiftId;
            shiftdetail.ShiftDate = DateOnly.FromDateTime(curdate);
            shiftdetail.Status = 1;
            //shiftdetail.ShiftDate = curdate;
            shiftdetail.RegionId = model.regionid;
            shiftdetail.StartTime = model.starttime;
            shiftdetail.EndTime = model.endtime;
            shiftdetail.IsDeleted = new BitArray(new[] { false });
            _context.ShiftDetails.Add(shiftdetail);
            _context.SaveChanges();

            var dayofweek = model.shiftdate.DayOfWeek.ToString();
            int valueforweek;
            if (dayofweek == "Sunday")
            {
                valueforweek = 0;
            }
            else if (dayofweek == "Monday")
            {
                valueforweek = 1;
            }
            else if (dayofweek == "Tuesday")
            {
                valueforweek = 2;
            }
            else if (dayofweek == "Wednesday")
            {
                valueforweek = 3;
            }
            else if (dayofweek == "Thursday")
            {
                valueforweek = 4;
            }
            else if (dayofweek == "Friday")
            {
                valueforweek = 5;
            }
            else
            {
                valueforweek = 6;
            }
            if (shift.IsRepeat[0] == true)
            {
                for (int j = 0; j < shift.WeekDays.Count(); j++)
                {
                    var z = shift.WeekDays;
                    var p = shift.WeekDays.ElementAt(j).ToString();
                    int ele = Int32.Parse(p);
                    int x;
                    if (valueforweek > ele)
                    {
                        x = 6 - valueforweek + 1 + ele;
                    }
                    else
                    {
                        x = ele - valueforweek;
                    }
                    if (x == 0)
                    {
                        x = 7;
                    }
                    DateTime newcurdate = model.shiftdate.AddDays(x);
                    for (int i = 0; i < model.repeatcount; i++)
                    {
                        ShiftDetail shiftdetailnew = new ShiftDetail
                        {
                            ShiftId = shift.ShiftId,
                            //ShiftDate = newcurdate,
                            ShiftDate = DateOnly.FromDateTime(newcurdate),
                            RegionId = model.regionid,
                            StartTime = new DateTime(newcurdate.Year, newcurdate.Month, newcurdate.Day, model.starttime.Hour, model.starttime.Minute, model.starttime.Second),
                            EndTime = new DateTime(newcurdate.Year, newcurdate.Month, newcurdate.Day, model.endtime.Hour, model.endtime.Minute, model.endtime.Second),
                            IsDeleted = new BitArray(new[] { false })
                        };
                        _context.ShiftDetails.Add(shiftdetailnew);
                        _context.SaveChanges();
                        newcurdate = newcurdate.AddDays(7);
                    }
                }
            }
            return RedirectToAction("Scheduling");
        }
        //public IActionResult viewShiftEdit(SchedulingViewModel obj)
        //{
        //    var currentDate = DateTime.Parse(date);
        //    List<Physician> physician = _context.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
        //    if (regionid == 0)
        //    {
        //        physician = _context.Physicians.ToList();
        //    }
        //    DayWiseScheduling day = new DayWiseScheduling
        //    {
        //        date = currentDate,
        //        physicians = physician,
        //        shiftdetails = _context.ShiftDetails.Include(u => u.Shift).ToList()
        //    };
        //    return PartialView("_DayWise", day);
        //}
        public IActionResult ViewShiftModelSavebtn(SchedulingViewModel obj)
        {
            ShiftDetail shiftdetail = _context.ShiftDetails.Include(m => m.Shift).FirstOrDefault(m => m.ShiftDetailId == obj.shiftdetailid);
            Physician physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == shiftdetail.Shift.PhysicianId);
            if (shiftdetail.StartTime != obj.starttime)
            {
                shiftdetail.StartTime = obj.starttime;
                shiftdetail.EndTime = obj.endtime;
                _context.ShiftDetails.Update(shiftdetail);
                _context.SaveChanges();
            }
            return RedirectToAction("Scheduling");
        }
        public IActionResult ViewShiftModelDeletebtn(int shiftdetailsid)
        {
            ShiftDetail shiftdetail = _context.ShiftDetails.FirstOrDefault(m => m.ShiftDetailId == shiftdetailsid);
            if (shiftdetail != null)
            {
                _context.Remove(shiftdetail);
                _context.SaveChanges();
            }
            return RedirectToAction("Scheduling");
        }
        public IActionResult ViewShiftModelReturnbtn(int shiftdetailsid)
        {
            ShiftDetail shiftdetail = _context.ShiftDetails.FirstOrDefault(m => m.ShiftDetailId == shiftdetailsid);
            if (shiftdetail != null)
            {
                if (shiftdetail.Status == 1)
                {

                    shiftdetail.Status = 2;
                }
                else if (shiftdetail.Status == 2)
                {
                    shiftdetail.Status = 1;
                }
                else
                {
                    shiftdetail.Status = 1;

                }
                _context.Update(shiftdetail);
                _context.SaveChanges();
            }
            return RedirectToAction("Scheduling");
        }
        public SchedulingViewModel ViewShiftOpen(int shiftdetailid)
        {

            ShiftDetail shiftdata = _context.ShiftDetails.Include(x => x.Shift).FirstOrDefault(s => s.ShiftDetailId == shiftdetailid);

            SchedulingViewModel model = new SchedulingViewModel
            {
                regionname = _context.Regions.FirstOrDefault(r => r.RegionId == shiftdata.RegionId).RegionId.ToString(),
                physicianname = _context.Physicians.FirstOrDefault(p => p.PhysicianId == shiftdata.Shift.PhysicianId).FirstName + " "
                                + _context.Physicians.FirstOrDefault(p => p.PhysicianId == shiftdata.Shift.PhysicianId).LastName,
                shiftdateviewshift = shiftdata.ShiftDate,
                starttime = shiftdata.StartTime,
                endtime = shiftdata.EndTime,
            };

            return model;

        }
    }
}
