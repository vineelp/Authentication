using System;

namespace Authentication.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<MLocation> MLocationRepository { get; }
        void SaveChanges();
    }
}