using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class OrderDetailQueryVm
    {
        public Guid? OrderID { get; set; }
        public Guid? ProductID { get; set; }
    }

    // 建立用
    public class OrderDetailCreateVm
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "單價必須大於0")]
        public decimal UnitPrice { get; set; }
    }

    // 編輯用
    public class OrderDetailEditVm : OrderDetailCreateVm
    {
        [Key]
        public Guid OrderDetailID { get; set; }
    }
}
