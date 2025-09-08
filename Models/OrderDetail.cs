using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }
        public Order OrderInfo { get; set; } = null!;

        public Guid ProductID { get; set; }
        public Product ProductInfo { get; set; } = null!;

        [Display(Name = "數量")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "單價")]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Display(Name = "小計")]
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
