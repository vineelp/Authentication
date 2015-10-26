using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Authentication.Domain.Entities;

namespace Authentication.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                return ApplicationName;
            }

            set
            {
                ApplicationName = value;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using(var context = new AuthenticationEntities())
            {
                User usr = context.Users.Where(u => u.UserName.Equals(username)).FirstOrDefault();
                if(usr != null)
                {
                    Role role = context.Roles.Where(r => r.RoleID == usr.RoleID).FirstOrDefault();
                    return new string[] { role.RoleName };
                }
                return new string[] { };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var context = new AuthenticationEntities())
            {
                User usr = context.Users.Where(u => u.UserName == username).FirstOrDefault();
                if(usr != null)
                {
                    Role role = context.Roles.Where(r => r.RoleID == usr.RoleID).FirstOrDefault();
                    if (role.RoleName.Equals(roleName))
                        return true;
                }
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}