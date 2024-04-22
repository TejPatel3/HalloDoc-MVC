using HalloDoc.DataContext;
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
                var physician = _context.Physicians.FirstOrDefault(m => m.Id == aspuser.Id.ToString());
                if (!_context.AspNetUsers.Where(user => user.PasswordHash == req.PasswordHash && user.Email == req.Email).Any())
                {
                    return 3;
                }
                if (!_context.Admins.Where(m => m.Email == req.Email).Any())
                {
                    if (physician != null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                if (!_context.Physicians.Where(m => m.Email == req.Email).Any())
                {
                    if (admin != null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            return 1;
        }
    }
}
