﻿using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers.Provider
{
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProviderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Provider()
        {
            var model = new ProviderDetailsViewModel();
            model.physician = _context.Physicians.ToList();
            return View(model);
        }

        public IActionResult ContactProviderModelSubmit(int physicianid, string message)
        {
            var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == physicianid);
            Sendemail(physician.Email, "Hallodoc admin sent you message", message);
            return RedirectToAction("Provider");
        }
        public async Task Sendemail(string email, string subject, string message)
        {
            try
            {
                //var mail = "tatva.dotnet.tejpatel@outlook.com";
                //var password = "7T6d2P3@K";
                var mail = "pateltej3122002@gmail.com";
                var password = "762397@TEj";
                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
                TempData["success"] = "Document sent in Email..!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        public IActionResult EditProviderAccount(int physicianid)
        {
            var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == physicianid);
            var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Id == physician.Id);
            var regionlist = _context.Regions.ToList();
            var selectedphyregionlist = _context.PhysicianRegions.Where(m => m.PhysicianId == physicianid).ToList();
            var model = new ProviderDetailsViewModel
            {
                username = aspnetuser.UserName,
                password = aspnetuser.PasswordHash,
                firstname = physician.FirstName,
                lastname = physician.LastName,
                email = physician.Email,
                phonenumber = physician.Mobile,
                medicallicencenumber = physician.MedicalLicense,
                npinumber = physician.Npinumber,
                regionlist = regionlist,
                address1 = physician.Address1,
                address2 = physician.Address2,
                city = physician.City,
                zip = physician.Zip,
                alterphonenumber = physician.AltPhone,
                businessname = physician.BusinessName,
                businesswebsite = physician.BusinessWebsite,
                adminnote = physician.AdminNotes,
                photo = physician.Photo,
                signature = physician.Signature,
                physicianid = physicianid,
                selectedregionlist = selectedphyregionlist,
            };
            //var physicianregion = _context.PhysicianRegions.Where(m => m.PhysicianId == physicianid).ToList();
            //if (physicianregion != null)
            //{

            //        var region = _context.PhysicianRegions.FirstOrDefault(m => m.PhysicianId == physicianid).;
            //        model.selectedregionlist.Add(region);

            //}
            return View(model);
        }
        [HttpPost]
        public IActionResult EditProviderAccount(ProviderDetailsViewModel obj)
        {
            var physician = _context.Physicians.Include(m => m.PhysicianRegions).FirstOrDefault(m => m.PhysicianId == obj.physicianid);
            var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Id == physician.Id);
            List<int> physicianRegion = physician.PhysicianRegions.Select(m => m.RegionId).ToList();
            if (obj.username != null)
            {
                aspnetuser.UserName = obj.username;
                _context.AspNetUsers.Update(aspnetuser);
                _context.SaveChanges();
            }
            if (obj.firstname != null)
            {
                var RegionToDelete = physicianRegion.Except(obj.selectedregion);
                foreach (var item in RegionToDelete)
                {
                    PhysicianRegion physicianRegionToDelete = _context.PhysicianRegions
                .FirstOrDefault(ar => ar.PhysicianId == obj.physicianid && ar.RegionId == item);

                    if (physicianRegionToDelete != null)
                    {
                        _context.PhysicianRegions.Remove(physicianRegionToDelete);
                    }
                }
                IEnumerable<int> regionsToAdd = obj.selectedregion.Except(physicianRegion);

                foreach (int item in regionsToAdd)
                {
                    PhysicianRegion newphysicianRegion = new PhysicianRegion
                    {
                        PhysicianId = obj.physicianid,
                        RegionId = item,
                    };
                    _context.PhysicianRegions.Add(newphysicianRegion);
                }
                _context.SaveChanges();

                physician.FirstName = obj.firstname;
                physician.LastName = obj.lastname;
                physician.Email = obj.email;
                physician.Mobile = obj.phonenumber;
                physician.MedicalLicense = obj.medicallicencenumber;
                physician.Npinumber = obj.npinumber;
                _context.Physicians.Update(physician);
                _context.SaveChanges();
            }
            if (obj.password != null)
            {
                aspnetuser.PasswordHash = obj.password;
                _context.AspNetUsers.Update(aspnetuser);
                _context.SaveChanges();
            }
            return RedirectToAction("EditProviderAccount", new { physicianid = obj.physicianid });
        }

        [HttpPost]

        public IActionResult EditProviderPhoto(int providerid, string base64String)
        {
            var physiciandata = _context.Physicians.FirstOrDefault(p => p.PhysicianId == providerid);
            physiciandata.Photo = base64String;

            _context.Physicians.Update(physiciandata);
            _context.SaveChanges();

            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });
        }

        [HttpPost]

        public IActionResult EditProviderSign(int providerid, string base64String)
        {
            var physiciandata = _context.Physicians.FirstOrDefault(p => p.PhysicianId == providerid);
            physiciandata.Signature = base64String;

            _context.Physicians.Update(physiciandata);
            _context.SaveChanges();

            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });

        }

    }
}
