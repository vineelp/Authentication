using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.DAL
{
    public class MLocationRepository : IMLocationRepository
    {
        private AuthenticationEntities context;

        public MLocationRepository(AuthenticationEntities context)
        {
            this.context = context;
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MLocation> Get()
        {
            return context.MLocations;
        }

        public User Get(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(MLocation user)
        {
            throw new NotImplementedException();
        }

        public void Update(MLocation user)
        {
            throw new NotImplementedException();
        }
    }
}