using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Invoice
    {
        [Key]
        public Guid InvoiceID { get; set; } = Guid.NewGuid();

        // 🔗 關聯訂單
        [Required]
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order OrderData { get; set; } = null!;

        [Display(Name = "發票號碼")]
        [StringLength(20)]
        [Required(ErrorMessage = "必填欄位")]
        public string InvoiceNumber { get; set; } = null!;

        [Display(Name = "發票日期")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Display(Name = "金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "稅額")]
        [Range(0, double.MaxValue)]
        public decimal Tax { get; set; }

        [Display(Name = "總金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "總金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
