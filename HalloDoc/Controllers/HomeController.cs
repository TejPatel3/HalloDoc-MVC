using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Implementation;
using Services.ViewModels;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAddOrUpdateRequestStatusLog _addOrUpdateRequestStatusLog;

        public HomeController(ApplicationDbContext context, IAddOrUpdateRequestStatusLog rsl)
        {
            _context = context;
            _addOrUpdateRequestStatusLog = rsl;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateUser(string email)
        {
            var decryptemail = EncryptionDecryption.DecryptStringFromBase64_Aes(email);
            registrationViewModel model = new registrationViewModel
            {
                Email = decryptemail,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateUser(registrationViewModel obj)
        {
            Guid id = Guid.NewGuid();
            AspNetUser user = _context.AspNetUsers.FirstOrDefault(m => m.Email == obj.Email);
            User checkuser = _context.Users.FirstOrDefault(m => m.Email == obj.Email);
            if (checkuser != null)
            {
                TempData["error"] = "User Already Exist on this email";
                return RedirectToAction("Login");
            }
            else
            {
                if (user != null)
                {
                    user.PasswordHash = obj.PasswordHash;
                    _context.AspNetUsers.Add(user);
                    _context.SaveChanges();
                    TempData["success"] = "Your Account Created Successful";
                }
                else
                {
                    AspNetUser asp = new AspNetUser
                    {
                        Email = obj.Email,
                        Id = id.ToString(),
                        CreatedDate = DateTime.Now,
                        UserName = obj.FirstName + obj.LastName,
                        PasswordHash = obj.PasswordHash,
                    };
                    _context.AspNetUsers.Add(asp);
                    User user1 = new User
                    {
                        Email = obj.Email,
                        Id = id.ToString(),
                        CreatedDate = DateTime.Now,
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        CreatedBy = id.ToString(),
                        RegionId = 2,
                    };
                    _context.Users.Add(user1);
                    AspNetUserRole asprole = new AspNetUserRole
                    {
                        RoleId = "3",
                        UserId = id.ToString(),
                    };
                    _context.Add(asprole);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("PatientDashboard", "Dashboard");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AspNetUser obj)
        {
            if (!_context.Users.Where(user => user.Email == obj.Email).Any())
            {
                TempData["email"] = "Email Id Not Exist";
                return View(obj);
            }
            if (!_context.AspNetUsers.Where(m => m.Email == obj.Email && m.PasswordHash == obj.PasswordHash).Any())
            {
                TempData["pswd"] = "Enter Valid Password";
                return View(obj);
            }
            else if (_context.AspNetUsers.Where(m => m.Email == obj.Email).Any() && _context.AspNetUsers.Where(user => user.PasswordHash == obj.PasswordHash).Any())
            {
                var user = _context.Users.FirstOrDefault(m => m.Email == obj.Email);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
                TempData["success"] = "Login Successful...!";
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult resetPassword(string useremail, string id)
        {
            var encemail = EncryptionDecryption.DecryptStringFromBase64_Aes(useremail);
            AspNetUser model = _context.AspNetUsers.FirstOrDefault(m => m.Id == id && m.Email == encemail);
            if (model == null)
            {
                return RedirectToAction("AccessDenied");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult ForgotPassword(AspNetUser user)
        {
            return View(user);
        }
        private string GenerateResetPasswordUrl(string userId, string email)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string encryptemail = EncryptionDecryption.EncryptStringToBase64_Aes(email);
            string resetPasswordPath = Url.Action("resetPassword", "Home", new { id = userId, useremail = encryptemail });
            return baseUrl + resetPasswordPath;
        }
        public IActionResult PatientResetPasswordEmail(AspNetUser user)
        {
            var usercheck = _context.Users.FirstOrDefault(m => m.Email == user.Email);
            if (usercheck != null)
            {
                string Id = (_context.AspNetUsers.FirstOrDefault(x => x.Email == user.Email)).Id;
                string resetPasswordUrl = GenerateResetPasswordUrl(Id, user.Email);
                SendEmail(user.Email, "Reset Your Password", $"Hello, Click On below Link for Reset Your Password: <a href=\"{resetPasswordUrl}\">here</a>");
                TempData["success"] = "Reset Password Link sent Successful";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["error"] = "Email Id Not Exist First Crrete account";
            }
            return RedirectToAction("ForgotPassword", "Home");
        }


        private Task SendEmail(string email, string subject, string message)
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

        [HttpPost]
        public IActionResult resetPassword(AspNetUser aspnetuser)
        {
            var aspuser = _context.AspNetUsers.FirstOrDefault(x => x.Id == aspnetuser.Id);
            if (aspuser == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            else
            {
                aspuser.PasswordHash = aspnetuser.PasswordHash;
                _context.AspNetUsers.Update(aspuser);
                _context.SaveChanges();
                TempData["success"] = "Your Password Reset Successful";
                return RedirectToAction("Login");
            }
        }

        // Review agreement Method
        [HttpPost]
        public IActionResult ReviewAgreementcancelCaseModal(int id, AdminRequestViewModel cancelnote, string casetagname)
        {
            var req = _context.Requests.FirstOrDefault(m => m.RequestId == id);
            if (req.Status == 3 || req.Status == 4)
            {
                TempData["errorCancelAgreementmodel"] = "Your Responce already submitted";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                req.Status = 3;
                var casetag = _context.CaseTags.FirstOrDefault(t => t.Name == casetagname);
                if (casetag != null)
                {
                    req.CaseTag = casetag.CaseTagId.ToString();
                }
                _context.Requests.Update(req);
                _context.SaveChanges();
                var adminid = HttpContext.Session.GetInt32("AdminId");
                _addOrUpdateRequestStatusLog.AddOrUpdateRequestStatusLog(id, cancelnote.BlockNotes, adminid);
                TempData["success"] = "Request Canceled Successfully..!";
                return RedirectToAction("Login", "Home");
            }
        }
        public IActionResult ReviewAgreement(string id)
        {
            var viewModel = new AdminRequestViewModel();
            var eid = EncryptionDecryption.DecryptStringFromBase64_Aes(id);
            var request = _context.Requests.FirstOrDefault(m => m.RequestId.ToString() == eid);
            if (request == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            else if (request.UserId != HttpContext.Session.GetInt32("UserId"))
            {
                return RedirectToAction("Login");
            }
            else
            {

                var requestid = int.Parse(EncryptionDecryption.DecryptStringFromBase64_Aes(id));
                var PatienName = _context.Requests.FirstOrDefault(m => m.RequestId == requestid);
                viewModel.patientName = PatienName.FirstName + " " + PatienName.LastName;
                viewModel.requestid = requestid;
                return View(viewModel);
            }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

