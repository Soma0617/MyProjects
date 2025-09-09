using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class PurchaseOrderDetail
    {
        public Guid PurchaseOrderDetailID { get; set; } = Guid.NewGuid();

        public Guid PurchaseOrderID { get; set; }
        [ForeignKey("PurchaseOrderID")]
        public PurchaseOrder PurchaseOrderInfo { get; set; } = null!;

        public Guid ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product ProductInfo { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
    }
}
