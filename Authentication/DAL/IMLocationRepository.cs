using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public interface IMLocationRepository
    {
        IQueryable<MLocation> Get();
        User Get(object id);
        void Insert(MLocation user);
        void Update(MLocation user);
        void Delete(object id);
    }
}