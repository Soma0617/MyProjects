using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class PurchaseOrderDetail
    {
        [Key]
        public Guid PurchaseOrderDetailID { get; set; } = Guid.NewGuid();

        // 🔗 對應主檔
        [Required]
        public Guid PurchaseOrderID { get; set; }
        [ForeignKey(nameof(PurchaseOrderID))]
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        // 🔗 對應產品
        [Required]
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product ProductData { get; set; } = null!;

        [Display(Name = "採購數量")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        public int Quantity { get; set; }

        [Display(Name = "單價")]
        [Range(0.01, double.MaxValue, ErrorMessage = "單價必須大於0")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "小計")]
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
