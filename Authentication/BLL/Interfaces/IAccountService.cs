using Authentication.Models;
using System;

namespace Authentication.BLL.Interfaces
{
    public interface IAccountService: IDisposable
    {
        bool IsActiveUser(string userName);
        bool EditUser(UserViewModel user);
        bool DeleteUser(int userId);
        UserViewModel[] GetUsers(string managerName, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords);
    }
}