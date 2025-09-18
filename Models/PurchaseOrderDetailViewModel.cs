using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用：Index 篩選
    public class PurchaseOrderDetailQueryVm
    {
        [Display(Name = "採購單")]
        public Guid? PurchaseOrderID { get; set; }

        [Display(Name = "產品")]
        public Guid? ProductID { get; set; }
    }

    // 新增用
    public class PurchaseOrderDetailCreateVm
    {
        [Required]
        [Display(Name = "採購單")]
        public Guid PurchaseOrderID { get; set; }

        [Required]
        [Display(Name = "產品")]
        public Guid ProductID { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        [Display(Name = "數量")]
        public int Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "單價必須大於 0")]
        [Display(Name = "單價")]
        public decimal UnitPrice { get; set; }
    }

    // 編輯用
    public class PurchaseOrderDetailEditVm : PurchaseOrderDetailCreateVm
    {
        [Key]
        public Guid PurchaseOrderDetailID { get; set; }
    }
}
