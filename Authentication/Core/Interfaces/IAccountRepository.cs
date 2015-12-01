using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Authentication.Core.Interfaces
{
    public interface IAccountRepository
    {
        bool IsActiveUser(string userName);
        bool EditUser(User user);
        User GetUser(int userId);
        bool DeleteUser(int userId);
        List<int> GetManagerLocations(string managerName);
        List<User> GetUsers(List<int> managerLocations, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords);
        List<User> GetUsers(string sortOrder, int page, int pageSize, out int totalPages, out int totalRecords, Func<User, bool> filter = null);
    }
}