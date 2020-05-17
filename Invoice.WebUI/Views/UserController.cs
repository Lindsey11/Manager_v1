using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invoice.Model.DataContext;

namespace Invoice.WebUI.Views
{
    public class UserController : Controller
    {
        // GET: User

        private readonly InvoiceDataContext db;
        public UserController()
        {
            db = new InvoiceDataContext();
        }
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }


    }
}