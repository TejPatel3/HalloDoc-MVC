using DataModels.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Collections;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers.Provider
{
    [AuthorizationRepository("Admin,Physician")]

    public class ProviderController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        public ProviderController(IunitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public IActionResult Provider()
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;
            var physiciannotificatin = _unitOfWork.tableData.GetPhysicianNotificationList();
            var model = new ProviderDetailsViewModel();
            model.physician = _unitOfWork.tableData.GetPhysicianList();
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
            var physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            //Sendemail(physician.Email, "Hallodoc admin sent you message", message);
            _unitOfWork.SendEmailAndSMS.Sendemail(physician.Email, "Hallodoc admin sent you message", message);
            _unitOfWork.SendEmailAndSMS.SendSMS();
            return RedirectToAction("Provider");
        }

        private Task Sendemail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.tejpatel@outlook.com";
            var password = "7T6d2P3@K";
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            MailMessage mailMessage = new MailMessage(from: mail, to: email, subject, message);
            mailMessage.IsBodyHtml = true;

            return client.SendMailAsync(mailMessage);
            //return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));


        }
        //public async Task Sendemail(string email, string subject, string message)
        //{
        //    try
        //    {
        //        var mail = "pateltej3122002@gmail.com";
        //        var password = "762397@TEj";
        //        var client = new SmtpClient("smtp.office365.com", 587)
        //        {
        //            EnableSsl = true,
        //            Credentials = new NetworkCredential(mail, password)
        //        };
        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(mail),
        //            Subject = subject,
        //            Body = message,
        //        };
        //        mailMessage.To.Add(email);
        //        await client.SendMailAsync(mailMessage);
        //        TempData["success"] = "Document sent in Email..!";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error sending email: {ex.Message}");
        //    }
        //}
        public IActionResult EditProviderAccount(int physicianid)
        {
            var physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(physician.Id);
            var regionlist = _unitOfWork.tableData.GetRegionList();
            var selectedphyregionlist = _unitOfWork.tableData.GetPhysicianRegionListByPhysicianId(physicianid);
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
            model.rolelist = _unitOfWork.tableData.GetRoleList().Where(m => m.AccountType == 2 || m.AccountType == 0).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult EditProviderAccount(ProviderDetailsViewModel obj)
        {
            var physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(obj.physicianid);
            var aspnetuser = _unitOfWork.tableData.GetAspNetUserByAspNetUserId(physician.Id);
            List<int> physicianRegion = physician.PhysicianRegions.Select(m => m.RegionId).ToList();
            if (obj.username != null)
            {
                aspnetuser.UserName = obj.username;
                _unitOfWork.UpdateData.UpdateAspNetUser(aspnetuser);
                physician.RoleId = obj.selectedroleid;
                physician.Status = obj.status;
                _unitOfWork.UpdateData.UpdatePhysician(physician);
                _unitOfWork.Add.SaveChangesDB();
            }
            if (obj.firstname != null)
            {
                var RegionToDelete = physicianRegion.Except(obj.selectedregion);
                foreach (var item in RegionToDelete)
                {
                    PhysicianRegion physicianRegionToDelete = _unitOfWork.tableData.GetPhysicianRegionListByPhysicianId(obj.physicianid).FirstOrDefault(ar => ar.RegionId == item);
                    if (physicianRegionToDelete != null)
                    {
                        _unitOfWork.RemoveData.RemovePhysicianRegion(physicianRegionToDelete);
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
                    _unitOfWork.Add.AddPhysicianRegion(newphysicianRegion);
                }
                _unitOfWork.Add.SaveChangesDB();
                physician.FirstName = obj.firstname;
                physician.LastName = obj.lastname;
                physician.Email = obj.email;
                physician.Mobile = obj.phonenumber;
                physician.MedicalLicense = obj.medicallicencenumber;
                physician.Npinumber = obj.npinumber;
                _unitOfWork.UpdateData.UpdatePhysician(physician);
            }
            if (obj.password != null)
            {
                aspnetuser.PasswordHash = obj.password;
                _unitOfWork.UpdateData.UpdateAspNetUser(aspnetuser);
            }
            if (obj.adminnote != null)
            {
                physician.BusinessWebsite = obj.businesswebsite;
                physician.BusinessName = obj.businessname;
                physician.AdminNotes = obj.adminnote;
                _unitOfWork.UpdateData.UpdatePhysician(physician);
            };
            if (obj.address1 != null)
            {
                physician.Address1 = obj.address1;
                physician.Address2 = obj.address2;
                physician.City = obj.city;
                physician.Zip = obj.zip;
                physician.AltPhone = obj.alterphonenumber;
                _unitOfWork.UpdateData.UpdatePhysician(physician);
            }
            return RedirectToAction("EditProviderAccount", new { physicianid = obj.physicianid });
        }

        [HttpPost]

        public IActionResult EditProviderPhoto(int providerid, string base64String)
        {
            var physiciandata = _unitOfWork.tableData.GetPhysicianFirstOrDefault(providerid);
            physiciandata.Photo = base64String;
            _unitOfWork.UpdateData.UpdatePhysician(physiciandata);
            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });
        }

        [HttpPost]

        public IActionResult EditProviderSign(int providerid, string base64String)
        {
            var physiciandata = _unitOfWork.tableData.GetPhysicianFirstOrDefault(providerid);
            physiciandata.Signature = base64String;
            _unitOfWork.UpdateData.UpdatePhysician(physiciandata);
            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });
        }

        public IActionResult CreateProviderAccount()
        {
            var regionlist = _unitOfWork.tableData.GetRegionList();
            ProviderDetailsViewModel model = new ProviderDetailsViewModel
            {
                regionlist = regionlist,
                rolelist = _unitOfWork.tableData.GetRoleList(),
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateProviderAccount(ProviderDetailsViewModel obj, int[] selectedregion)
        {
            var adminid = HttpContext.Session.GetInt32("AdminId");
            var admin = _unitOfWork.tableData.GetAdminByAdminId(adminid);
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
                _unitOfWork.Add.AddAspNetUser(aspnetuser);
                var aspnetuserroles = new AspNetUserRole
                {
                    RoleId = "2",
                    UserId = id.ToString()
                };
                _unitOfWork.Add.AddAspNetUserRole(aspnetuserroles);
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
                _unitOfWork.Add.AddPhysician(physician);
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
                    _unitOfWork.Add.AddPhysicianRegion(physicianregion);
                }
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
                Physician physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(providerid);
                BitArray bitset = new BitArray(1);
                // Set some bits
                bitset[0] = true;
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
                _unitOfWork.UpdateData.UpdatePhysician(physician);
            }
            return RedirectToAction("EditProviderAccount", new { physicianid = providerid });
        }
        [HttpPost]
        public IActionResult UpdateNotification(int[] provideridlist)
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;
            List<int> physiciannotificationDB = _unitOfWork.tableData.GetPhysicianNotificationList().Select(m => m.PhysicianId).ToList();
            var deletePhysicianNotification = physiciannotificationDB.Except(provideridlist);
            foreach (var item in deletePhysicianNotification)
            {
                PhysicianNotification physicianNotification = _unitOfWork.tableData.GetPhysicianNotificationByPhysicianId(item);
                _unitOfWork.RemoveData.RemovePhysicianNotification(physicianNotification);
            }
            var addPhysicianNotification = provideridlist.Except(physiciannotificationDB);
            foreach (var item in addPhysicianNotification)
            {
                PhysicianNotification physiciannotification = new PhysicianNotification
                {
                    PhysicianId = item,
                    IsNotificationStopped = bitset,
                };
                _unitOfWork.Add.AddPhysicianNotification(physiciannotification);
            };
            return RedirectToAction("Provider");
        }
        public IActionResult DeleteProviderAccount(int physicianid)
        {
            BitArray bitset = new BitArray(1);
            bitset[0] = true;
            Physician physician = _unitOfWork.tableData.GetPhysicianFirstOrDefault(physicianid);
            physician.IsDeleted = bitset;
            _unitOfWork.UpdateData.UpdatePhysician(physician);
            return RedirectToAction("Provider");
        }
    }
}
