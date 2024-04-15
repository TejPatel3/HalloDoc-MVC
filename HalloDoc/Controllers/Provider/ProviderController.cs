using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers.Provider
{
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IunitOfWork _unitOfWork;
        public ProviderController(ApplicationDbContext context, IunitOfWork unit)
        {
            _context = context;
            _unitOfWork = unit;
        }
        public IActionResult Provider()
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;

            var physiciannotificatin = _context.PhysicianNotifications.ToList();
            var model = new ProviderDetailsViewModel();
            model.physician = _context.Physicians.Where(m => m.IsDeleted != bitset).ToList();
            model.isnotificationstopped = physiciannotificatin;
            model.regionlist = _unitOfWork.tableData.GetRegionList();
            return View(model);
        }
        public IActionResult GetProviderTable(int regionid)
        {

            var model = new ProviderDetailsViewModel();
            if (regionid == 0)
            {
                model.physician = _unitOfWork.tableData.GetPhysicianList().ToList();
            }
            else
            {
                model.physician = _unitOfWork.tableData.GetPhysicianList().Where(m => m.RegionId == regionid).ToList();
            }
            model.isnotificationstopped = _unitOfWork.tableData.GetPhysicianNotificationList();
            return PartialView("_ProviderTable", model);
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
                IsAgreementDoc = physician.IsAgreementDoc,
                IsCredentialDoc = physician.IsCredentialDoc,
                IsBackgroundDoc = physician.IsBackgroundDoc,
                IsLicenseDoc = physician.IsLicenseDoc,
                IsNonDisclosureDoc = physician.IsNonDisclosureDoc,
                selectedroleid = (int)physician.RoleId,
                status = (short)physician.Status,
            };
            model.rolelist = _context.Roles.Where(m => m.AccountType == 2 || m.AccountType == 0).ToList();
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
                physician.RoleId = obj.selectedroleid;
                physician.Status = obj.status;
                _context.Update(physician);
                _context.SaveChanges();
            }
            if (obj.firstname != null)
            {


                var RegionToDelete = physicianRegion.Except(obj.selectedregion);
                foreach (var item in RegionToDelete)
                {
                    PhysicianRegion physicianRegionToDelete = _context.PhysicianRegions.FirstOrDefault(ar => ar.PhysicianId == obj.physicianid && ar.RegionId == item);

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
            if (obj.adminnote != null)
            {
                physician.BusinessWebsite = obj.businesswebsite;
                physician.BusinessName = obj.businessname;
                physician.AdminNotes = obj.adminnote;
                _context.Physicians.Update(physician);
            };
            if (obj.address1 != null)
            {
                physician.Address1 = obj.address1;
                physician.Address2 = obj.address2;
                physician.City = obj.city;
                physician.Zip = obj.zip;
                physician.AltPhone = obj.alterphonenumber;
                _context.Physicians.Update(physician);
            }

            _context.SaveChanges();
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

        public IActionResult CreateProviderAccount()
        {
            var regionlist = _context.Regions.ToList();
            ProviderDetailsViewModel model = new ProviderDetailsViewModel
            {
                regionlist = regionlist,
                rolelist = _context.Roles.ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateProviderAccount(ProviderDetailsViewModel obj, int[] selectedregion)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _context.Admins.FirstOrDefault(m => m.AdminId == adminid);

            if (adminid != 0)
            {
                Guid id = Guid.NewGuid();

                var aspnetuser = new AspNetUser
                {
                    Id = id.ToString(),
                    UserName = obj.username,
                    Email = obj.email,
                    PasswordHash = obj.password,
                    PhoneNumber = obj.phonenumber,
                    CreatedDate = DateTime.Now,
                };
                _context.AspNetUsers.Add(aspnetuser);
                _context.SaveChanges();

                var aspnetuserroles = new AspNetUserRole
                {
                    RoleId = "2",
                    UserId = id.ToString()
                };

                _context.AspNetUserRoles.Add(aspnetuserroles);
                _context.SaveChanges();

                var physician = new Physician
                {
                    Id = aspnetuser.Id,
                    FirstName = obj.firstname,
                    LastName = obj.lastname,
                    Email = obj.email,
                    Mobile = obj.phonenumber,
                    MedicalLicense = obj.medicallicencenumber,
                    AdminNotes = obj.adminnote,
                    Address1 = obj.address1,
                    RegionId = 1,
                    RoleId = obj.selectedroleid,
                    Address2 = obj.address2,
                    City = obj.city,
                    Zip = obj.zip,
                    AltPhone = obj.alterphonenumber,
                    CreatedBy = admin.AspNetUserId,
                    CreatedDate = DateTime.Now,
                    Npinumber = obj.npinumber,
                    Photo = obj.photo,
                    Status = 1,
                    BusinessName = obj.businessname,
                    BusinessWebsite = obj.businesswebsite,
                    IsAgreementDoc = new BitArray(new[] { false }),
                    IsBackgroundDoc = new BitArray(new[] { false }),
                    IsCredentialDoc = new BitArray(new[] { false }),
                    IsNonDisclosureDoc = new BitArray(new[] { false }),
                    IsLicenseDoc = new BitArray(new[] { false }),
                    IsDeleted = new BitArray(new[] { false }),
                };
                _context.Physicians.Add(physician);
                _context.SaveChanges();
                if (obj.AgreementDoc != null)
                {
                    uploadFile(obj.AgreementDoc, physician.PhysicianId, "IndependentContractorAgreement");
                    physician.IsAgreementDoc = new BitArray(new[] { true });
                }
                if (obj.BackgroundDoc != null)
                {
                    uploadFile(obj.AgreementDoc, physician.PhysicianId, "BackgroundCheck");
                    physician.IsBackgroundDoc = new BitArray(new[] { true });

                }
                if (obj.CredentialDoc != null)
                {
                    uploadFile(obj.AgreementDoc, physician.PhysicianId, "HIPAACompliance");
                    physician.IsCredentialDoc = new BitArray(new[] { true });

                }
                if (obj.NonDisclosureDoc != null)
                {
                    uploadFile(obj.AgreementDoc, physician.PhysicianId, "Non-DisclosureAgreement");
                    physician.IsNonDisclosureDoc = new BitArray(new[] { true });

                }
                if (obj.LicenseDoc != null)
                {
                    uploadFile(obj.AgreementDoc, physician.PhysicianId, "LicenseDocument");
                    physician.IsLicenseDoc = new BitArray(new[] { true });

                }

                PhysicianRegion physicianregion = new PhysicianRegion
                {
                    PhysicianId = physician.PhysicianId,
                };
                foreach (var item in selectedregion)
                {
                    physicianregion.RegionId = item;
                    _context.Add(physicianregion);
                }


                _context.SaveChanges();
                TempData["success"] = "Physician Account Created Successfully..!";

            }
            else
            {
                TempData["error"] = "Something went wrong try again..!";

            }
            return RedirectToAction("Provider");
        }

        public IActionResult uploadFile(IFormFile file, int providerid, string onboardinguploadvalue)
        {
            if (file != null && file.Length > 0)
            {

                string extension = Path.GetExtension(file.FileName);
                string filename = onboardinguploadvalue + extension;

                string folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "onboarding", providerid.ToString());

                if (!Directory.Exists(folderpath))
                    Directory.CreateDirectory(folderpath);

                string uploadFile = Path.Combine(folderpath, filename);

                using (var fileStream = new FileStream(uploadFile, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }



                Physician physician = _context.Physicians.FirstOrDefault(x => x.PhysicianId == providerid);


                BitArray bitset = new BitArray(1);

                // Set some bits
                bitset[0] = true; // Set the first bit to 1

                if (onboardinguploadvalue == "IndependentContractorAgreement")
                {
                    physician.IsAgreementDoc = new BitArray(new[] { true });
                }
                else if (onboardinguploadvalue == "BackgroundCheck")
                {
                    physician.IsBackgroundDoc = new BitArray(new[] { true });

                }
                else if (onboardinguploadvalue == "HIPAACompliance")
                {
                    physician.IsCredentialDoc = new BitArray(new[] { true });

                }
                else if (onboardinguploadvalue == "Non-DisclosureAgreement")
                {
                    physician.IsNonDisclosureDoc = new BitArray(new[] { true });

                }
                else if (onboardinguploadvalue == "LicenseDocument")
                {
                    physician.IsLicenseDoc = new BitArray(new[] { true });

                }

                _context.Update(physician);
                _context.SaveChanges();
            }

            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });
        }
        [HttpPost]
        public IActionResult UpdateNotification(int[] provideridlist)
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;
            List<int> physiciannotificationDB = _context.PhysicianNotifications.Select(m => m.PhysicianId).ToList();
            var deletePhysicianNotification = physiciannotificationDB.Except(provideridlist);
            foreach (var item in deletePhysicianNotification)
            {
                PhysicianNotification physicianNotification = _context.PhysicianNotifications.FirstOrDefault(m => m.PhysicianId == item);
                _context.Remove(physicianNotification);
            }
            _context.SaveChanges();
            var addPhysicianNotification = provideridlist.Except(physiciannotificationDB);
            foreach (var item in addPhysicianNotification)
            {
                PhysicianNotification physiciannotification = new PhysicianNotification
                {
                    PhysicianId = item,
                    IsNotificationStopped = bitset,
                };
                _context.Add(physiciannotification);
            };
            _context.SaveChanges();
            return RedirectToAction("Provider");
        }
        public IActionResult DeleteProviderAccount(int physicianid)
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;
            Physician physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == physicianid);
            physician.IsDeleted = bitset;
            _context.Update(physician);
            _context.SaveChanges();
            return RedirectToAction("Provider");

        }
    }
}
