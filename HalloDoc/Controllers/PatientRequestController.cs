using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace HalloDoc.Controllers
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
        public IActionResult Patient()
        {
            return View();
        }

        //Post Method for Patient Request
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _context.AspNetUsers.Add(aspuser);
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
                addUser.ModifiedBy = id.ToString();
                addUser.IntYear = int.Parse(req.BirthDate?.ToString("yyyy"));
                addUser.IntDate = int.Parse(req.BirthDate?.ToString("dd"));
                addUser.StrMonth = req.BirthDate?.ToString("MMM");
                //users.IntDate = req.BirthDat

                _context.Users.Add(addUser);
                _context.SaveChanges();
            }
            var users = await _context.Users.FirstOrDefaultAsync(m => m.Email == req.Email);
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.RegionId == user.RegionId);
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
                ConfirmationNumber = $"{req.FirstName.Substring(0, 2)}{req.BirthDate.ToString().Substring(0, 2)}{req.LastName.Substring(0, 2)}{req.BirthDate.ToString().Substring(3, 2)}{req.BirthDate.ToString().Substring(6, 4)}",
                //ConfirmationNumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + req.LastName.Substring(0, 2) + req.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
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
                //IntDate = req.BirthDate.Day,
                //IntYear = req.BirthDate.Year,
                //StrMonth = req.BirthDate.ToString("MMMM"),
            };
            _context.RequestClients.Add(requestclients);
            _context.SaveChanges();


            if (req.Upload != null)
            {
                uploadFile(req.Upload, requestdata);
            }

            HttpContext.Session.SetInt32("UserId", users.UserId);
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
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", id,item.FileName);
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
            if (aspuser != null)
            {
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
                    //ConfirmationNumber = $"{req.FirstName.Substring(0, 2)}{req.BirthDate.ToString().Substring(0, 2)}{req.LastName.Substring(0, 2)}{req.BirthDate.ToString().Substring(3, 2)}{req.BirthDate.ToString().Substring(6, 4)}",
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
                };
                _context.RequestClients.Add(requestclients);
                _context.SaveChanges();

                if (req.Upload != null)
                {
                    uploadFile(req.Upload, requests.RequestId);
                }
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

        //Get Method for Concierge Patient Request
        public IActionResult Concierge()
        {
            return View();
        }

        //Get Method for Concierge Patient Request
        [HttpPost]
        public async Task<IActionResult> Concierge(request req)
        {

            Request requests = new Request
            {
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

            var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == req.Email);
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
                RequestId = requestdata.RequestId,
                RegionId = 1,
                Notes = req.Notes,
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
        public async Task<IActionResult> Business(request req)
        {
            Request requests = new Request
            {
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

            var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == req.Email);
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
                RequestId = requestdata.RequestId,
                RegionId = 1,
                Notes = req.Notes,
            };
            _context.RequestClients.Add(requestclients);
            _context.SaveChanges();

            Business businesses = new Business
            {
                Name = req.rFirstName + " " + req.rLastName,
                PhoneNumber = req.rPhoneNumber,
                CreatedDate = DateTime.Now,
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



    }



}