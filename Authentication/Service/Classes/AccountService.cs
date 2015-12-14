using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Authentication.DAL;
using Authentication.Service.Interfaces;
using System.Linq.Expressions;

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

        public bool EditUser(User user)
        {
            unitOfWork.UserRepository.Update(user);
            unitOfWork.SaveChanges();
            return true;
        }

        public User GetUser(int userId)
        {
            return unitOfWork.UserRepository.Get(userId);
        }

        public List<User> GetUsers(string managerName, string sortIndex, string sortOrder, int page, int pageSize, string userName, string firstName, string lastName, out int totalPages, out int totalRecords)
        {
            List<int> managerLocations = null;
            if (!string.IsNullOrEmpty(managerName))
            {
                int managerID = unitOfWork.UserRepository.Get().FirstOrDefault(u => u.UserName.Equals(managerName)).UserID;
                managerLocations = unitOfWork.MLocationRepository.Get().Where(u => u.ManagerID == managerID).Select(u => u.LocationID).ToList();
            }
            Expression<Func<User, bool>> usersFilter = (u => u.UserName.Contains(userName) && u.FirstName.Contains(firstName) && u.LastName.Contains(lastName));
            Expression<Func<User, bool>> managersFilter = null;
            if(managerLocations != null)
                managersFilter = (u => managerLocations.Contains(u.LocationID.Value));
            userName = userName ?? string.Empty;
            firstName = firstName ?? string.Empty;
            lastName = lastName ?? string.Empty;

            IQueryable<User> userList = unitOfWork.UserRepository.Get();
            int pageIndex = Convert.ToInt32(page) - 1;
            userList = userList.Where(usersFilter);
            if(managersFilter != null)
                userList = userList.Where(managersFilter);

            totalRecords = userList.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            if (!string.IsNullOrEmpty(sortIndex))
            {
                if (sortOrder.ToUpper() == "DESC")
                    userList = userList.OrderBy(sortIndex+ " descending");
                else
                    userList = userList.OrderBy(sortIndex);
            }
            else
                userList = userList.OrderBy(u => u.UserName);
            userList = userList.Skip(pageIndex * pageSize).Take(pageSize);

            return userList.ToList();
        }

        public bool IsActiveUser(string userName)
        {
            bool activeUser = false;
            activeUser = activeUser = unitOfWork.UserRepository.Get().Any(user => (user.UserName.Equals(userName) && user.Active.Equals("Y")));
            return activeUser;
        }
    }
}