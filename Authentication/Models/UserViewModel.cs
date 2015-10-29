using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Authentication.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{4,15}$", 
            ErrorMessage = "password should contain atleast one small letter, one capital letter and a number")]
        public string Password { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string Active { get; set; }
    }
}