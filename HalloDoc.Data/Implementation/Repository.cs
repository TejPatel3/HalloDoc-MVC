using DataModels.DataContext;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Linq.Expressions;

namespace Services.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> Set;
        public Repository(ApplicationDbContext context)
        {
            _context = new ApplicationDbContext();
            this.Set = _context.Set<T>();
        }
        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = Set;
            return query.ToList();
        }

        public T GetFirst(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = Set;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Remove(T entity)
        {
            Set.Remove(entity);
        }
        public void Update(T entity)
        {
            Set.Update(entity);
        }
    }
}
