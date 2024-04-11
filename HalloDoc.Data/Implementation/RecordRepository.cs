using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;

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
        public List<EmailLogViewModel> GetEmailLogs()
        {
            var emailLogs = _context.EmailLogs.ToList();
            var model = new List<EmailLogViewModel>();
            foreach (var item in emailLogs)
            {
                var viewmodel = new EmailLogViewModel();
                viewmodel.ConfirmationNumber = item.ConfirmationNumber;
                viewmodel.CreatedDate = item.CreateDate.ToString("MMM dd, yyyy");
                viewmodel.SentDate = item.SentDate;
                viewmodel.Email = item.EmailId;

                model.Add(viewmodel);
            }
            return model;
        }
        public EmailLogViewModel GetAspNetRoleList()
        {
            var model = new EmailLogViewModel();
            model.AspNetRoleList = _context.AspNetRoles.ToList();
            return model;
        }
    }
}
