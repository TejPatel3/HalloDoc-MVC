using DataModels.DataContext;
using DataServices.Interface;

namespace DataServices.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        public LoginRepository()
        {
            _context = new ApplicationDbContext();
        }
    }
}
