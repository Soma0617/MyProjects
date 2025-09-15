using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order OrderInfo { get; set; } = null!;

        public Guid ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product ProductInfo { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
    }
}
