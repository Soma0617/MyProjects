using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class PurchaseOrder
    {
        [Key]
        public Guid PurchaseOrderID { get; set; } = Guid.NewGuid();

        [Display(Name = "採購單號")]
        [Required]
        [StringLength(20)]
        public string PurchaseOrderCode { get; set; } = null!; // 例如：PO20250818001

        [Display(Name = "供應商名稱")]
        [Required]
        [StringLength(50)]
        public string SupplierName { get; set; } = null!;

        [Display(Name = "狀態")]
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        // 可能值：Pending, Approved, Rejected, Completed

        [Display(Name = "建立時間")]
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "更新時間")]
        public DateTime? UpdatedDate { get; set; }

        // 🔗 對應到明細
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
