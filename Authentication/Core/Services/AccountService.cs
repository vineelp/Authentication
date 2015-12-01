using Authentication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Authentication.Models;
using Authentication.Infrastructure;

namespace Authentication.Core.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository mAccountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            mAccountRepository = accountRepository;
        }

        public bool DeleteUser(int userID)
        {
            return mAccountRepository.DeleteUser(userID);
        }

        public bool EditUser(UserViewModel user)
        {
            User userToEdit = mAccountRepository.GetUser(user.UserID);
            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.EmailAddress = user.EmailAddress;
            userToEdit.Password = user.Password;
            userToEdit.Active = user.Active;
            return mAccountRepository.EditUser(userToEdit);
        }

        public UserViewModel[] GetUsers(string managerName, string sidx, string sord, int page, int rows, string userName, string firstName, string lastName, out int totalPages, out int totalRecords)
        {
            List<int> managerLocations = null;
            if (string.IsNullOrEmpty(managerName))
            {
                managerLocations = mAccountRepository.GetManagerLocations(managerName);
            }
            Func<User, bool> filter = (e =>
            {
                bool filterPassed = true;
                if (managerLocations != null)
                    filterPassed = managerLocations.Contains(e.LocationID.Value);
                if (!string.IsNullOrEmpty(userName))
                    filterPassed = e.UserName.Contains(userName);
                if (!string.IsNullOrEmpty(firstName))
                    filterPassed = e.UserName.Contains(firstName);
                if (!string.IsNullOrEmpty(lastName))
                    filterPassed = e.UserName.Contains(lastName);
                return filterPassed;
            });
            //List <User> users = mAccountRepository.GetUsers(sord, page, rows, out totalPages, out totalRecords, filter);
            List<User> users = mAccountRepository.GetUsers(managerLocations, sidx, sord, page, rows, userName, firstName, lastName, out totalPages, out totalRecords);

            UserViewModel[] userViewModels = users.Select(
                        u => new UserViewModel
                        {
                            UserID = u.UserID,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            UserName = u.UserName,
                            EmailAddress = u.EmailAddress,
                            Password = u.Password,
                            RoleID = u.RoleID,
                            LocationID = u.LocationID,
                            Active = u.Active
                        }).ToArray();

            return userViewModels;
        }

        public bool IsActiveUser(string userName)
        {
            bool activeUser = false;
            activeUser = mAccountRepository.IsActiveUser(userName);
            return activeUser;
        }
    }
}