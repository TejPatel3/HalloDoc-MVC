﻿using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using System.Globalization;

namespace HelloDoc.Controllers.Scheduling
{
    public class SchedulingController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IunitOfWork _unitOfWork;

        public SchedulingController(ApplicationDbContext context, IunitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Scheduling()
        {
            SchedulingViewModel modal = new SchedulingViewModel();
            modal.regions = _unitOfWork.tableData.GetRegionList();
            return View(modal);
        }


        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid, int status)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _context.PhysicianRegions.Include(u => u.Physician).Where(u => u.RegionId == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _context.Physicians.ToList();
            }
            if (HttpContext.Session.GetInt32("PhysicianId") != null)
            {
                var phyid = HttpContext.Session.GetInt32("PhysicianId");
                MonthWiseScheduling month = new MonthWiseScheduling
                {
                    date = currentDate,
                };
                //if (regionid != 0 && status != 0)
                //{
                //    var dataphy = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();

                //    month.shiftdetails = ;
                //}
                //else if (regionid != 0)
                //{
                //    month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift.PhysicianId).Where(m => m.IsDeleted != new BitArray(new[] { true }) && m.Shift.PhysicianId == phyid).ToList();

                //}
                if (status != 0)
                {
                    month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.Shift.PhysicianId == phyid).ToList();

                }
                else
                {
                    month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.IsDeleted != new BitArray(new[] { true }) && m.Shift.PhysicianId == phyid).ToList();
                }
                return PartialView("_MonthWise", month);
            }
            else
            {
                switch (PartialName)
                {

                    case "_DayWise":
                        DayWiseScheduling day = new DayWiseScheduling
                        {
                            date = currentDate,
                            physicians = physician,
                        };
                        if (regionid != 0 && status != 0)
                        {
                            day.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        else if (regionid != 0)
                        {
                            day.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else if (status != 0)
                        {
                            day.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else
                        {
                            day.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        return PartialView("_DayWise", day);

                    case "_WeekWise":
                        WeekWiseScheduling week = new WeekWiseScheduling
                        {
                            date = currentDate,
                            physicians = physician,

                        };
                        if (regionid != 0 && status != 0)
                        {
                            week.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        else if (regionid != 0)
                        {
                            week.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else if (status != 0)
                        {
                            week.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else
                        {
                            week.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        return PartialView("_WeekWise", week);

                    case "_MonthWise":
                        MonthWiseScheduling month = new MonthWiseScheduling
                        {
                            date = currentDate,
                            physicians = physician,
                        };
                        if (regionid != 0 && status != 0)
                        {
                            month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        else if (regionid != 0)
                        {
                            month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else if (status != 0)
                        {
                            month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true })).ToList();

                        }
                        else
                        {
                            month.shiftdetails = _context.ShiftDetails.Include(u => u.Shift).Where(m => m.IsDeleted != new BitArray(new[] { true })).ToList();
                        }
                        return PartialView("_MonthWise", month);

                    default:
                        return PartialView("_DayWise");
                }
            }
        }

        public List<Physician> filterregion(string regionid)
        {
            List<Physician> physicians = _context.PhysicianRegions.Where(u => u.RegionId.ToString() == regionid).Select(y => y.Physician).ToList();
            return physicians;
        }

        public IActionResult AddShift(SchedulingViewModel model)
        {
            var idcheck = "";
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var physicianid = HttpContext.Session.GetInt32("PhysicianId");
            if (adminid != null)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);
                idcheck = admin.AspNetUserId;
                //AspNetUser aspnetadmin = _context.AspNetUsers.FirstOrDefault(m => m.Id == admin.AspNetUserId);
            }
            else
            {
                var phy = _context.Physicians.FirstOrDefault(m => m.PhysicianId == physicianid);
                idcheck = phy.Id;
            }
            AspNetUser aspnetadmin = _context.AspNetUsers.FirstOrDefault(m => m.Id == idcheck);
            var chk = Request.Form["repeatdays"].ToList();
            var shiftid = _context.Shifts.Where(u => u.PhysicianId == model.providerid).Select(u => u.ShiftId).ToList();
            if (shiftid.Count() > 0)
            {
                foreach (var obj in shiftid)
                {
                    //var shiftdetailchk = _context.ShiftDetails.Where(u => u.ShiftId == obj && u.ShiftDate == model.shiftdate).ToList();
                    var shiftdetailchk = _context.ShiftDetails.Where(u => u.ShiftId == obj && u.ShiftDate == DateOnly.FromDateTime(model.shiftdate) && u.IsDeleted != new BitArray(new[] { true })).ToList();
                    if (shiftdetailchk.Count() > 0)
                    {
                        foreach (var item in shiftdetailchk)
                        {
                            if ((model.starttime >= DateTime.Parse(item.StartTime.ToString()) && model.starttime <= DateTime.Parse(item.EndTime.ToString())) || (model.endtime >= DateTime.Parse(item.StartTime.ToString()) && model.endtime <= DateTime.Parse(item.EndTime.ToString())))
                            {
                                TempData["shiftalreadyassigned"] = "Shift is already assigned in this time";
                                if (adminid != null)
                                {
                                    return RedirectToAction("Scheduling");
                                }
                                else
                                {
                                    return RedirectToAction("MyScheduling", "ProviderSide");
                                }
                            }
                        }
                    }
                }
            }

            Shift shift = new Shift
            {
                StartDate = DateOnly.FromDateTime(model.shiftdate),
                RepeatUpto = model.repeatcount,
                CreatedDate = DateTime.Now,
                CreatedBy = aspnetadmin.Id
            };
            if (adminid != null)
            {
                shift.PhysicianId = model.providerid;
            }
            else
            {
                shift.PhysicianId = (int)physicianid;
            }
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
                            Status = 1,
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
            TempData["ShiftCreated"] = "Shift Created successfully";

            if (adminid != null)
            {
                return RedirectToAction("Scheduling");
            }
            else
            {
                return RedirectToAction("MyScheduling", "ProviderSide");
            }
        }

        public void ViewShiftModelSavebtn(SchedulingViewModel obj)
        {
            ShiftDetail shiftdetail = _context.ShiftDetails.Include(m => m.Shift).FirstOrDefault(m => m.ShiftDetailId == obj.shiftdetailid);
            Physician physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == shiftdetail.Shift.PhysicianId);
            if (shiftdetail.StartTime != obj.starttime || shiftdetail.EndTime != obj.endtime)
            {
                shiftdetail.StartTime = obj.starttime;
                shiftdetail.EndTime = obj.endtime;
                _context.ShiftDetails.Update(shiftdetail);
                _context.SaveChanges();
            }
            //return RedirectToAction("Scheduling");
        }
        public IActionResult ViewShiftModelDeletebtn(int shiftdetailsid)
        {
            ShiftDetail shiftdetail = _context.ShiftDetails.FirstOrDefault(m => m.ShiftDetailId == shiftdetailsid);
            if (shiftdetail != null)
            {
                shiftdetail.IsDeleted = new BitArray(new[] { true });
                _context.Update(shiftdetail);
                _context.SaveChanges();
            }
            TempData["deleteshift"] = "delete shift success";
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
        //public List<Physician> Getondutyphysicianlist(List<ShiftDetail> shiftdetaillist)
        //{
        //    List<Physician> physicians;
        //    if (shiftdetaillist != null)
        //    {

        //        foreach (var item in shiftdetaillist)
        //        {
        //            physicians = _context.Physicians.Where(m => m.PhysicianId == item.Shift.PhysicianId).ToList();
        //        }
        //        return physicians;
        //    }
        //        return ;

        //}
        public IActionResult ProviderOnCall(string PartialName, string date, int regionid, int status)
        {
            ProviderOnCall model = new ProviderOnCall();
            DateOnly dateOnly;
            IEnumerable<Physician> physicianlist = new List<Physician>();
            if (regionid != 0)
            {
                physicianlist = _context.Physicians.Where(m => m.RegionId == regionid).ToList();
            }
            else
            {
                physicianlist = _context.Physicians.ToList();
            }
            if (PartialName == "_WeekWise")
            {
                DateTime dateTime = DateTime.Parse(date);
                dateOnly = DateOnly.FromDateTime(dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Sunday));
            }
            else if (PartialName == "_MonthWise")
            {
                dateOnly = DateOnly.FromDateTime(DateTime.Parse(date).AddDays(-DateTime.Parse(date).Day + 1));
            }
            else
            {
                dateOnly = DateOnly.ParseExact(date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            if (PartialName == "_DayWise")
            {

                List<ShiftDetail> shiftdetaillist = new List<ShiftDetail>();
                if (status != 0 && regionid != 0)
                {
                    shiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.RegionId == regionid && m.ShiftDate == dateOnly).ToList();
                }
                else if (regionid != 0)
                {

                    shiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == dateOnly).ToList();
                }
                else if (status != 0)
                {
                    shiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == dateOnly).ToList();

                }
                else
                {
                    shiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.ShiftDate == dateOnly && m.IsDeleted != new BitArray(new[] { true })).ToList();
                }

                IEnumerable<Physician> ondutyphysician = new List<Physician>();
                foreach (var item in shiftdetaillist)
                {
                    var x = _context.Physicians.Where(m => m.PhysicianId == item.Shift.PhysicianId).ToList();
                    ondutyphysician = ondutyphysician.Concat(x);
                }
                model.offdutyphysicianlist = physicianlist.Except(ondutyphysician);
                model.ondutyphysicianlist = ondutyphysician.Distinct();
            }

            if (PartialName == "_WeekWise")
            {
                List<DateOnly> weekDates = new List<DateOnly>();
                for (int i = 0; i < 7; i++)
                {
                    weekDates.Add(dateOnly.AddDays(i));
                }
                List<ShiftDetail> weekShiftdetaillist = new List<ShiftDetail>();
                foreach (var weekDate in weekDates)
                {
                    List<ShiftDetail> tempShiftdetaillist = new List<ShiftDetail>();
                    if (status != 0 && regionid != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.RegionId == regionid && m.ShiftDate == weekDate).ToList();
                    }
                    else if (regionid != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == weekDate).ToList();

                    }
                    else if (status != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == weekDate).ToList();

                    }
                    else
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.ShiftDate == weekDate && m.IsDeleted != new BitArray(new[] { true })).ToList();
                    }
                    weekShiftdetaillist.AddRange(tempShiftdetaillist);
                }
                IEnumerable<Physician> weekondutyphysician = new List<Physician>();
                foreach (var item in weekShiftdetaillist)
                {
                    var x = _context.Physicians.Where(m => m.PhysicianId == item.Shift.PhysicianId).ToList();
                    weekondutyphysician = weekondutyphysician.Concat(x);
                }
                model.offdutyphysicianlist = physicianlist.Except(weekondutyphysician);
                model.ondutyphysicianlist = weekondutyphysician.Distinct();
            }
            if (PartialName == "_MonthWise")
            {
                List<DateOnly> monthDates = new List<DateOnly>();
                for (int i = 1; i <= DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month); i++)
                {
                    monthDates.Add(dateOnly.AddDays(i - 1));
                }

                List<ShiftDetail> monthShiftdetaillist = new List<ShiftDetail>();
                foreach (var monthDate in monthDates)
                {
                    List<ShiftDetail> tempShiftdetaillist = new List<ShiftDetail>();
                    if (status != 0 && regionid != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.Status == status && m.IsDeleted != new BitArray(new[] { true }) && m.RegionId == regionid && m.ShiftDate == monthDate).ToList();
                    }
                    else if (regionid != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == monthDate).ToList();

                    }
                    else if (status != 0)
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.RegionId == regionid && m.IsDeleted != new BitArray(new[] { true }) && m.ShiftDate == monthDate).ToList();

                    }
                    else
                    {
                        tempShiftdetaillist = _context.ShiftDetails.Include(s => s.Shift).Where(m => m.ShiftDate == monthDate && m.IsDeleted != new BitArray(new[] { true })).ToList();
                    }
                    monthShiftdetaillist.AddRange(tempShiftdetaillist);
                }

                IEnumerable<Physician> monthondutyphysician = new List<Physician>();
                foreach (var item in monthShiftdetaillist)
                {
                    var x = _context.Physicians.Where(m => m.PhysicianId == item.Shift.PhysicianId).ToList();
                    monthondutyphysician = monthondutyphysician.Concat(x);
                }
                model.offdutyphysicianlist = physicianlist.Except(monthondutyphysician);
                model.ondutyphysicianlist = monthondutyphysician.Distinct();
            }
            model.regions = _context.Regions.ToList();
            model.selectedRegionid = regionid;
            return View(model);
        }

        public IActionResult ShiftForReview()
        {
            SchiftsForReviewViewModel model = new SchiftsForReviewViewModel();
            model.regionlist = _unitOfWork.vendor.getRegionList();

            return View(model);
        }
        public IActionResult GetShiftData(string currentPartial, string date, int regionid, int pagesize, int currentpage)
        {
            SchiftsForReviewViewModel model = _unitOfWork.scheduling.getShiftData(currentPartial, date, regionid, pagesize, currentpage);
            return PartialView("_ShiftForReviewTableData", model);
        }
        public int ShiftDataCountForPagination(string currentPartial, string date, int regionid)
        {
            var curdate = DateTime.Parse(date);
            var currentdate = DateOnly.FromDateTime(curdate);
            int count = _unitOfWork.scheduling.totalpagecount(currentPartial, date, regionid);
            return count;
        }
        [HttpPost]
        public void SelectedShiftUpdate(List<string> selectedshiftvalues, string clickvalue)
        {
            if (clickvalue == "approve")
            {
                foreach (var shiftdetailid in selectedshiftvalues)
                {
                    ShiftDetail x = _context.ShiftDetails.FirstOrDefault(s => s.ShiftDetailId == int.Parse(shiftdetailid));
                    x.Status = 2;
                    _context.ShiftDetails.Update(x);

                }
            }
            else if (clickvalue == "delete")
            {
                foreach (var shiftdetailid in selectedshiftvalues)
                {
                    ShiftDetail x = _context.ShiftDetails.FirstOrDefault(s => s.ShiftDetailId == int.Parse(shiftdetailid));
                    x.IsDeleted = new BitArray(new[] { true });
                    _context.ShiftDetails.Update(x);
                }
            }
            _context.SaveChanges();
        }
    }
}

