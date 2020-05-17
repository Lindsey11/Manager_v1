using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Invoice.Model.Model;

namespace Invoice.WebUI.ViewModel
{
    public class InvoiceDetailsViewModel
    {
        public CompanyModel Company { get; set; }
        public InvoiceModel Invoice { get; set; }
    }
}