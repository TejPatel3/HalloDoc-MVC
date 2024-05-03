using DataModels.DataContext;
using DataModels.DataModels;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System.Collections;
using System.Globalization;

namespace Services.Implementation
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public RecordRepository()
        {
            _context = new ApplicationDbContext();
        }

        //patient history methods
        public List<User> GetUserList(string firstname, string lastname, string email, int phone)
        {
            List<User> list = _context.Users.ToList();
            if (firstname != null)
            {
                list = list.Where(m => m.FirstName.ToLower().Contains(firstname.ToLower())).ToList();
            }
            if (lastname != null)
            {
                list = list.Where(m => m.LastName.ToLower().Contains(lastname.ToLower())).ToList();
            }
            if (email != null)
            {
                list = list.Where(m => m.Email.ToLower().Contains(email.ToLower())).ToList();
            }
            if (phone != 0)
            {
                list = list.Where(m => m.Mobile.ToLower().Contains(phone.ToString())).ToList();
            }
            return list;
        }

        //Patient Record method
        public List<PatientRecordViewModel> GetPatientRecordData(int userid)
        {
            List<PatientRecordViewModel> patientRecordViewModels = new List<PatientRecordViewModel>();
            List<Request> request = _context.Requests.Include(m => m.RequestClients).Include(m => m.RequestStatusLogs).Where(m => m.UserId == userid).ToList();
            foreach (var item in request)
            {
                PatientRecordViewModel model = new PatientRecordViewModel();
                model.ClientName = item.RequestClients.FirstOrDefault().FirstName + item.RequestClients.FirstOrDefault().LastName;
                model.CreatedDate = item.CreatedDate.ToString("MMM dd,yyyy");
                model.Confirmation = item.ConfirmationNumber;
                var physician = _context.Physicians.FirstOrDefault(m => m.PhysicianId == item.PhysicianId);
                model.ProviderName = physician != null ? physician.FirstName + physician.LastName : "-";
                model.ConcludedDate = "-";
                if (item.RequestStatusLogs.Count > 0)
                {
                    //var ConcludedDateobj = item.RequestStatusLogs.FirstOrDefault(m => m.Status == 6).CreatedDate.ToString("MMM dd,yyyy");
                    var ConcludedDateobj = _context.RequestStatusLogs.FirstOrDefault(m => m.Status == 6 && m.RequestId == item.RequestId);

                    model.ConcludedDate = ConcludedDateobj != null ? ConcludedDateobj.CreatedDate.ToString() : "-";
                }
                model.RequestId = item.RequestId;
                patientRecordViewModels.Add(model);
            }
            return patientRecordViewModels;
        }

        //Email Log Methods
        public List<EmailLogViewModel> GetEmailLogs(int Role, string ReceiverName, string Email, DateTime CreatedDate, DateTime SentDate)
        {
            var emailLogs = _context.EmailLogs.ToList();
            var aspnetrole = _context.AspNetRoles.ToList();
            var model = new List<EmailLogViewModel>();
            foreach (var item in emailLogs)
            {
                var viewmodel = new EmailLogViewModel();
                viewmodel.ConfirmationNumber = item.ConfirmationNumber != null ? item.ConfirmationNumber : "-";
                viewmodel.CreatedDate = item.CreateDate.ToString("MMM dd, yyyy");
                var sentdateobj = item.SentDate;
                viewmodel.SentDate = sentdateobj != null ? sentdateobj?.ToString("MMM dd, yyyy") : "-";
                viewmodel.Email = item.EmailId;
                viewmodel.SentTries = item.SentTries.ToString();
                viewmodel.Sent = item.IsEmailSent[0] == true ? item.IsEmailSent : new BitArray(new[] { false });
                viewmodel.Action = item.SubjectName;
                viewmodel.Recipint = item.EmailId.Split("@")[0];
                viewmodel.RoleName = aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()) != null ? aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()).Name : "-";
                viewmodel.RoleId = aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()) != null ? aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()).Id : "-";
                model.Add(viewmodel);
            }
            if (Role != 0)
            {
                model = model.Where(m => m.RoleId == Role.ToString()).ToList();
            }
            if (ReceiverName != null)
            {
                model = model.Where(m => m.Recipint.ToLower().Contains(ReceiverName.ToLower())).ToList();

            }
            if (Email != null)
            {
                model = model.Where(m => m.Email.ToLower().Contains(Email.ToLower())).ToList();

            }
            if (CreatedDate.ToString() != "01-01-0001 00:00:00")
            {
                model = model.Where(m => m.CreatedDate == CreatedDate.ToString("MMM dd, yyyy")).ToList();
            }
            if (SentDate.ToString() != "01-01-0001 00:00:00")
            {
                model = model.Where(m => m.SentDate == SentDate.ToString("MMM dd, yyyy")).ToList();
            }
            return model;
        }
        public EmailLogViewModel GetAspNetRoleList()
        {
            var model = new EmailLogViewModel();
            model.AspNetRoleList = _context.AspNetRoles.ToList();
            return model;
        }

        // sms log methods
        public List<EmailLogViewModel> GetSMSLogs(int Role, string ReceiverName, string Email, DateTime CreatedDate, DateTime SentDate)
        {
            var SMSLogs = _context.Smslogs.ToList();
            var aspnetrole = _context.AspNetRoles.ToList();
            var model = new List<EmailLogViewModel>();
            foreach (var item in SMSLogs)
            {
                var viewmodel = new EmailLogViewModel();
                viewmodel.ConfirmationNumber = item.ConfirmationNumber != null ? item.ConfirmationNumber : "-";
                viewmodel.CreatedDate = item.CreateDate.ToString("MMM dd, yyyy");
                var sentdateobj = item.SentDate;
                viewmodel.SentDate = sentdateobj != null ? sentdateobj?.ToString("MMM dd, yyyy") : "-";
                viewmodel.Email = item.MobileNumber.ToString();
                viewmodel.SentTries = item.SentTries.ToString();
                viewmodel.Sent = item.IsSmssent[0] == true ? item.IsSmssent : new BitArray(new[] { false });
                viewmodel.Action = "-";
                viewmodel.Recipint = item.SmslogId.ToString().Split("@")[0];
                viewmodel.RoleName = aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()) != null ? aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()).Name : "-";
                viewmodel.RoleId = aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()) != null ? aspnetrole.FirstOrDefault(x => x.Id == item.RoleId.ToString()).Id : "-";
                model.Add(viewmodel);
            }
            if (Role != 0)
            {
                model = model.Where(m => m.RoleId == Role.ToString()).ToList();
            }
            if (ReceiverName != null)
            {
                model = model.Where(m => m.Recipint.ToLower().Contains(ReceiverName.ToLower())).ToList();

            }
            if (Email != null)
            {
                model = model.Where(m => m.Email.ToLower().Contains(Email.ToLower())).ToList();

            }
            if (CreatedDate.ToString() != "01-01-0001 00:00:00")
            {
                model = model.Where(m => m.CreatedDate == CreatedDate.ToString("MMM dd, yyyy")).ToList();
            }
            if (SentDate.ToString() != "01-01-0001 00:00:00")
            {
                model = model.Where(m => m.SentDate == SentDate.ToString("MMM dd, yyyy")).ToList();
            }
            return model;
        }

        public List<RecordViewModel> GetSearchRecordData(int reqstatus, string patientname, int requesttype, string fromdateofservice,
            string todateofservice, string physicianname, string email, string phonenumber)
        {
            var recorddata = _context.Requests.Where(x => x.IsDeleted == null).Include(r => r.RequestClients).Include(x => x.Physician).Include(r => r.RequestStatusLogs).Include(r => r.RequestNotes).ToList();

            //for request status search
            if (reqstatus != 0)
            {
                if (reqstatus == 45)
                {
                    recorddata = recorddata.Where(x => x.Status == 4 || x.Status == 5).ToList();

                }
                else if (reqstatus == 378)
                {
                    recorddata = recorddata.Where(x => x.Status == 3 || x.Status == 7 || x.Status == 8).ToList();

                }
                else
                {
                    recorddata = recorddata.Where(x => x.Status == reqstatus).ToList();
                }
            }

            //for patientname search
            if (patientname != null)
            {
                recorddata = recorddata.Where(x => (x.RequestClients.FirstOrDefault().FirstName +
                x.RequestClients.FirstOrDefault().FirstName)
                .ToLower()
                .Contains(patientname.ToLower()))
                .ToList();
            }

            //for requesttype search
            if (requesttype != 0)
            {
                recorddata = recorddata.Where(x => x.RequestTypeId == requesttype).ToList();
            }
            //for fromdateofservice search

            if (fromdateofservice != null)
            {
                DateTime dt = DateTime.ParseExact(fromdateofservice, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                recorddata = recorddata.Where(m => m.CreatedDate >= dt).ToList();
            }

            //for todateofservice search

            if (todateofservice != null)
            {
                DateOnly dt = DateOnly.ParseExact(todateofservice, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                recorddata = recorddata.Where(m => DateOnly.FromDateTime(m.CreatedDate) <= dt).ToList();
            }

            //for physicianname search

            if (physicianname != null)
            {
                recorddata = recorddata.Where(x => x.Physician != null).ToList();
                recorddata = recorddata.Where(x => (x.Physician.FirstName + x.Physician.LastName).ToLower().Contains(physicianname.ToLower())).ToList();
            }

            //for email search

            if (email != null)
            {
                recorddata = recorddata.Where(x => x.RequestClients.FirstOrDefault().Email.ToLower().Contains(email.ToLower())).ToList();
            }

            //for phonenumber search

            if (phonenumber != null)
            {
                recorddata = recorddata.Where(x => x.RequestClients.FirstOrDefault().PhoneNumber.ToLower().Contains(phonenumber)).ToList();

            }

            List<RecordViewModel> records = new List<RecordViewModel>();


            foreach (var request in recorddata)
            {
                var model = new RecordViewModel();
                model.PatientName = request.RequestClients.FirstOrDefault().FirstName + " " + request.RequestClients.FirstOrDefault().LastName;
                model.Requestor = model.RequestTypeName(request.RequestTypeId);
                model.DateOfService = request.CreatedDate.ToString("dd MMM, yyyy");

                model.Physician = "-";
                if (request.PhysicianId != null)
                {
                    model.Physician = _context.Physicians.FirstOrDefault(x => x.PhysicianId == request.PhysicianId).FirstName.ToString();

                }

                model.CloseCaseDate = "-";
                model.CancelProviderNote = "-";
                if (request.RequestStatusLogs.Count() > 0)
                {

                    var noteobj = request.RequestStatusLogs.Where(m => m.Status == 3 && m.PhysicianId == request.PhysicianId).FirstOrDefault();
                    model.CancelProviderNote = noteobj != null ? noteobj.Notes : "-";
                    model.CloseCaseDate = request.RequestStatusLogs.FirstOrDefault().CreatedDate.ToString("MMM dd,yyyy") ?? "-";

                }
                model.PhysicianNote = "-";
                model.AdminNote = "-";
                if (request.RequestNotes.Count() > 0)
                {
                    model.PhysicianNote = request.RequestNotes.FirstOrDefault().PhysicianNotes ?? "-";
                    model.AdminNote = request.RequestNotes.FirstOrDefault().AdminNotes ?? "-";

                }


                model.Email = request.RequestClients.FirstOrDefault().Email;
                model.PhoneNumber = request.RequestClients.FirstOrDefault().PhoneNumber != null ? request.RequestClients.FirstOrDefault().PhoneNumber.ToString() : "-";
                model.Address = request.RequestClients.FirstOrDefault().Address;
                model.Zip = request.RequestClients.FirstOrDefault().ZipCode;
                model.RequestStatus = request.Status;
                model.PatientNote = request.RequestClients.FirstOrDefault().Notes;
                model.requestid = request.RequestId;
                records.Add(model);
            }


            return records;
        }

        //for deleting search record
        public int DeleteSearchRecord(int requestid)
        {
            var x = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);


            if (x != null)
            {
                x.IsDeleted = new BitArray(new[] { true });
                _context.Requests.Update(x);
            }
            _context.SaveChanges();

            return 1;
        }



        //get block history data

        public List<RecordViewModel> GetBlockHistoryFilterData(string name, string date, string email, string phonenumber)
        {

            var blockdata = _context.BlockRequests.ToList();


            if (date != null)
            {

                blockdata = blockdata.Where(x => DateOnly.FromDateTime((DateTime)x.CreatedDate).ToString("yyyy-MM-dd") == date).ToList();

            }


            //for email search

            if (email != null)
            {
                blockdata = blockdata.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            //for phonenumber search

            if (phonenumber != null)
            {
                blockdata = blockdata.Where(x => x.PhoneNumber.Contains(phonenumber)).ToList();
            }

            List<RecordViewModel> blockhistorydata = new List<RecordViewModel>();
            foreach (var x in blockdata)
            {


                var model = new RecordViewModel();

                model.PatientName = _context.RequestClients.FirstOrDefault(m => m.RequestId == int.Parse(x.RequestId)).FirstName +
                " " + _context.RequestClients.FirstOrDefault(m => m.RequestId == int.Parse(x.RequestId)).LastName;

                model.PhoneNumber = x.PhoneNumber;
                model.Email = x.Email;
                model.DateOfService = x.CreatedDate.ToString();
                model.PatientNote = x.Reason;
                model.requestid = int.Parse(x.RequestId);//for unblock
                blockhistorydata.Add(model);


            }

            //for name search
            if (name != null)
            {
                blockhistorydata = blockhistorydata.Where(x => x.PatientName.ToLower().Contains(name.ToLower())).ToList();
            }

            return blockhistorydata;
        }

        public int UnblockBlockHistory(int requestid, int adminid)
        {
            var requestdata = _context.Requests.FirstOrDefault(x => x.RequestId == requestid);
            var blockhistory = _context.BlockRequests.FirstOrDefault(x => x.RequestId == requestid.ToString());
            var requeststatuslog = new RequestStatusLog();

            if (requestid != 0)
            {
                requestdata.Status = 1;
                requeststatuslog.RequestId = requestid;
                requeststatuslog.Status = 1;
                requeststatuslog.AdminId = adminid;
                requeststatuslog.CreatedDate = DateTime.Now;
            }

            if (requeststatuslog != null && requestdata != null && blockhistory != null)
            {
                _context.Requests.Update(requestdata);
                _context.RequestStatusLogs.Add(requeststatuslog);
                _context.BlockRequests.Remove(blockhistory);
                _context.SaveChanges();
                return 1;

            }

            return 0;
        }
    }
}
