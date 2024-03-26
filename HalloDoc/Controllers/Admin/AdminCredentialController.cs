using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.ViewModels;
using System.Net.Mail;
using System.Net;

namespace HalloDoc.Controllers.Admin
{
    public class AdminCredentialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminLog adminLog;
        private readonly IJwtRepository _jwtRepo;

        public AdminCredentialController(IAdminLog _admin, IJwtRepository jwtRepository)
        {
            _context = new ApplicationDbContext();
            adminLog = _admin;
            _jwtRepo = jwtRepository;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AspNetUser req)
        {
            int num = adminLog.AdminLogin(req);
            if (num == 1)
            {
                TempData["email"] = "Email Not Exist";
                return View(req);
            }

            else if (num == 2)
            {
                TempData["email"] = "You Are not Admin";
                return View(req);
            }

            else if (num == 3)
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(req);
            }

            else if (num == 0)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.Email == req.Email);
                HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                HttpContext.Session.SetString("AdminName", $"{admin.FirstName} {admin.LastName}");
                TempData["success"] = "Login Successful...!";
                TempData["user"] = admin.FirstName;
                var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
                var LogedinUser = new LogedInUserViewModel();
                LogedInUserViewModel loggedInPersonViewModel = new LogedInUserViewModel();
                loggedInPersonViewModel.AspNetUserId = aspnetuser.Id;
                loggedInPersonViewModel.UserName = aspnetuser.UserName;
                var Roleid = _context.AspNetUserRoles.FirstOrDefault(x => x.UserId == aspnetuser.Id).UserId.ToString();
                loggedInPersonViewModel.RoleName = _context.AspNetRoles.FirstOrDefault(x => x.Id == Roleid).Name;
                Response.Cookies.Append("jwt", _jwtRepo.GenerateJwtToken(loggedInPersonViewModel));
                return RedirectToAction("AdminDashboard", "Admin");
            }
            return View(req);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("AdminLogin", "AdminCredential");
        }


        [HttpPost]
        public IActionResult ResetPassword(string email, string password)
        {
            var adiminid = HttpContext.Session.GetInt32("AdminId");
            var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == email);
            aspnetuser.PasswordHash = password;
            _context.AspNetUsers.Update(aspnetuser);
            _context.SaveChanges();
            TempData["success"] = "Password Reset Successfully...!";
            return RedirectToAction("AdminProfile", "Admin");
        }
        public IActionResult AdminForgotPassword()
        {
            return View();
        }
        public int generateconfirmationcode()
        {
            Random rnd = new Random();
            int confirmationcode = rnd.Next(100000, 1000000);
            return confirmationcode;
        }
        [HttpPost]
        public IActionResult AdminForgotPassword(bool sendcode, bool checkcode, bool updatepassword, string email, int confirmationcode, string password ,int originalconfirmationcode)
        {
            int generatedconfirmationcode = 0;

            if (sendcode)
            {
                generatedconfirmationcode = generateconfirmationcode();
                TempData["Confirmationcode"] = generatedconfirmationcode;
                SendEmail(email, "Your Attachments", "Please Find Your Attachments Here", generatedconfirmationcode);
                
                TempData["success"] = "Code sent successfully";

                return Json(new { success = true, confirmationCode = generatedconfirmationcode });
            }
            if (checkcode)
            {
                if (confirmationcode == originalconfirmationcode)
                {
                    //return Json(new { redirectToUrl = Url.Action("AdminForgotPassword", "AdminCredential") });

                    return View();
                }
                else
                {
                return Json(new { success = true, confirmationnumbernotmatch = true });
                    //TempData["warning"] = "Confirmation code wrong";
                    //return View();
                    //TempData["warning"] = "Confirmation code wrong";
                    //return Json(new { message = TempData["warning"].ToString() });
                }
            }
            if (updatepassword)
            {
                var aspnetuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == email);
                aspnetuser.PasswordHash = password;
                _context.AspNetUsers.Update(aspnetuser);
                _context.SaveChanges();
                return RedirectToAction("AdminLogin");
            }
            return RedirectToAction("AdminLogin");
        }

        private Task SendEmail(string email, string subject, string message, int confirmationcode)
        {
            var mail = "tatva.dotnet.tejpatel@outlook.com";
            var password = "7T6d2P3@K";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message + "\n" + confirmationcode,
            };
            mailMessage.To.Add(email);
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
