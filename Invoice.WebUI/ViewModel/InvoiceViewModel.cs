using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice.Model.Model;

namespace Invoice.WebUI.ViewModel
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
           
        }
        public InvoiceModel Invoice { get; set; }
        public IEnumerable<ProductList> ProductList { get; set; }
    }
}
