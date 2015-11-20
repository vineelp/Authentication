using Authentication.Infrastructure;
using Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.Core.Interfaces
{
    public interface IAccountService
    {
        bool IsActiveUser(string userName);
        bool EditUser(UserViewModel user);
        bool DeleteUser(int userId);
        UserViewModel[] GetUsers(string managerName, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords);
    }
}