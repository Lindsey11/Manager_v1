using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Invoice.WebUI.ViewModel;
using Newtonsoft.Json;

namespace Invoice.WebUI.Helper
{
    public class InvoiceService
    {
        public IList<ProductList> Add(ProductList product, HttpContextBase httpContext)
        {
            var productList = ProductListFromCookie(httpContext);
            productList.Add(product);
            var cookie =UpdateCookieValue(httpContext, productList);

            return GetProducts(cookie);
        }
        public IList<ProductList> Remove(int id, HttpContextBase httpContext)
        {
            var productList = ProductListFromCookie(httpContext);
            var product = productList.FirstOrDefault(x => x.ProductId == id);
            if (product != null)
                productList.Remove(product); 
          var cookie=  UpdateCookieValue(httpContext, productList);

            return GetProducts(cookie);
        }
        public void Clear(HttpContextBase httpContext)
        {
            UpdateCookieValue(httpContext,new List<ProductList>());
        }
        public IEnumerable<ProductList> ProductList(HttpContextBase httpContext)
        {
            return ProductListFromCookie(httpContext);
        }

        private HttpCookie GetInvoiceCookie(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request == null ? null : httpContext.Request.Cookies["ProductList"];
            if (cookie == null)
            {
                CreateInvoiceCookie(httpContext);
                cookie = httpContext.Request.Cookies["ProductList"];   
            }

             
            if (cookie.Expires <= DateTime.Today) cookie.Expires = DateTime.Today.AddDays(7); 
            return cookie;
        }

        private void CreateInvoiceCookie(HttpContextBase httpContext)
        {
           var invoiceCookie = new HttpCookie("ProductList");
           httpContext.Request.Cookies.Add(invoiceCookie);
        }
        private IList<ProductList> ProductListFromCookie(HttpContextBase httpContext)
        {
            var cookie = GetInvoiceCookie(httpContext) ?? GetInvoiceCookie(httpContext);
            return GetProducts(cookie);
        }

        private  IList<ProductList> GetProducts(HttpCookie cookie)
        {
            
            var productListString = cookie.Value;
            if (string.IsNullOrEmpty(productListString)) return new List<ProductList>();
            var prodcuts = JsonConvert.DeserializeObject<IList<ProductList>>(productListString);
            if (prodcuts == null) return new List<ProductList>();
            return prodcuts;
        }

        private HttpCookie UpdateCookieValue(HttpContextBase httpContext, IEnumerable<ProductList> products)
        {
            var cookie = GetInvoiceCookie(httpContext);
            cookie.Value = JsonConvert.SerializeObject(products); 
            httpContext.Response.SetCookie(cookie);
            return cookie;
        }
    }
}
