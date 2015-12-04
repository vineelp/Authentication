using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public class UnitOfWork : IDisposable
    {
        private AuthenticationEntities authContext = new AuthenticationEntities();
        private IGenericRepository<User> userRepository;
        private IGenericRepository<MLocation> mlocationRepository;

        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new GenericRepository<User>(authContext);
                return userRepository;
            }
        }

        public IGenericRepository<MLocation> MLocationRepository
        {
            get
            {
                if (mlocationRepository == null)
                    mlocationRepository = new GenericRepository<MLocation>(authContext);
                return mlocationRepository;
            }
        }

        public void Save()
        {
            authContext.SaveChanges();
        }

        public void Dispose()
        {
            authContext.Dispose();
        }
    }
}