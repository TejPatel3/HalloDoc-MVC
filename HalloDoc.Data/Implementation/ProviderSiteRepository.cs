using DataModels.DataContext;
using DataModels.DataModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;

namespace Services.Contracts
{
    public class ProviderSiteRepository : IProviderSiteRepository
    {
        private readonly ApplicationDbContext _db;
        public ProviderSiteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<InvoicingView> GetMonthHalfTimeSheetData(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }
            //if (month == DateTime.Now.Month)
            //{
            //    endDate = new DateTime(year, month, DateTime.Now.Day);
            //}

            IEnumerable<InvoicingView> result;

            bool isSheet = _db.Timesheets.Any(a => a.StartDate == startDate && a.PhysicianId == physicianId);
            if (isSheet)
            {
                result = from td in _db.TimesheetDetails.Include(i => i.Timesheet)
                         orderby td.Date
                         where td.Date <= endDate && td.Date >= startDate && td.Timesheet.PhysicianId == physicianId
                         select new InvoicingView
                         {
                             TimeSheetId = td.Timesheet.TimesheetId,
                             TimeSheetDetailId = td.TimesheetDetailId,
                             physicianId = physicianId,
                             Date = td.Date,
                             onCallHours = td.OnCallHours,
                             totalHours = td.TotalHours,
                             isWeekend = td.IsHoliday ?? false,
                             noOfHouseCalls = td.NoOfHouseCall != null ? td.NoOfHouseCall : 0,
                             noOfPhoneConsults = td.NoOfPhoneConsult != null ? td.NoOfPhoneConsult : 0,
                         };
            }
            else
            {
                var allDatesInRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                     .Select(offset => startDate.AddDays(offset));

                result = from date in allDatesInRange
                         join sd in _db.ShiftDetails.Include(i => i.Shift)
                           on date equals DateTime.Parse(sd.ShiftDate.ToString()) into shiftGroup
                         from shift in shiftGroup.DefaultIfEmpty()
                         group shift by date into g
                         select new InvoicingView
                         {
                             Date = g.Key,
                             onCallHours = Convert.ToInt32(Math.Round(g.Where(x => x != null && x.Shift.PhysicianId == physicianId)
                                                                         .Sum(x => (x.EndTime - x.StartTime).TotalHours))),
                             totalHours = Convert.ToInt32(Math.Round(g.Where(x => x != null && x.Shift.PhysicianId == physicianId)
                                                                         .Sum(x => (x.EndTime - x.StartTime).TotalHours))),
                             physicianId = physicianId,
                             noOfHouseCalls = 0,
                             noOfPhoneConsults = 0,
                             isWeekend = false,
                         };

                //Timesheet timesheet = new Timesheet
                //{
                //    PhysicianId = physicianId,
                //    StartDate = startDate,
                //    IsFinalize = false,
                //    IsApproved = false,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //};
                //_db.Add(timesheet);
                //_db.SaveChanges();

                //foreach (var item in allDatesInRange)
                //{


                //    TimesheetDetail timesheetDetail = new TimesheetDetail
                //    {
                //        TimesheetId = timesheet.TimesheetId,
                //        Date = item.Date,

                //    };
                //}

            }

            return result;
        }

        public IEnumerable<ReceiptView> GetMonthHalfReceiptData(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }

            IEnumerable<ReceiptView> result;

            //bool isSheet = _db.TimesheetBills.Include(i => i.TimesheetDetail).Any(a => a.TimesheetDetail.Date == startDate && a.TimesheetDetail.Timesheet.PhysicianId == physicianId);
            //if (isSheet)
            //{
            //    result = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
            //             join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
            //             orderby tb.TimesheetDetail.Date
            //             where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId
            //             select new ReceiptView
            //             {
            //                 TimeSheetBillId = tb.TimesheetBillId,
            //                 TimeSheetDetailId = tb.TimesheetDetailId,
            //                 physicianId = physicianId,
            //                 Date = tb.TimesheetDetail.Date,
            //                 Item = tb.Item,
            //                 Amount = tb.Amount,
            //                 FileName = tb.FilePath,
            //             };
            //}
            //else
            //{
            //}
            var allDatesInRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                 .Select(offset => startDate.AddDays(offset));

            //result = from date in allDatesInRange
            //         join td in _db.TimesheetDetails
            //         on date equals td.Date into shiftGroup
            //         from td in shiftGroup.DefaultIfEmpty()
            //         join tb in _db.TimesheetBills on td.TimesheetDetailId equals tb.TimesheetDetailId into detailGroup
            //         from tb in detailGroup.DefaultIfEmpty()
            //         where (tb == null || tb.IsDeleted != true)
            //         select new ReceiptView
            //         {
            //             Date = date.Date,
            //             TimeSheetDetailId = td != null ? td.TimesheetDetailId : 0,
            //             physicianId = physicianId,
            //             TimeSheetBillId = tb != null ? tb.TimesheetBillId : 0,
            //             Item = tb != null ? tb.Item : "",
            //             Amount = tb != null ? tb.Amount : null,
            //             FileName = tb != null ? tb.FilePath : null,
            //         };

            result = from date in allDatesInRange
                     join td in _db.TimesheetDetails.Include(i => i.Timesheet).Where(d => d.Date >= startDate && d.Date <= endDate && d.Timesheet.PhysicianId == physicianId)
                     on date equals td.Date into shiftGroup
                     from td in shiftGroup.DefaultIfEmpty()
                     join tb in _db.TimesheetBills.Include(i => i.TimesheetDetail).Where(b => b.IsDeleted != true && (b.TimesheetDetail.Date >= startDate && b.TimesheetDetail.Date <= endDate))
                     on td?.TimesheetDetailId equals tb.TimesheetDetailId into detailGroup
                     from tb in detailGroup.DefaultIfEmpty()
                     select new ReceiptView
                     {
                         Date = date.Date,
                         TimeSheetDetailId = td != null ? td.TimesheetDetailId : 0,
                         physicianId = physicianId,
                         TimeSheetBillId = tb != null ? tb.TimesheetBillId : 0,
                         Item = tb != null ? tb.Item : "",
                         Amount = tb != null ? tb.Amount : null,
                         FileName = tb != null ? tb.FilePath : null,
                     };


            return result;
        }

        public IEnumerable<ReceiptView> GetMonthHalfTimesheetBillData(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }

            IEnumerable<ReceiptView> result;


            result = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
                     join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
                     orderby tb.TimesheetDetail.Date
                     where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId && tb.IsDeleted != true
                     select new ReceiptView
                     {
                         TimeSheetBillId = tb.TimesheetBillId,
                         TimeSheetDetailId = tb.TimesheetDetailId,
                         physicianId = physicianId,
                         Date = tb.TimesheetDetail.Date,
                         Item = tb.Item,
                         Amount = tb.Amount,
                         FileName = tb.FilePath,
                     };

            return result;
        }

        public void AddUpdateTimesheet(IEnumerable<InvoicingView> model)
        {
            bool isSheet = _db.Timesheets.Any(a => a.StartDate == model.ToList()[0].Date && a.PhysicianId == model.ToList()[0].physicianId);

            if (!isSheet)
            {
                Timesheet timesheet = new Timesheet
                {
                    PhysicianId = model.ToList()[0].physicianId,
                    StartDate = model.ToList()[0].Date,
                    IsFinalize = false,
                    IsApproved = false,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                };
                _db.Add(timesheet);
                _db.SaveChanges();

                foreach (var item in model.OrderBy(o => o.Date))
                {
                    TimesheetDetail timesheetDetail = new TimesheetDetail
                    {
                        TimesheetId = timesheet.TimesheetId,
                        Date = item.Date,
                        OnCallHours = item.onCallHours,
                        TotalHours = item.totalHours,
                        IsHoliday = item.isWeekend != null ? item.isWeekend : false,
                        NoOfHouseCall = item.noOfHouseCalls,
                        NoOfPhoneConsult = item.noOfPhoneConsults,
                        CreatedDate = DateTime.Now,
                    };
                    _db.Add(timesheetDetail);
                }
                _db.SaveChanges();
            }
            else
            {
                Timesheet timesheet = _db.Timesheets.FirstOrDefault(f => f.PhysicianId == model.ToList()[0].physicianId && f.StartDate == model.ToList()[0].Date);
                foreach (var item in model.OrderBy(o => o.Date))
                {
                    TimesheetDetail timesheetDetail = _db.TimesheetDetails.FirstOrDefault(f => f.TimesheetDetailId == item.TimeSheetDetailId);
                    timesheetDetail.TotalHours = item.totalHours;
                    timesheetDetail.IsHoliday = item.isWeekend != null ? item.isWeekend : false;
                    timesheetDetail.NoOfHouseCall = item.noOfHouseCalls;
                    timesheetDetail.NoOfPhoneConsult = item.noOfPhoneConsults;
                    _db.Update(timesheetDetail);
                }
                _db.SaveChanges();
            }
        }

        public void AddTimesheetBill(TimesheetBill model)
        {
            _db.Add(model);
        }

        public void DeleteTimesheetBill(int timesheetBillId)
        {
            TimesheetBill timesheetBill = _db.TimesheetBills.Where(f => f.TimesheetBillId == timesheetBillId).FirstOrDefault();
            if (timesheetBill != null)
            {
                timesheetBill.IsDeleted = true;
                timesheetBill.ModifiedDate = DateTime.Now;
                _db.Update(timesheetBill);
                _db.SaveChanges();
            }
        }

        public bool IsTimeSheetFinalized(int year, int month, int half, int physicianId)
        {
            DateTime startDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
            }

            Timesheet timesheet = _db.Timesheets.Where(t => t.PhysicianId == physicianId && t.StartDate == startDate && t.IsDeleted != true).FirstOrDefault();
            bool isExists;
            if (timesheet != null)
            {
                isExists = (bool)timesheet.IsFinalize;
            }
            else
            {
                isExists = false;
            }
            return isExists;

        }

        public bool FinalizeTimeSheet(int timeSheetId)
        {
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == timeSheetId);
            if (timesheet != null)
            {
                timesheet.IsFinalize = true;
                timesheet.FinalizedDate = DateTime.Now;
                timesheet.ModifiedDate = DateTime.Now;
                _db.Update(timesheet);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExistsTimesheetBillByDetailId(int timeSheetDetailId)
        {
            TimesheetBill timesheetBill = _db.TimesheetBills.Where(t => t.IsDeleted != true && t.TimesheetDetailId == timeSheetDetailId).FirstOrDefault();
            if (timesheetBill != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateTimeSheetBill(TimesheetBill model)
        {
            TimesheetBill timesheetBill = _db.TimesheetBills.Where(t => t.IsDeleted != true && t.TimesheetDetailId == model.TimesheetDetailId).FirstOrDefault();
            timesheetBill.Item = model.Item;
            timesheetBill.Amount = model.Amount;
            timesheetBill.FilePath = model.FilePath;
            timesheetBill.ModifiedDate = DateTime.Now;
            _db.Update(timesheetBill);
        }

        public InvoicingViewAll GetAdminTimesheetData(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }

            InvoicingViewAll? result;
            IEnumerable<ReceiptView>? receipts;
            IEnumerable<InvoicingView>? invoices;

            Timesheet timesheet = _db.Timesheets.Include(i => i.Physician).Where(t => t.PhysicianId == physicianId && t.StartDate == startDate).FirstOrDefault();
            Physician phy = _db.Physicians.Where(p => p.PhysicianId == physicianId).FirstOrDefault();
            if (timesheet == null)
            {
                result = new InvoicingViewAll
                {
                    IsFinalized = false,
                    phyName = $"{phy.FirstName} {phy.LastName}",
                    Month = month,
                    Half = half,
                    selectedPhyId = phy.PhysicianId,
                };
                return result;
            }
            else if (timesheet.IsFinalize == false)
            {
                result = new InvoicingViewAll
                {
                    TimeSheetId = timesheet.TimesheetId,
                    IsFinalized = false,
                    phyName = $"{timesheet.Physician.FirstName} {timesheet.Physician.LastName}",
                    Month = month,
                    Half = half,
                    selectedPhyId = phy.PhysicianId,
                };
                return result;
            }
            else if (timesheet.IsApproved == false && timesheet.IsFinalize == true)
            {
                result = (from t in _db.Timesheets
                          where t.PhysicianId == physicianId && t.StartDate == startDate && t.IsFinalize == true && t.IsDeleted != true
                          select new InvoicingViewAll
                          {
                              TimeSheetId = t.TimesheetId,
                              selectedPhyId = phy.PhysicianId,
                              StartDate = startDate.ToUniversalTime(),
                              EndDate = endDate.ToUniversalTime(),
                              Status = "Pending",
                              Month = month,
                              Half = half,
                              IsFinalized = true,
                          }).ToList().FirstOrDefault();

                return result;
            }
            else if (timesheet.IsApproved == true && timesheet.IsFinalize == true)
            {
                receipts = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
                           join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
                           orderby tb.TimesheetDetail.Date
                           where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId && tb.IsDeleted != true
                           select new ReceiptView
                           {
                               TimeSheetBillId = tb.TimesheetBillId,
                               TimeSheetDetailId = tb.TimesheetDetailId,
                               physicianId = physicianId,
                               Date = tb.TimesheetDetail.Date,
                               Item = tb.Item,
                               Amount = tb.Amount,
                               FileName = tb.FilePath,
                           };

                invoices = from td in _db.TimesheetDetails.Include(i => i.Timesheet)
                           orderby td.Date
                           where td.Date <= endDate && td.Date >= startDate && td.Timesheet.PhysicianId == physicianId
                           select new InvoicingView
                           {
                               TimeSheetId = td.Timesheet.TimesheetId,
                               TimeSheetDetailId = td.TimesheetDetailId,
                               physicianId = physicianId,
                               Date = td.Date,
                               onCallHours = td.OnCallHours,
                               totalHours = td.TotalHours,
                               isWeekend = td.IsHoliday ?? false,
                               noOfHouseCalls = td.NoOfHouseCall,
                               noOfPhoneConsults = td.NoOfPhoneConsult,
                           };

                Payrate payrate = _db.Payrates.FirstOrDefault(p => p.Physicinaid == physicianId);

                result = new InvoicingViewAll
                {
                    TimeSheetId = timesheet.TimesheetId,
                    selectedPhyId = phy.PhysicianId,
                    Receipt = receipts,
                    Invoicing = invoices,
                    Month = month,
                    Half = half,
                    IsFinalized = true,
                    IsApproved = true,
                    PayTotalHours = payrate.Shift,
                    PayWeekend = payrate.Nightshift,
                    PayHouseCalls = payrate.Housecall,
                    PayPhoneConcults = payrate.Consult,
                };

                return result;
            }
            else
            {
                result = new InvoicingViewAll
                {
                    Month = month,
                    Half = half,
                    selectedPhyId = phy.PhysicianId,
                };
                return result;
            }

        }

        public PayrateView GetPayrateData(int physicianId)
        {
            bool isExists = _db.Payrates.Any(p => p.Physicinaid == physicianId);
            if (isExists)
            {
                PayrateView? result = (from p in _db.Payrates
                                       where p.Physicinaid == physicianId
                                       select new PayrateView
                                       {
                                           PayrateId = p.Payrateid,
                                           PhysicianId = p.Physicinaid,
                                           NightShiftWeekend = p.Nightshift,
                                           Shift = p.Shift,
                                           HouseCallNightWeekend = p.Nighthousecall,
                                           PhoneConsult = p.Consult,
                                           PhoneConsultNightWeekend = p.Nightconsult,
                                           BatchTesting = p.Batchtesting,
                                           HouseCalls = p.Housecall
                                       }).ToList().FirstOrDefault();
                return result;
            }
            else
            {
                PayrateView? result = new PayrateView
                {
                    NightShiftWeekend = 0,
                    PhysicianId = physicianId,
                    Shift = 0,
                    HouseCallNightWeekend = 0,
                    PhoneConsult = 0,
                    PhoneConsultNightWeekend = 0,
                    BatchTesting = 0,
                    HouseCalls = 0,
                    isExists = false,
                };
                return result;
            }
        }

        public void UpdatePayrateData(int phyId, int payrateValue, string payrateType)
        {
            bool isPayrate = _db.Payrates.Any(p => p.Physicinaid == phyId);

            if (isPayrate)
            {
                Payrate payrate = _db.Payrates.FirstOrDefault(p => p.Physicinaid == phyId);
                switch (payrateType)
                {
                    case "NightShiftWeekend":
                        {
                            payrate.Nightshift = payrateValue;
                            break;
                        }

                    case "Shift":
                        {
                            payrate.Shift = payrateValue;
                            break;
                        }

                    case "HouseCallNightWeekend":
                        {
                            payrate.Nighthousecall = payrateValue;
                            break;
                        }

                    case "PhoneConsult":
                        {
                            payrate.Consult = payrateValue;
                            break;
                        }

                    case "PhoneConsultNightWeekend":
                        {
                            payrate.Nightconsult = payrateValue;
                            break;
                        }

                    case "BatchTesting":
                        {
                            payrate.Batchtesting = payrateValue;
                            break;
                        }

                    case "HouseCalls":
                        {
                            payrate.Housecall = payrateValue;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                _db.Update(payrate);
                _db.SaveChanges();
            }
            else
            {
                Payrate payrate = new Payrate();
                payrate.Physicinaid = phyId;
                switch (payrateType)
                {
                    case "NightShiftWeekend":
                        {
                            payrate.Nightshift = payrateValue;
                            break;
                        }

                    case "Shift":
                        {
                            payrate.Shift = payrateValue;
                            break;
                        }

                    case "HouseCallNightWeekend":
                        {
                            payrate.Nighthousecall = payrateValue;
                            break;
                        }

                    case "PhoneConsult":
                        {
                            payrate.Consult = payrateValue;
                            break;
                        }

                    case "PhoneConsultNightWeekend":
                        {
                            payrate.Nightconsult = payrateValue;
                            break;
                        }

                    case "BatchTesting":
                        {
                            payrate.Batchtesting = payrateValue;
                            break;
                        }

                    case "HouseCalls":
                        {
                            payrate.Housecall = payrateValue;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                _db.Add(payrate);
                _db.SaveChanges();
            }
        }

        public InvoicingViewAll GetAdminApproveSheetData(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }

            InvoicingViewAll? result;
            IEnumerable<ReceiptView>? receipts;
            IEnumerable<InvoicingView>? invoices;

            Timesheet timesheet = _db.Timesheets.Include(i => i.Physician).Where(t => t.PhysicianId == physicianId && t.StartDate == startDate).FirstOrDefault();
            Physician phy = _db.Physicians.Where(p => p.PhysicianId == physicianId).FirstOrDefault();

            receipts = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
                       join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
                       orderby tb.TimesheetDetail.Date
                       where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId && tb.IsDeleted != true
                       select new ReceiptView
                       {
                           TimeSheetBillId = tb.TimesheetBillId,
                           TimeSheetDetailId = tb.TimesheetDetailId,
                           physicianId = physicianId,
                           Date = tb.TimesheetDetail.Date,
                           Item = tb.Item,
                           Amount = tb.Amount,
                           FileName = tb.FilePath,
                       };

            invoices = from td in _db.TimesheetDetails.Include(i => i.Timesheet)
                       orderby td.Date
                       where td.Date <= endDate && td.Date >= startDate && td.Timesheet.PhysicianId == physicianId
                       select new InvoicingView
                       {
                           TimeSheetId = td.Timesheet.TimesheetId,
                           TimeSheetDetailId = td.TimesheetDetailId,
                           physicianId = physicianId,
                           Date = td.Date,
                           onCallHours = td.OnCallHours,
                           totalHours = td.TotalHours,
                           isWeekend = td.IsHoliday ?? false,
                           noOfHouseCalls = td.NoOfHouseCall,
                           noOfPhoneConsults = td.NoOfPhoneConsult,
                       };

            bool isPayrate = _db.Payrates.Any(p => p.Physicinaid == physicianId);

            if (!isPayrate)
            {
                Payrate newPayrate = new Payrate
                {
                    Physicinaid = physicianId,
                    Nightshift = 50,
                    Shift = 50,
                    Housecall = 50,
                    Nighthousecall = 50,
                    Consult = 50,
                    Nightconsult = 50,
                    Batchtesting = 50,
                    Modifieddate = DateTime.Now,
                };
                _db.Add(newPayrate);
                _db.SaveChanges();
            }

            Payrate payrate = _db.Payrates.FirstOrDefault(p => p.Physicinaid == physicianId);

            result = new InvoicingViewAll
            {
                TimeSheetId = timesheet.TimesheetId,
                selectedPhyId = phy.PhysicianId,
                Receipt = receipts,
                Invoicing = invoices,
                Month = month,
                Half = half,
                IsFinalized = true,
                IsApproved = true,
                PayTotalHours = payrate.Shift,
                PayWeekend = payrate.Nightshift,
                PayHouseCalls = payrate.Housecall,
                PayPhoneConcults = payrate.Consult,
                SumTotalHours = invoices.Sum(s => s.totalHours) * payrate.Shift,
                SumWeekend = invoices.Count(s => s.isWeekend == true) * payrate.Nightshift,
                SumHouseCalls = invoices.Sum(s => s.noOfHouseCalls) * payrate.Housecall,
                SumPhoneConcults = invoices.Sum(s => s.noOfPhoneConsults) * payrate.Consult,
            };
            result.InvoiceTotal = result.SumTotalHours + result.SumWeekend + result.SumHouseCalls + result.SumPhoneConcults;

            return result;

        }

        public Timesheet GetTimesheet(int year, int month, int half, int physicianId)
        {
            DateTime startDate;
            DateTime endDate;

            if (half == 1)
            {
                startDate = new DateTime(year, month, 1);
                endDate = new DateTime(year, month, 14);
            }
            else
            {
                startDate = new DateTime(year, month, 15);
                endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.StartDate == startDate && t.PhysicianId == physicianId);
            if (timesheet != null)
            {
                return timesheet;
            }
            else
            {
                Timesheet timesheet1 = new Timesheet
                {
                    TimesheetId = 0
                };
                return timesheet1;
            }
        }

        public bool ApproveTimeSheet(InvoicingViewAll model)
        {
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == model.TimeSheetId);

            if (timesheet != null)
            {
                timesheet.IsApproved = true;
                timesheet.ApprovedDate = DateTime.Now;
                timesheet.InvoiceTotal = model.InvoiceTotalSubmit;
                timesheet.Bonus = model.Bonus;
                timesheet.ModifiedDate = DateTime.Now;
                timesheet.AdminDescription = model.AdminDescription;
                timesheet.ApprovedBy = model.ApprovedBy;
                _db.Update(timesheet);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }


        //private readonly ApplicationDbContext _db;
        //public ProviderSiteRepository(ApplicationDbContext db)
        //{
        //    _db = db;
        //}
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        //public IEnumerable<InvoicingView> GetMonthHalfTimeSheetData(int year, int month, int half, int physicianId)
        //{
        //    DateTime startDate;
        //    DateTime endDate;

        //    if (half == 1)
        //    {
        //        startDate = new DateTime(year, month, 1);
        //        endDate = new DateTime(year, month, 14);
        //    }
        //    else
        //    {
        //        startDate = new DateTime(year, month, 15);
        //        endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        //    }
        //    //if (month == DateTime.Now.Month)
        //    //{
        //    //    endDate = new DateTime(year, month, DateTime.Now.Day);
        //    //}

        //    IEnumerable<InvoicingView> result;

        //    bool isSheet = _db.Timesheets.Any(a => a.StartDate == startDate && a.PhysicianId == physicianId);
        //    if (isSheet)
        //    {
        //        result = from td in _db.TimesheetDetails.Include(i => i.Timesheet)
        //                 orderby td.Date
        //                 where td.Date <= endDate && td.Date >= startDate && td.Timesheet.PhysicianId == physicianId
        //                 select new InvoicingView
        //                 {
        //                     TimeSheetId = td.Timesheet.TimesheetId,
        //                     TimeSheetDetailId = td.TimesheetDetailId,
        //                     physicianId = physicianId,
        //                     Date = td.Date,
        //                     onCallHours = td.OnCallHours,
        //                     totalHours = td.TotalHours,
        //                     isWeekend = td.IsHoliday ?? false,
        //                     noOfHouseCalls = td.NoOfHouseCall,
        //                     noOfPhoneConsults = td.NoOfPhoneConsult,
        //                 };
        //    }
        //    else
        //    {
        //        var allDatesInRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
        //                             .Select(offset => startDate.AddDays(offset));

        //        result = from date in allDatesInRange
        //                 join sd in _db.ShiftDetails.Include(i => i.Shift)
        //                 on date equals DateTime.Parse(sd.ShiftDate.ToString()) into shiftGroup
        //                 from shift in shiftGroup.DefaultIfEmpty()
        //                 group shift by date into g
        //                 select new InvoicingView
        //                 {
        //                     Date = g.Key,
        //                     onCallHours = Convert.ToInt32(Math.Round(g.Where(x => x != null && x.Shift.PhysicianId == physicianId)
        //                                                                 .Sum(x => (x.EndTime - x.StartTime).TotalHours))),
        //                     totalHours = Convert.ToInt32(Math.Round(g.Where(x => x != null && x.Shift.PhysicianId == physicianId)
        //                                                                 .Sum(x => (x.EndTime - x.StartTime).TotalHours))),
        //                     physicianId = physicianId,
        //                     isWeekend = false,
        //                 };

        //        //Timesheet timesheet = new Timesheet
        //        //{
        //        //    PhysicianId = physicianId,
        //        //    StartDate = startDate,
        //        //    IsFinalize = false,
        //        //    IsApproved = false,
        //        //    CreatedDate = DateTime.Now,
        //        //    IsDeleted = false,
        //        //};
        //        //_db.Add(timesheet);
        //        //_db.SaveChanges();

        //        //foreach (var item in allDatesInRange)
        //        //{


        //        //    TimesheetDetail timesheetDetail = new TimesheetDetail
        //        //    {
        //        //        TimesheetId = timesheet.TimesheetId,
        //        //        Date = item.Date,

        //        //    };
        //        //}

        //    }

        //    return result;
        //}

        //public IEnumerable<ReceiptView> GetMonthHalfReceiptData(int year, int month, int half, int physicianId)
        //{
        //    DateTime startDate;
        //    DateTime endDate;

        //    if (half == 1)
        //    {
        //        startDate = new DateTime(year, month, 1);
        //        endDate = new DateTime(year, month, 14);
        //    }
        //    else
        //    {
        //        startDate = new DateTime(year, month, 15);
        //        endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        //    }

        //    IEnumerable<ReceiptView> result;

        //    //bool isSheet = _db.TimesheetBills.Include(i => i.TimesheetDetail).Any(a => a.TimesheetDetail.Date == startDate && a.TimesheetDetail.Timesheet.PhysicianId == physicianId);
        //    //if (isSheet)
        //    //{
        //    //    result = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
        //    //             join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
        //    //             orderby tb.TimesheetDetail.Date
        //    //             where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId
        //    //             select new ReceiptView
        //    //             {
        //    //                 TimeSheetBillId = tb.TimesheetBillId,
        //    //                 TimeSheetDetailId = tb.TimesheetDetailId,
        //    //                 physicianId = physicianId,
        //    //                 Date = tb.TimesheetDetail.Date,
        //    //                 Item = tb.Item,
        //    //                 Amount = tb.Amount,
        //    //                 FileName = tb.FilePath,
        //    //             };
        //    //}
        //    //else
        //    //{
        //    //}
        //    var allDatesInRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
        //                         .Select(offset => startDate.AddDays(offset));

        //    //result = from date in allDatesInRange
        //    //         join td in _db.TimesheetDetails
        //    //         on date equals td.Date into shiftGroup
        //    //         from td in shiftGroup.DefaultIfEmpty()
        //    //         join tb in _db.TimesheetBills on td.TimesheetDetailId equals tb.TimesheetDetailId into detailGroup
        //    //         from tb in detailGroup.DefaultIfEmpty()
        //    //         where (tb == null || tb.IsDeleted != true)
        //    //         select new ReceiptView
        //    //         {
        //    //             Date = date.Date,
        //    //             TimeSheetDetailId = td != null ? td.TimesheetDetailId : 0,
        //    //             physicianId = physicianId,
        //    //             TimeSheetBillId = tb != null ? tb.TimesheetBillId : 0,
        //    //             Item = tb != null ? tb.Item : "",
        //    //             Amount = tb != null ? tb.Amount : null,
        //    //             FileName = tb != null ? tb.FilePath : null,
        //    //         };

        //    result = from date in allDatesInRange
        //             join td in _db.TimesheetDetails.Where(d => d.Date >= startDate && d.Date <= endDate)
        //             on date equals td.Date into shiftGroup
        //             from td in shiftGroup.DefaultIfEmpty()
        //             join tb in _db.TimesheetBills.Include(i => i.TimesheetDetail).Where(b => b.IsDeleted != true && (b.TimesheetDetail.Date >= startDate && b.TimesheetDetail.Date <= endDate))
        //             on td?.TimesheetDetailId equals tb.TimesheetDetailId into detailGroup
        //             from tb in detailGroup.DefaultIfEmpty()
        //             select new ReceiptView
        //             {
        //                 Date = date.Date,
        //                 TimeSheetDetailId = td != null ? td.TimesheetDetailId : 0,
        //                 physicianId = physicianId,
        //                 TimeSheetBillId = tb != null ? tb.TimesheetBillId : 0,
        //                 Item = tb != null ? tb.Item : "",
        //                 Amount = tb != null ? tb.Amount : null,
        //                 FileName = tb != null ? tb.FilePath : null,
        //             };


        //    return result;
        //}

        //public IEnumerable<ReceiptView> GetMonthHalfTimesheetBillData(int year, int month, int half, int physicianId)
        //{
        //    DateTime startDate;
        //    DateTime endDate;

        //    if (half == 1)
        //    {
        //        startDate = new DateTime(year, month, 1);
        //        endDate = new DateTime(year, month, 14);
        //    }
        //    else
        //    {
        //        startDate = new DateTime(year, month, 15);
        //        endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        //    }

        //    IEnumerable<ReceiptView> result;


        //    result = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
        //             join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
        //             orderby tb.TimesheetDetail.Date
        //             where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId && tb.IsDeleted != true
        //             select new ReceiptView
        //             {
        //                 TimeSheetBillId = tb.TimesheetBillId,
        //                 TimeSheetDetailId = tb.TimesheetDetailId,
        //                 physicianId = physicianId,
        //                 Date = tb.TimesheetDetail.Date,
        //                 Item = tb.Item,
        //                 Amount = tb.Amount,
        //                 FileName = tb.FilePath,
        //             };

        //    return result;
        //}

        //public void AddUpdateTimesheet(IEnumerable<InvoicingView> model)
        //{
        //    bool isSheet = _db.Timesheets.Any(a => a.StartDate == model.ToList()[0].Date && a.PhysicianId == model.ToList()[0].physicianId);

        //    if (!isSheet)
        //    {
        //        Timesheet timesheet = new Timesheet
        //        {
        //            PhysicianId = model.ToList()[0].physicianId,
        //            StartDate = model.ToList()[0].Date,
        //            IsFinalize = false,
        //            IsApproved = false,
        //            CreatedDate = DateTime.Now,
        //            IsDeleted = false,
        //        };
        //        _db.Add(timesheet);
        //        _db.SaveChanges();

        //        foreach (var item in model.OrderBy(o => o.Date))
        //        {
        //            TimesheetDetail timesheetDetail = new TimesheetDetail
        //            {
        //                TimesheetId = timesheet.TimesheetId,
        //                Date = item.Date,
        //                OnCallHours = item.onCallHours,
        //                TotalHours = item.totalHours,
        //                IsHoliday = item.isWeekend != null ? item.isWeekend : false,
        //                NoOfHouseCall = item.noOfHouseCalls,
        //                NoOfPhoneConsult = item.noOfPhoneConsults,
        //                CreatedDate = DateTime.Now,
        //            };
        //            _db.Add(timesheetDetail);
        //        }
        //        _db.SaveChanges();
        //    }
        //    else
        //    {
        //        Timesheet timesheet = _db.Timesheets.FirstOrDefault(f => f.PhysicianId == model.ToList()[0].physicianId && f.StartDate == model.ToList()[0].Date);
        //        foreach (var item in model.OrderBy(o => o.Date))
        //        {
        //            TimesheetDetail timesheetDetail = _db.TimesheetDetails.FirstOrDefault(f => f.TimesheetDetailId == item.TimeSheetDetailId);
        //            timesheetDetail.TotalHours = item.totalHours != null ? item.totalHours : null;
        //            timesheetDetail.IsHoliday = item.isWeekend != null ? item.isWeekend : false;
        //            timesheetDetail.NoOfHouseCall = item.noOfHouseCalls != null ? item.noOfHouseCalls : null;
        //            timesheetDetail.NoOfPhoneConsult = item.noOfPhoneConsults != null ? item.noOfPhoneConsults : null;
        //            _db.Update(timesheetDetail);
        //        }
        //        _db.SaveChanges();
        //    }
        //}

        //public void AddTimesheetBill(TimesheetBill model)
        //{
        //    _db.Add(model);
        //}

        //public void DeleteTimesheetBill(int timesheetBillId)
        //{
        //    TimesheetBill timesheetBill = _db.TimesheetBills.Where(f => f.TimesheetBillId == timesheetBillId).FirstOrDefault();
        //    if (timesheetBill != null)
        //    {
        //        timesheetBill.IsDeleted = true;
        //        timesheetBill.ModifiedDate = DateTime.Now;
        //        _db.Update(timesheetBill);
        //        _db.SaveChanges();
        //    }
        //}

        //public bool IsTimeSheetFinalized(int year, int month, int half, int physicianId)
        //{
        //    DateTime startDate;

        //    if (half == 1)
        //    {
        //        startDate = new DateTime(year, month, 1);
        //    }
        //    else
        //    {
        //        startDate = new DateTime(year, month, 15);
        //    }

        //    Timesheet timesheet = _db.Timesheets.Where(t => t.PhysicianId == physicianId && t.StartDate == startDate && t.IsDeleted != true).FirstOrDefault();
        //    bool isExists;
        //    if (timesheet != null)
        //    {
        //        isExists = (bool)timesheet.IsFinalize;
        //    }
        //    else
        //    {
        //        isExists = false;
        //    }
        //    return isExists;

        //}

        //public bool FinalizeTimeSheet(int timeSheetId)
        //{
        //    Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == timeSheetId);
        //    if (timesheet != null)
        //    {
        //        timesheet.IsFinalize = true;
        //        timesheet.FinalizedDate = DateTime.Now;
        //        timesheet.ModifiedDate = DateTime.Now;
        //        _db.Update(timesheet);
        //        _db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool IsExistsTimesheetBillByDetailId(int timeSheetDetailId)
        //{
        //    TimesheetBill timesheetBill = _db.TimesheetBills.Where(t => t.IsDeleted != true && t.TimesheetDetailId == timeSheetDetailId).FirstOrDefault();
        //    if (timesheetBill != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public void UpdateTimeSheetBill(TimesheetBill model)
        //{
        //    TimesheetBill timesheetBill = _db.TimesheetBills.Where(t => t.IsDeleted != true && t.TimesheetDetailId == model.TimesheetDetailId).FirstOrDefault();
        //    timesheetBill.Item = model.Item;
        //    timesheetBill.Amount = model.Amount;
        //    timesheetBill.FilePath = model.FilePath;
        //    timesheetBill.ModifiedDate = DateTime.Now;
        //    _db.Update(timesheetBill);
        //}

        //public InvoicingViewAll GetAdminTimesheetData(int year, int month, int half, int physicianId)
        //{
        //    DateTime startDate;
        //    DateTime endDate;

        //    if (half == 1)
        //    {
        //        startDate = new DateTime(year, month, 1);
        //        endDate = new DateTime(year, month, 14);
        //    }
        //    else
        //    {
        //        startDate = new DateTime(year, month, 15);
        //        endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        //    }

        //    InvoicingViewAll? result;
        //    IEnumerable<ReceiptView>? receipts;
        //    IEnumerable<InvoicingView>? invoices;

        //    Timesheet timesheet = _db.Timesheets.Include(i => i.Physician).Where(t => t.PhysicianId == physicianId && t.StartDate == startDate).FirstOrDefault();
        //    Physician phy = _db.Physicians.Where(p => p.PhysicianId == physicianId).FirstOrDefault();
        //    if (timesheet == null)
        //    {
        //        result = new InvoicingViewAll
        //        {
        //            IsFinalized = false,
        //            phyName = $"{phy.FirstName} {phy.LastName}",
        //            Month = month,
        //            Half = half,
        //            selectedPhyId = phy.PhysicianId,
        //        };
        //        return result;
        //    }
        //    else if (timesheet.IsFinalize == false)
        //    {
        //        result = new InvoicingViewAll
        //        {
        //            TimeSheetId = timesheet.TimesheetId,
        //            IsFinalized = false,
        //            phyName = $"{timesheet.Physician.FirstName} {timesheet.Physician.LastName}",
        //            Month = month,
        //            Half = half,
        //            selectedPhyId = phy.PhysicianId,
        //        };
        //        return result;
        //    }
        //    else if (timesheet.IsApproved == false && timesheet.IsFinalize == true)
        //    {
        //        result = (from t in _db.Timesheets
        //                  where t.PhysicianId == physicianId && t.StartDate == startDate && t.IsFinalize == true && t.IsDeleted != true
        //                  select new InvoicingViewAll
        //                  {
        //                      TimeSheetId = t.TimesheetId,
        //                      selectedPhyId = phy.PhysicianId,
        //                      StartDate = startDate.ToUniversalTime(),
        //                      EndDate = endDate.ToUniversalTime(),
        //                      Status = "Pending",
        //                      Month = month,
        //                      Half = half,
        //                      IsFinalized = true,
        //                  }).ToList().FirstOrDefault();

        //        return result;
        //    }
        //    else if (timesheet.IsApproved == true && timesheet.IsFinalize == true)
        //    {
        //        receipts = from tb in _db.TimesheetBills.Include(i => i.TimesheetDetail)
        //                   join td in _db.TimesheetDetails on tb.TimesheetDetailId equals td.TimesheetDetailId
        //                   orderby tb.TimesheetDetail.Date
        //                   where tb.TimesheetDetail.Date <= endDate && tb.TimesheetDetail.Date >= startDate && tb.TimesheetDetail.Timesheet.PhysicianId == physicianId && tb.IsDeleted != true
        //                   select new ReceiptView
        //                   {
        //                       TimeSheetBillId = tb.TimesheetBillId,
        //                       TimeSheetDetailId = tb.TimesheetDetailId,
        //                       physicianId = physicianId,
        //                       Date = tb.TimesheetDetail.Date,
        //                       Item = tb.Item,
        //                       Amount = tb.Amount,
        //                       FileName = tb.FilePath,
        //                   };

        //        invoices = from td in _db.TimesheetDetails.Include(i => i.Timesheet)
        //                   orderby td.Date
        //                   where td.Date <= endDate && td.Date >= startDate && td.Timesheet.PhysicianId == physicianId
        //                   select new InvoicingView
        //                   {
        //                       TimeSheetId = td.Timesheet.TimesheetId,
        //                       TimeSheetDetailId = td.TimesheetDetailId,
        //                       physicianId = physicianId,
        //                       Date = td.Date,
        //                       onCallHours = td.OnCallHours,
        //                       totalHours = td.TotalHours,
        //                       isWeekend = td.IsHoliday ?? false,
        //                       noOfHouseCalls = td.NoOfHouseCall,
        //                       noOfPhoneConsults = td.NoOfPhoneConsult,
        //                   };

        //        result = new InvoicingViewAll
        //        {
        //            TimeSheetId = timesheet.TimesheetId,
        //            selectedPhyId = phy.PhysicianId,
        //            Receipt = receipts,
        //            Invoicing = invoices,
        //            Month = month,
        //            Half = half,
        //            IsFinalized = true,
        //            IsApproved = true,
        //        };

        //        return result;
        //    }
        //    else
        //    {
        //        result = new InvoicingViewAll
        //        {
        //            Month = month,
        //            Half = half,
        //            selectedPhyId = phy.PhysicianId,
        //        };
        //        return result;
        //    }

        //}

    }
}
