using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public RecordRepository()
        {
            _context = new ApplicationDbContext();
        }
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
    }
}
