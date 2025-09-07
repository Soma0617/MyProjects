using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        // 🔗 關聯訂單
        [Required]
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order OrderData { get; set; } = null!;

        [Display(Name = "付款方式")]
        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; } = null!;
        // e.g. "CreditCard", "LinePay", "ATM", "CashOnDelivery"

        [Display(Name = "付款金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "付款金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "付款日期")]
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Display(Name = "付款狀態")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        // e.g. "Pending", "Paid", "Failed", "Refunded"

        [Display(Name = "交易編號")]
        [StringLength(50)]
        public string? TransactionID { get; set; }
        // 第三方支付或銀行交易編號（可選）

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
