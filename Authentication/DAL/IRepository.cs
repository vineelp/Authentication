using System.Linq;

namespace Authentication.DAL
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        T Get(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
    }
}