using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AuthenticationEntities context;
        private DbSet<T> table = null;
        public GenericRepository(AuthenticationEntities context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public void Delete(object id)
        {
            T obj = table.Find(id);
            table.Remove(obj);
        }

        public IQueryable<T> Get()
        {
            return table;
        }

        public T Get(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }
    }
}