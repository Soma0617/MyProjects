using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class PurchaseOrder
    {
        public Guid PurchaseOrderID { get; set; } = Guid.NewGuid();

        public string PurchaseOrderCode { get; set; } = null!;

        public string SupplierName { get; set; } = null!;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
