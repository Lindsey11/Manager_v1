using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Invoice.Model.Model;

namespace Invoice.WebUI.ViewModel
{
    public static class LoginUser
    {
        public static UserModel GetLogin()
        {
            var user = HttpContext.Current.Session["Loginuser"] as UserModel;
            return user;
        }
    }
}