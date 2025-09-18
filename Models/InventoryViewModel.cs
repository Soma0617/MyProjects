using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用：Index 篩選
    public class InventoryQueryVm
    {
        public Guid? ProductID { get; set; }

        [Display(Name = "最少數量")]
        public int? MinQty { get; set; }

        [Display(Name = "最多數量")]
        public int? MaxQty { get; set; }
    }

    // 新增用
    public class InventoryCreateVm
    {
        [Required]
        [Display(Name = "產品")]
        public Guid ProductID { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "庫存數量")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "安全庫存量")]
        public int SafetyStock { get; set; }
    }

    // 修改用
    public class InventoryEditVm : InventoryCreateVm
    {
        [Key]
        public Guid InventoryID { get; set; }
    }

    // 詳細頁
    public class InventoryDetailsVm
    {
        public Guid InventoryID { get; set; }

        [Display(Name = "產品名稱")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "庫存數量")]
        public int Quantity { get; set; }

        [Display(Name = "安全庫存量")]
        public int SafetyStock { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime LastUpdated { get; set; }
    }
}
