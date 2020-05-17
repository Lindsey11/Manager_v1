using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.Model.Model
{
    [Table("Product")]

    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        public string ProductCode { get; set; }
        [Required]
        public string Product { get; set; }
        public string Description { get; set; }
        [Required]
        public double UnitPrice { get; set; }
      

    }

    public class ProductForInvoice
    {
        [Key]
        public int Id { get; set; }
        public ProductModel Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }


        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
