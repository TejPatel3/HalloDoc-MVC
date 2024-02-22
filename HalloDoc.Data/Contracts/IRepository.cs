using System.Linq.Expressions;

namespace Services.Contracts
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        void Add(T entity);

        void Update(T entity);
        void Remove(T entity);

        T GetFirst(Expression<Func<T, bool>> filter);
    }
}
