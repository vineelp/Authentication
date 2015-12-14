﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Authentication.Models;
using Authentication.Service.Interfaces;
using Authentication.DAL;
using AutoMapper;

namespace Authentication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService mAccountService;

        public AccountController(IAccountService accountService)
        {
            mAccountService = accountService;
        }

        protected override void Dispose(bool disposing)
        {
            mAccountService.Dispose();
            base.Dispose(disposing);
        }

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
                bool activeUser = mAccountService.IsActiveUser(model.UserName);
                if (activeUser)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User is not active");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username/Password");
            }
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
            int totalPages, totalRecords;
            string managerName = null;
            if (Roles.IsUserInRole("MANAGER"))
                managerName = User.Identity.Name;
            List<User> filteredUsers = mAccountService.GetUsers(managerName, sidx, sord, page, rows, userName, firstName, lastName, out totalPages, out totalRecords);
            List<UserViewModel> filteredUserViewModels = Mapper.Map<List<User>, List<UserViewModel>>(filteredUsers);
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            List<int> locations = new List<int>();

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = filteredUserViewModels
            };
            JsonResult result = Json(jsonData, JsonRequestBehavior.AllowGet);
            return result;
        }

        public string EditUser(UserViewModel userViewModel)
        {
            string msg;
            if (ModelState.IsValid)
            {
                User user = mAccountService.GetUser(userViewModel.UserID);
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.EmailAddress = userViewModel.EmailAddress;
                user.Password = userViewModel.Password;
                user.Active = userViewModel.Active;
                bool success = mAccountService.EditUser(user);
                if (success)
                    msg = "Saved Successfully";
                else
                    msg = "Error updating user";
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

            if (mAccountService.DeleteUser(id))
                msg = "Deleted Successfully";
            else
                msg = "Error deleting user";

            return msg;
        }
    }
}