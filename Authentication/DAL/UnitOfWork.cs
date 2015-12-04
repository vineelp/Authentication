using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public class UnitOfWork : IDisposable
    {
        private AuthenticationEntities authContext = new AuthenticationEntities();
        private IUserRepository userRepository;
        private IMLocationRepository mlocationRepository;

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(authContext);
                return userRepository;
            }
        }

        public IMLocationRepository MLocationRepository
        {
            get
            {
                if (mlocationRepository == null)
                    mlocationRepository = new MLocationRepository(authContext);
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