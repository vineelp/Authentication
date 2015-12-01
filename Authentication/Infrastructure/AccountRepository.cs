using Authentication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Authentication.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        public bool DeleteUser(int userId)
        {
            using (var dbContext = new AuthenticationEntities())
            {
                User user = dbContext.Users.Where(u => u.UserID == userId).FirstOrDefault();
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
            return true;
        }

        public bool EditUser(User user)
        {
            using (var dbContext = new AuthenticationEntities())
            {
                dbContext.Entry(user).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            return true;
        }

        public List<int> GetManagerLocations(string managerName)
        {
            using (var dbContext = new AuthenticationEntities())
            {
                int managerID = dbContext.Users.FirstOrDefault(u => u.UserName.Equals(managerName)).UserID;
                return dbContext.MLocations.Where(u => u.ManagerID == managerID).Select(u => u.LocationID).ToList();
            }
        }

        public User GetUser(int userId)
        {
            using (var dbContext = new AuthenticationEntities())
            {
                return dbContext.Users.Where(u => u.UserID == userId).FirstOrDefault();
            }
        }

        public List<User> GetUsers(string sortOrder, int page, int pageSize, out int totalPages, out int totalRecords, Func<User, bool> filter = null)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            using (var dbContext = new AuthenticationEntities())
            {
                var userListResults = dbContext.Users.Select(u => u).Where(filter);
                totalRecords = userListResults.Count();
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                if (sortOrder.ToUpper() == "DESC")
                    userListResults = userListResults.OrderByDescending(s => s.UserName);
                else
                    userListResults = userListResults.OrderBy(s => s.UserName);
                userListResults = userListResults.Skip(pageIndex * pageSize).Take(pageSize);
                //userListResults.Where(filter);
                return userListResults.ToList();
            }
        }

        public List<User> GetUsers(List<int> managerLocations, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            using (var dbContext = new AuthenticationEntities())
            {
                var userListResults = dbContext.Users.Select(u => u);
                if (managerLocations != null)
                    userListResults = userListResults.Where(u => managerLocations.Contains(u.LocationID.Value));
                if (!string.IsNullOrEmpty(userName))
                    userListResults = userListResults.Where(u => u.UserName.Contains(userName));
                if (!string.IsNullOrEmpty(firstName))
                    userListResults = userListResults.Where(u => u.FirstName.Contains(firstName));
                if (!string.IsNullOrEmpty(lastName))
                    userListResults = userListResults.Where(u => u.LastName.Contains(lastName));
                totalRecords = userListResults.Count();
                totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                if (sord.ToUpper() == "DESC")
                {
                    userListResults = userListResults.OrderByDescending(s => s.UserName);
                    userListResults = userListResults.Skip(pageIndex * pageSize).Take(pageSize);
                }
                else
                {
                    userListResults = userListResults.OrderBy(s => s.UserName);
                    userListResults = userListResults.Skip(pageIndex * pageSize).Take(pageSize);
                }

                return userListResults.ToList();
            }
        }

        public bool IsActiveUser(string userName)
        {
            bool activeUser = false;
            using (var ctxt = new AuthenticationEntities())
            {
                activeUser = ctxt.Users.Any(user => (user.UserName.Equals(userName) && user.Active.Equals("Y")));
            }
            return activeUser;
        }
    }
}