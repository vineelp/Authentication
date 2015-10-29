using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Authentication.Domain.Entities;
using Authentication.Models;

namespace Authentication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("LoginError", "Invalid Username/Password");
            return View(model);
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
            Response.Redirect("/Account/Login");
        }

        [Authorize(Roles = "MANAGER, SUPERADMIN")]
        public ActionResult UserGrid()
        {
            return View();
        }

        public JsonResult GetUsersList(string sidx, string sord, int page, int rows, string userName, string firstName, string lastName)  //Gets the Users.
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            List<int> locations = new List<int>();
            
            using (var db = new AuthenticationEntities())
            {
                if (Roles.IsUserInRole("MANAGER"))
                {
                    int userID = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).UserID;
                    locations = db.MLocations.Where(u => u.ManagerID == userID).Select(u=>u.LocationID).ToList();
                }
                var userListResults = db.Users.Select(
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
                        });
                if (locations.Count > 0)
                    userListResults = userListResults.Where(u => locations.Contains(u.LocationID.Value));
                if(!string.IsNullOrEmpty(userName))
                    userListResults = userListResults.Where(u => u.UserName.Contains(userName));
                if (!string.IsNullOrEmpty(firstName))
                    userListResults = userListResults.Where(u => u.FirstName.Contains(firstName));
                if (!string.IsNullOrEmpty(lastName))
                    userListResults = userListResults.Where(u => u.LastName.Contains(lastName));
                int totalRecords = userListResults.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
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
                
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = userListResults.ToArray()
                };
                JsonResult result = Json(jsonData, JsonRequestBehavior.AllowGet);
                return result;
            }
        }

        public string EditUser(UserViewModel usr)
        {
            string msg;
            if (ModelState.IsValid)
            {
                using (var dbContext = new AuthenticationEntities())
                {
                    User userToEdit = dbContext.Users.FirstOrDefault(u => u.UserID == usr.UserID);
                    userToEdit.FirstName = usr.FirstName;
                    userToEdit.LastName = usr.LastName;
                    userToEdit.EmailAddress = usr.EmailAddress;
                    userToEdit.Password = usr.Password;
                    dbContext.SaveChanges();
                    msg = "Saved Successfully";
                }
            }
            else
            {
                if (ModelState["Password"].Errors.Count > 0)
                    msg = ModelState["Password"].Errors[0].ErrorMessage;
                else
                    msg = "Data validation is not successfull";
            }
            return msg;
        }

        public string DeleteUser(int id)
        {
            string msg;
            if (ModelState.IsValid)
            {
                using (var dbContext = new AuthenticationEntities())
                {
                    User userToDelete = dbContext.Users.FirstOrDefault(u => u.UserID == id);
                    if (userToDelete != null)
                    {
                        dbContext.Users.Remove(userToDelete);
                        dbContext.SaveChanges();
                    }
                    msg = "Deleted Successfully";
                }
            }
            else
            {
                msg = "Error deleting user";
            }
            return msg;
        }
    }
}