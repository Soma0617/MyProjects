using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Inventory
    {
        [Key]
        public Guid InventoryID { get; set; } = Guid.NewGuid();

        // 🔗 對應產品
        [Required]
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product ProductInfo { get; set; } = null!;

        [Display(Name = "庫存數量")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存數量不能小於0")]
        public int Quantity { get; set; }

        [Display(Name = "安全庫存量")]
        [Range(0, int.MaxValue, ErrorMessage = "安全庫存量不能小於0")]
        public int SafetyStock { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
