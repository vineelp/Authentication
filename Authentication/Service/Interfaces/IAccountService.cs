using Authentication.DAL;
using System;
using System.Collections.Generic;

namespace Authentication.Service.Interfaces
{
    public interface IAccountService: IDisposable
    {
        bool IsActiveUser(string userName);
        bool EditUser(User user);
        bool DeleteUser(int userId);
        User GetUser(int userId);
        List<User> GetUsers(string managerName, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords);
    }
}