using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.Model.Model
{
    [Table("Invoice")]
    public class InvoiceModel
    {

        public InvoiceModel()
        {
            Products = new List<ProductForInvoice>();
        }
        [Key]
        public int Id { get; set; }
        public string InvoiceCode { get; set; }
        public string Status { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public double TotalAmount { get; set; }
        public double AmountPaid { get; set; }
        public double BalanceDue { get; set; }
        public string Notes { get; set; }

        [ForeignKey("CustomerModel")]
        public int CustomerId { get; set; }
        public CustomerModel CustomerModel { get; set; }
        public ICollection<ProductForInvoice> Products { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        public int? UserId { get; set; }

    }
}
