using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.Model.Model
{
    [Table("Customer")]
    public class CustomerModel
    {
        [Key]
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string Web { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Addess { get; set; }

    }
}
