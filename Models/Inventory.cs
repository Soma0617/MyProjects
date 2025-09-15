using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Inventory
    {
        [Key]
        public Guid InventoryID { get; set; } = Guid.NewGuid();

        public Guid ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product ProductInfo { get; set; } = null!;

        public int Quantity { get; set; }

        public int SafetyStock { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
