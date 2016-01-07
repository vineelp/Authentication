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
        private IRepository<User> userRepository;
        private IRepository<MLocation> mLocationRepository;
        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            userRepository = unitOfWork.GetRepository<User>();
            mLocationRepository = unitOfWork.GetRepository<MLocation>();
        }

        public bool DeleteUser(int userID)
        {
            userRepository.Delete(userID);
            unitOfWork.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public bool EditUser(User user)
        {
            userRepository.Update(user);
            unitOfWork.SaveChanges();
            return true;
        }

        public User GetUser(int userId)
        {
            return userRepository.Get(userId);
        }

        public List<User> GetUsers(string managerName, string sortIndex, string sortOrder, int page, int pageSize, string userName, string firstName, string lastName, out int totalPages, out int totalRecords)
        {
            List<int> managerLocations = null;
            if (!string.IsNullOrEmpty(managerName))
            {
                int managerID = userRepository.Get().FirstOrDefault(u => u.UserName.Equals(managerName)).UserID;
                managerLocations = mLocationRepository.Get().Where(u => u.ManagerID == managerID).Select(u => u.LocationID).ToList();
            }
            else
            {
                managerLocations = mLocationRepository.Get().Select(u => u.LocationID).ToList();
            }
            Expression<Func<User, bool>> filter = (u => (
                                                            (userName == null || u.UserName.Contains(userName)) &&
                                                            (firstName == null || u.FirstName.Contains(firstName)) &&
                                                            (lastName == null || u.LastName.Contains(lastName)) &&
                                                            managerLocations.Contains(u.LocationID.Value)
                                                            ));
            IQueryable<User> userList = userRepository.Get();
            int pageIndex = Convert.ToInt32(page) - 1;
            userList = userList.Where(filter);
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
            activeUser = activeUser = userRepository.Get().Any(user => (user.UserName.Equals(userName) && user.Active.Equals("Y")));
            return activeUser;
        }
    }
}