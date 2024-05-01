using DataModels.DataContext;
using DataModels.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Implementation;
using System.Net;
using System.Net.Mail;

namespace DataModels.Controllers
{
    public class PatientRequestController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        public PatientRequestController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult CreateRequest()
        {
            return View();
        }

        // Get Method for Patient Request
        public IActionResult Patient(string? firstname = null, string? phonenumber = null, string? email = null)
        {
            if (email != null)
            {
                var model = new patientRequest();
                model.Email = email;
                model.FirstName = firstname;
                model.PhoneNumber = phonenumber;
                return View(model);
            }
            return View();
        }

        //Post Method for Patient Request
        [HttpPost]
        public async Task<IActionResult> Patient(patientRequest req)
        {
            Guid id = Guid.NewGuid();
            var Asp = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            if (Asp == null)
            {
                AspNetUser aspuser = new AspNetUser();
                aspuser.Email = req.Email;
                aspuser.PasswordHash = req.Password;
                aspuser.UserName = req.FirstName;
                aspuser.Id = id.ToString();
                aspuser.CreatedDate = DateTime.Now;
                aspuser.PhoneNumber = req.PhoneNumber;
                _context.AspNetUsers.Add(aspuser);
                AspNetUserRole asprole = new AspNetUserRole
                {
                    RoleId = 3.ToString(),
                    UserId = id.ToString(),
                };
                _context.Add(asprole);
                _context.SaveChanges();
            }

            User user = _context.Users.FirstOrDefault(m => m.Email == req.Email);
            if (user == null)
            {
                User addUser = new User();
                addUser.Email = req.Email;
                addUser.Id = id.ToString();
                addUser.FirstName = req.FirstName;
                addUser.LastName = req.LastName;
                addUser.CreatedBy = id.ToString();
                addUser.CreatedDate = DateTime.Now;
                addUser.City = req.City;
                addUser.Street = req.Street;
                addUser.State = req.State;
                addUser.ZipCode = req.ZipCode;
                addUser.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
                addUser.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
                addUser.StrMonth = req.BirthDate?.ToString("MMM");
                addUser.RegionId = 2;
                _context.Users.Add(addUser);
                _context.SaveChanges();
            }

            var users = await _context.Users.FirstOrDefaultAsync(m => m.Email == req.Email);
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.RegionId == users.RegionId);
            var requestcount = (from m in _context.Requests where m.CreatedDate.Date == DateTime.Now.Date select m).ToList();
            Request requests = new Request
            {
                UserId = users.UserId,
                RequestTypeId = 2,
                Status = 1,
                Email = users.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                CreatedDate = DateTime.Now,
                ConfirmationNumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
            };
            _context.Requests.Add(requests);
            _context.SaveChanges();

            int requestdata = requests.RequestId;
            RequestClient requestclients = new RequestClient
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
                RequestId = requestdata,
                RegionId = 1,
                Notes = req.Notes,
                Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
                IntDate = int.Parse(req.BirthDate?.ToString("dd")),
                IntYear = int.Parse(req.BirthDate?.ToString("yyyy")),
                StrMonth = req.BirthDate?.ToString("MMM"),
            };
            _context.RequestClients.Add(requestclients);
            _context.SaveChanges();

            if (req.Upload != null)
            {
                uploadFile(req.Upload, requestdata);
            }
            TempData["success"] = "Your Request Submited Successful...!";
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public void uploadFile(List<IFormFile> file, int id)
        {
            foreach (var item in file)
            {
                string path = _environment.WebRootPath + "/UploadDocument/" + id + item.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(fileStream);
                }
                RequestWiseFile requestWiseFiles = new RequestWiseFile
                {
                    RequestId = id,
                    FileName = path,
                    CreatedDate = DateTime.Now,
                };
                _context.RequestWiseFiles.Add(requestWiseFiles);
                _context.SaveChanges();
            }
        }

        //Get method for Family-Friend 
        public IActionResult FamilyFriend()
        {
            return View();
        }

        //Post method for Family-Friend 
        [HttpPost]
        public async Task<IActionResult> FamilyFriend(request req)
        {
            var aspuser = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Email == req.Email);
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.RegionId == user.RegionId);
            var requestcount = (from m in _context.Requests where m.CreatedDate.Date == DateTime.Now.Date select m).ToList();
            if (aspuser != null)
            {
                aspuser.PhoneNumber = req.PhoneNumber;
                user.Mobile = req.PhoneNumber;
                user.Street = req.Street;
                user.City = req.City;
                user.State = req.State;
                user.ZipCode = req.ZipCode;
                user.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
                user.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
                user.StrMonth = req.BirthDate?.ToString("MMM");
                _context.AspNetUsers.Update(aspuser);
                _context.Users.Update(user);
                _context.SaveChanges();
                Request requests = new Request
                {
                    FirstName = req.rFirstName,
                    LastName = req.rLastName,
                    Email = req.rEmail,
                    CreatedDate = DateTime.Now,
                    RequestTypeId = 3,
                    Status = 1,
                    UserId = user.UserId,
                    PhoneNumber = req.rPhoneNumber,
                    ModifiedDate = DateTime.Now,
                    User = user,
                    ConfirmationNumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                };
                _context.Requests.Add(requests);
                _context.SaveChanges();
                RequestClient requestclients = new RequestClient
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    PhoneNumber = req.PhoneNumber,
                    Street = req.Street,
                    City = req.City,
                    State = req.State,
                    ZipCode = req.ZipCode,
                    RequestId = requests.RequestId,
                    RegionId = 1,
                    Notes = req.Notes,
                    Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
                    IntDate = int.Parse(req.BirthDate?.ToString("dd")),
                    IntYear = int.Parse(req.BirthDate?.ToString("yyyy")),
                    StrMonth = req.BirthDate?.ToString("MMM"),
                };
                _context.RequestClients.Add(requestclients);
                _context.SaveChanges();

                if (req.Upload != null)
                {
                    uploadFile(req.Upload, requests.RequestId);
                }
            }
            if (_context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email) == null)
            {
                familyCreatePatient(req);
                var aspuser1 = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
                PatientResetPasswordEmail(aspuser1);
            }

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            else
            {
                return RedirectToAction("CreateRequest");
            }
        }
        public IActionResult familyCreatePatient(request req)
        {
            Guid id = Guid.NewGuid();
            AspNetUser aspuser = new AspNetUser();
            aspuser.Email = req.Email;
            aspuser.UserName = req.FirstName;
            aspuser.Id = id.ToString();
            aspuser.CreatedDate = DateTime.Now;
            aspuser.PhoneNumber = req.PhoneNumber;
            _context.AspNetUsers.Add(aspuser);
            AspNetUserRole asprole = new AspNetUserRole
            {
                RoleId = 3.ToString(),
                UserId = id.ToString(),
            };
            _context.Add(asprole);
            _context.SaveChanges();
            User addUser = new User();
            addUser.Email = req.Email;
            addUser.Id = id.ToString();
            addUser.FirstName = req.FirstName;
            addUser.LastName = req.LastName;
            addUser.CreatedBy = id.ToString();
            addUser.CreatedDate = DateTime.Now;
            addUser.City = req.City;
            addUser.Street = req.Street;
            addUser.State = req.State;
            addUser.ZipCode = req.ZipCode;
            addUser.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
            addUser.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
            addUser.StrMonth = req.BirthDate?.ToString("MMM");
            addUser.RegionId = 2;
            _context.Users.Add(addUser);
            _context.SaveChanges();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("PatientDashboard", "Dashboard");
            }
            else
            {
                return RedirectToAction("CreateRequest");
            }
        }
        //Email send 
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("CreateUser", "Home", new { id = userId });
            return baseUrl + resetPasswordPath;
        }
        public IActionResult PatientResetPasswordEmail(AspNetUser user)
        {
            var usercheck = _context.Users.FirstOrDefault(m => m.Email == user.Email);
            if (usercheck != null)
            {
                string Id = (_context.AspNetUsers.FirstOrDefault(x => x.Email == user.Email)).Id;
                string resetPasswordUrl = GenerateResetPasswordUrl(Id);
                SendEmail(user.Email, "Reset Your Password", $"Hello, Click On below Link for Reset Your Password: {resetPasswordUrl}");
                TempData["success"] = "Reset Password Link sent Successful";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["error"] = "Email Id Not Exist First Crrete account";
            }
            return RedirectToAction("ForgotPassword", "PatientRequest");
        }
        private string GenerateCreateUserLink(string email)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var encryptemail = EncryptionDecryption.EncryptStringToBase64_Aes(email);
            string resetPasswordPath = Url.Action("CreateUser", "Home", new { email = encryptemail });
            return baseUrl + resetPasswordPath;
        }
        public IActionResult CreateUserSendMail(string email)
        {
            string resetPasswordUrl = GenerateCreateUserLink(email);
            SendEmail(email, "Crete Your Account", $"Hello, Click On below Link for Crete Your Account:{resetPasswordUrl}");
            TempData["success"] = "crete user account Link sent Successful";
            return RedirectToAction("CreateRequest", "PatientRequest");
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
            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        [HttpPost]
        public IActionResult CreatePatient(AspNetUser aspnetuser)
        {
            var aspuser = _context.AspNetUsers.FirstOrDefault(x => x.Email == aspnetuser.Email);
            aspuser.PasswordHash = aspnetuser.PasswordHash;
            _context.AspNetUsers.Update(aspuser);
            _context.SaveChanges();
            TempData["success"] = "Your Password Reset Successful";
            return RedirectToAction("PatientDashboard", "Dashboard");
        }

        //Get Method for Concierge Patient Request
        public IActionResult Concierge()
        {
            return View();
        }

        //Get Method for Concierge Patient Request
        [HttpPost]
        public async Task<IActionResult> Concierge(conciergeViewModel req)
        {
            var aspuser = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Email == req.Email);
            aspuser.PhoneNumber = req.PhoneNumber;
            user.Mobile = req.PhoneNumber;
            user.Street = req.Street;
            user.City = req.City;
            user.State = req.State;
            user.ZipCode = req.ZipCode;
            user.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
            user.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
            user.StrMonth = req.BirthDate?.ToString("MMM");
            _context.AspNetUsers.Update(aspuser);
            _context.Users.Update(user);
            _context.SaveChanges();
            Request requests = new Request
            {
                UserId = user.UserId,
                FirstName = req.rFirstName,
                LastName = req.rLastName,
                Email = req.rEmail,
                PhoneNumber = req.rPhoneNumber,
                CreatedDate = DateTime.Now,
                RequestTypeId = 4,
                Status = 1,
            };
            _context.Requests.Add(requests);
            _context.SaveChanges();

            RequestClient requestclients = new RequestClient
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
                RequestId = requests.RequestId,
                RegionId = 1,
                Notes = req.Notes,
                Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
                IntDate = int.Parse(req.BirthDate?.ToString("dd")),
                IntYear = int.Parse(req.BirthDate?.ToString("yyyy")),
                StrMonth = req.BirthDate?.ToString("MMM"),
            };
            _context.RequestClients.Add(requestclients);
            _context.SaveChanges();

            Concierge congierges = new Concierge
            {
                ConciergeName = req.FirstName + " " + req.LastName,
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
                CreatedDate = DateTime.Now,
                Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
            };
            _context.Concierges.Add(congierges);
            _context.SaveChanges();

            RequestConcierge reqconcierges = new RequestConcierge
            {
                RequestId = requests.RequestId,
                ConciergeId = congierges.ConciergeId,
            };
            _context.RequestConcierges.Add(reqconcierges);
            _context.SaveChanges();
            return RedirectToAction("CreateRequest");
        }

        //Get Method for Business Patient Request
        public IActionResult Business()
        {
            return View();
        }

        //Post Method for Business Patient Request
        [HttpPost]
        public async Task<IActionResult> Business(businessViewModel req)
        {
            var aspuser = await _context.AspNetUsers.FirstOrDefaultAsync(m => m.Email == req.Email);
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Email == req.Email);
            aspuser.PhoneNumber = req.PhoneNumber;
            user.Mobile = req.PhoneNumber;
            user.Street = req.Street;
            user.City = req.City;
            user.State = req.State;
            user.ZipCode = req.ZipCode;
            user.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
            user.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
            user.StrMonth = req.BirthDate?.ToString("MMM");
            _context.AspNetUsers.Update(aspuser);
            _context.Users.Update(user);
            _context.SaveChanges();
            Request requests = new Request
            {
                UserId = user.UserId,
                FirstName = req.rFirstName,
                LastName = req.rLastName,
                Email = req.rEmail,
                PhoneNumber = req.rPhoneNumber,
                CreatedDate = DateTime.Now,
                RequestTypeId = 1,
                Status = 1,
            };
            _context.Requests.Add(requests);
            _context.SaveChanges();

            RequestClient requestclients = new RequestClient
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
                RequestId = requests.RequestId,
                RegionId = 1,
                Notes = req.Notes,
                Address = req.Street + " , " + req.City + " , " + req.State + " , " + req.ZipCode,
                IntDate = int.Parse(req.BirthDate?.ToString("dd")),
                IntYear = int.Parse(req.BirthDate?.ToString("yyyy")),
                StrMonth = req.BirthDate?.ToString("MMM"),
            };
            _context.RequestClients.Add(requestclients);
            _context.SaveChanges();

            Business businesses = new Business
            {
                Name = req.rFirstName + " " + req.rLastName,
                PhoneNumber = req.rPhoneNumber,
                CreatedDate = DateTime.Now,
                Address1 = req.Street + " , " + req.City,
                Address2 = req.State + " , " + req.ZipCode,
                City = req.City,
                RegionId = 1,
                ZipCode = req.ZipCode,
            };
            _context.Businesses.Add(businesses);
            _context.SaveChanges();

            RequestBusiness reqBusiness = new RequestBusiness
            {
                RequestId = requests.RequestId,
                BusinessId = businesses.BusinessId,
            };
            _context.RequestBusinesses.Add(reqBusiness);
            _context.SaveChanges();
            return RedirectToAction("CreateRequest");
        }

        [Route("/Patient/PatientInfoPage/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.AspNetUsers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        public bool CheckUserExist(string email)
        {
            bool check = false;
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                check = true;
            }
            return check;
        }
    }
}