﻿using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;
using Services.ViewModels;

namespace Services.Implementation
{
    public class PayrateRepository : IPayrate
    {
        private readonly ApplicationDbContext _context;
        public PayrateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddUpadtePayrate(PayRateViewModel model)
        {
            var payrate = _context.Payrates.FirstOrDefault(m => m.Physicinaid == model.PhysicianId);
            if (payrate == null)
            {
                var add = new Payrate
                {
                    Physicinaid = model.PhysicianId,
                    Nightshift = 50,
                    Shift = 50,
                    Housecall = 50,
                    Nighthousecall = 50,
                    Consult = 50,
                    Nightconsult = 50,
                    Batchtesting = 50,
                    Modifieddate = DateTime.Now,
                };
                if (model.FieldCheck == "NSWE")
                {
                    add.Nightshift = model.NightShiftWeekEnd;
                }
                else if (model.FieldCheck == "S")
                {
                    add.Shift = model.Shift;
                }
                else if (model.FieldCheck == "HCNWE")
                {
                    add.Nighthousecall = model.HouseCallNightWeekEnd;
                }
                else if (model.FieldCheck == "PC")
                {
                    add.Consult = model.PhoneConsult;
                }
                else if (model.FieldCheck == "PCNWE")
                {
                    add.Nightconsult = model.PhoneConsultNightWeekEnd;
                }
                else if (model.FieldCheck == "BT")
                {
                    add.Batchtesting = model.BatchTesting;
                }
                else if (model.FieldCheck == "HC")
                {
                    add.Housecall = model.HouseCalls;
                }
                _context.Payrates.Add(add);
                _context.SaveChanges();
            }
            else
            {
                if (model.FieldCheck == "NSWE")
                {
                    payrate.Nightshift = model.NightShiftWeekEnd;
                }
                else if (model.FieldCheck == "S")
                {
                    payrate.Shift = model.Shift;
                }
                else if (model.FieldCheck == "HCNWE")
                {
                    payrate.Nighthousecall = model.HouseCallNightWeekEnd;
                }
                else if (model.FieldCheck == "PC")
                {
                    payrate.Consult = model.PhoneConsult;
                }
                else if (model.FieldCheck == "PCNWE")
                {
                    payrate.Nightconsult = model.PhoneConsultNightWeekEnd;
                }
                else if (model.FieldCheck == "BT")
                {
                    payrate.Batchtesting = model.BatchTesting;
                }
                else if (model.FieldCheck == "HC")
                {
                    payrate.Housecall = model.HouseCalls;
                }
                _context.Payrates.Update(payrate);
                _context.SaveChanges();
            }
        }
    }
}
