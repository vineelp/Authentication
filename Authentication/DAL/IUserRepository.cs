using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public interface IUserRepository
    {
        IQueryable<User> Get();
        User Get(object id);
        void Insert(User user);
        void Update(User user);
        void Delete(object id);
    }
}