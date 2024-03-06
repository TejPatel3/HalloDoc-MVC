using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;
using Services.ViewModels;
using System.Globalization;

namespace Services.Implementation
{
    public class ViewCaseRepo : Repository<RequestClient>, IViewCaseRepo
    {
        private readonly ApplicationDbContext _context;

        public ViewCaseRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void EditInfo(ViewCaseViewModel viewModel)
        {
            var request = _context.Requests.FirstOrDefault(m => m.ConfirmationNumber == viewModel.ConfirmationNumber);
            request.FirstName = viewModel.FirstName;
            request.LastName = viewModel.LastName;
            request.PhoneNumber = viewModel.PhoneNumber;
            request.ModifiedDate = DateTime.Now;

            var requestclient = _context.RequestClients.FirstOrDefault(m => m.RequestId == request.RequestId);
            requestclient.FirstName = viewModel.FirstName;
            requestclient.LastName = viewModel.LastName;
            requestclient.PhoneNumber = viewModel.PhoneNumber;
            requestclient.Notes = viewModel.PatientNotes;
            requestclient.IntDate = int.Parse(viewModel.DOB.ToString("dd"));
            requestclient.IntYear = int.Parse(viewModel.DOB.ToString("yyyy"));
            requestclient.StrMonth = viewModel.DOB.ToString("MMM");
            if (request != null && requestclient != null)
            {
                _context.Requests.Update(request);
                _context.SaveChanges();
                _context.RequestClients.Update(requestclient);
                _context.SaveChanges();
            }
        }

        public ViewCaseViewModel GetViewCaseData(int reqid)
        {
            var model = _context.RequestClients.FirstOrDefault(m => m.RequestId == reqid);
            var regionName = _context.Regions.FirstOrDefault(m => m.RegionId == model.RegionId);
            var request = _context.Requests.FirstOrDefault(m => m.RequestId == reqid);
            var details = new ViewCaseViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DOB = new DateTime(Convert.ToInt32(model.IntYear), DateTime.ParseExact(model.StrMonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(model.IntDate)),
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                Region = regionName.Name,
                ConfirmationNumber = request.ConfirmationNumber,
                PatientNotes = model.Notes,
                requestId = reqid,
                status = request.Status,
            };
            return details;
        }

    }
}
