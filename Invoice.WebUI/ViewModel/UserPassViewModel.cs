using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Invoice.WebUI.ViewModel
{
    public class UserPassViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}