using System;

namespace Authentication.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}