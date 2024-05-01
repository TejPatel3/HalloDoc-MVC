using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class AdminDashboard : Repository<Request>, IAdminDashboard
    {
        public ApplicationDbContext _context { get; }
        public AdminDashboard(ApplicationDbContext context) : base(context)
        {
        }
    }
}
