using DataModels.DataContext;
using DataModels.DataModels;
using Services.Contracts;

namespace Services.Implementation
{
    public class AccessRepository : IAccessRepository
    {
        private readonly ApplicationDbContext _db;

        public AccessRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public List<AspNetRole> GetAllAccountTypes()
        {
            return _db.AspNetRoles.ToList();
        }



    }
}
