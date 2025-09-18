using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用：Index 篩選
    public class PurchaseOrderQueryVm
    {
        [Display(Name = "供應商名稱")]
        public string? SupplierName { get; set; }

        [Display(Name = "狀態")]
        public string? Status { get; set; }
    }

    // 新增用
    public class PurchaseOrderCreateVm
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "採購單號")]
        public string PurchaseOrderCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Display(Name = "供應商名稱")]
        public string SupplierName { get; set; } = null!;

        [StringLength(20)]
        [Display(Name = "狀態")]
        public string Status { get; set; } = "Pending";
    }

    // 修改用
    public class PurchaseOrderEditVm : PurchaseOrderCreateVm
    {
        [Key]
        public Guid PurchaseOrderID { get; set; }
    }

    // 詳細頁
    public class PurchaseOrderDetailsVm
    {
        public Guid PurchaseOrderID { get; set; }

        [Display(Name = "採購單號")]
        public string PurchaseOrderCode { get; set; } = null!;

        [Display(Name = "供應商名稱")]
        public string SupplierName { get; set; } = null!;

        [Display(Name = "狀態")]
        public string Status { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "更新時間")]
        public DateTime? UpdatedDate { get; set; }
    }
}
