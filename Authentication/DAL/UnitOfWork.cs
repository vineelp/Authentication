using System;

namespace Authentication.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuthenticationEntities authContext = new AuthenticationEntities();
        private IRepository<User> userRepository;
        private IRepository<MLocation> mlocationRepository;
        private bool disposed = false;

        public IRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<User>(authContext);
                return userRepository;
            }
        }

        public IRepository<MLocation> MLocationRepository
        {
            get
            {
                if (mlocationRepository == null)
                    mlocationRepository = new Repository<MLocation>(authContext);
                return mlocationRepository;
            }
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