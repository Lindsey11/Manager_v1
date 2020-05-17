using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Invoice.Model.DataContext;

namespace Invoice.WebUI.ViewModel
{
    public class DashBoardViewModel
    {
        private InvoiceDataContext db;
        public DashBoardViewModel()
        {
            db = new InvoiceDataContext();
        }
        public int TotalCustomer
        {
         get {return db.Customer.Count(); }
        }

        public int TotalProduct
        {
            get { return db.Products.Count(); }
        }

        public int TotalInvoices
        {
            get { return db.Invoices.Count(); }
        }

        public int TotalUser
        {
            get { return db.User.Count(); }
        }
    }
}