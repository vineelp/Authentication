using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.Models;
using Authentication.DAL;
using Authentication.Service.Interfaces;

namespace Authentication.Service.Classes
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool DeleteUser(int userID)
        {
            unitOfWork.UserRepository.Delete(userID);
            unitOfWork.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public bool EditUser(UserViewModel userViewModel)
        {
            User user = unitOfWork.UserRepository.Get(userViewModel.UserID);
            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.EmailAddress = userViewModel.EmailAddress;
            user.Password = userViewModel.Password;
            user.Active = userViewModel.Active;
            unitOfWork.UserRepository.Update(user);
            unitOfWork.SaveChanges();
            return true;
        }

        public UserViewModel[] GetUsers(string managerName, string sidx, string sortOrder, int page, int pageSize, string userName, string firstName, string lastName, out int totalPages, out int totalRecords)
        {
            List<int> managerLocations = null;
            if (string.IsNullOrEmpty(managerName))
            {
                int managerID = unitOfWork.UserRepository.Get().FirstOrDefault(u => u.UserName.Equals(managerName)).UserID;
                managerLocations = unitOfWork.MLocationRepository.Get().Where(u => u.ManagerID == managerID).Select(u => u.LocationID).ToList();
            }
            IQueryable<User> userList = unitOfWork.UserRepository.Get();
            int pageIndex = Convert.ToInt32(page) - 1;
            if (managerLocations != null)
                userList = userList.Where(u => managerLocations.Contains(u.LocationID.Value));
            if (!string.IsNullOrEmpty(userName))
                userList = userList.Where(u => u.UserName.Contains(userName));
            if (!string.IsNullOrEmpty(firstName))
                userList = userList.Where(u => u.FirstName.Contains(firstName));
            if (!string.IsNullOrEmpty(lastName))
                userList = userList.Where(u => u.LastName.Contains(lastName));
            totalRecords = userList.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            if (sortOrder.ToUpper() == "DESC")
                userList = userList.OrderByDescending(s => s.UserName);
            else
                userList = userList.OrderBy(s => s.UserName);
            userList = userList.Skip(pageIndex * pageSize).Take(pageSize);

            List<User> filteredUsers = userList.ToList();

            UserViewModel[] userViewModels = filteredUsers.Select(
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
            activeUser = activeUser = unitOfWork.UserRepository.Get().Any(user => (user.UserName.Equals(userName) && user.Active.Equals("Y")));
            return activeUser;
        }
    }
}