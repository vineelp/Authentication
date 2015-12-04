using System.Data.Entity;
using System.Linq;

namespace Authentication.DAL
{
    public class UserRepository : IUserRepository
    {
        private AuthenticationEntities context;
        public UserRepository(AuthenticationEntities context)
        {
            this.context = context;
        }

        public void Delete(object id)
        {
            User user = context.Users.Find(id);
            context.Users.Remove(user);
        }

        public IQueryable<User> Get()
        {
            return context.Users;
        }

        public User Get(object id)
        {
            return context.Users.Find(id);
        }

        public void Insert(User user)
        {
            context.Users.Add(user);
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}