
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuthenticationEntities authContext;
        private Dictionary<Type, object> repositories;
        private bool disposed;

        public UnitOfWork()
        {
            authContext = new AuthenticationEntities();
            repositories = new Dictionary<Type, object>();
            disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if(repositories.Keys.Contains(typeof(TEntity)))
            {
                return repositories[typeof(TEntity)] as IRepository<TEntity>;
            }
            var repository = new Repository<TEntity>(authContext);
            repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    authContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void SaveChanges()
        {
            authContext.SaveChanges();
        }
    }
}