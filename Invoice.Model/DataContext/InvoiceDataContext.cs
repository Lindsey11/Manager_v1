using System.Data.Entity;
using Invoice.Model.Model;

namespace Invoice.Model.DataContext
{
    public class InvoiceDataContext : DbContext
    {
        public InvoiceDataContext()
            : base("InvoiceDbContext")
        {
            
        }
        public DbSet<CompanyModel> Company { get; set; }
        public DbSet<CustomerModel> Customer { get; set; }
        public DbSet<ProductModel> Products{ get; set; }
        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<ProductForInvoice> ProductForInvoice { get; set; }
        public DbSet<UserModel> User { get; set; } 

       
    }
}
