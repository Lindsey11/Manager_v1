using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.WebUI.ViewModel
{
    public class ProductList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }      
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
