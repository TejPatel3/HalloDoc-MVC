using HalloDoc.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class AdminLog : IAdminLog
    {
        private readonly ApplicationDbContext _context;
        public AdminLog(ApplicationDbContext context)
        {
            _context = context;
        }
        public int AdminLogin(AspNetUser req)
        {
            var aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            if (!_context.AspNetUsers.Where(m => m.Email == req.Email).Any())
            {
                return 1;
            }
            if (aspuser != null)
            {
                var admin = _context.Admins.FirstOrDefault(m => m.AspNetUserId == aspuser.Id.ToString());
                if (!_context.Admins.Where(m => m.Email == req.Email).Any())
                {
                    return 2;
                }
                else if (!_context.AspNetUsers.Where(user => user.PasswordHash == req.PasswordHash).Any())
                {
                    return 3;
                }
                else if (_context.AspNetUsers.Where(m => m.Email == req.Email).Any() && _context.AspNetUsers.Where(user => user.PasswordHash == req.PasswordHash).Any())
                {
                    return 0;
                }
            }
            return -1;
        }
    }
}
