using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; } = Guid.NewGuid();

        [Display(Name = "訂單編號")]
        [StringLength(12, MinimumLength = 12)]
        [Required(ErrorMessage = "必填欄位")]
        public string OrderNumber { get; set; } = null!;   // 系統自動產生 (例如 20250818-0001)

        // 關聯到會員
        [Required]
        public Guid MemberID { get; set; }
        public Member MemberData { get; set; } = null!;

        [Display(Name = "建立時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // 收件資訊
        [Display(Name = "收件人姓名")]
        [StringLength(20)]
        [Required(ErrorMessage = "必填欄位")]
        public string RecipientName { get; set; } = null!;

        [Display(Name = "收件人電話")]
        [StringLength(15)]
        [Required(ErrorMessage = "必填欄位")]
        [DataType(DataType.PhoneNumber)]
        public string RecipientPhone { get; set; } = null!;

        [Display(Name = "收件地址")]
        [StringLength(100)]
        [Required(ErrorMessage = "必填欄位")]
        public string RecipientAddress { get; set; } = null!;

        // 訂單狀態
        [Display(Name = "付款狀態")]
        [Required]
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Paid, Refunded

        [Display(Name = "付款方式")]
        [StringLength(20)]
        public string? PaymentMethod { get; set; } // CreditCard, ATM, COD

        [Display(Name = "出貨狀態")]
        [Required]
        public string ShippingStatus { get; set; } = "Pending"; // Pending, Shipped, Delivered

        [Display(Name = "訂單狀態")]
        [Required]
        public string OrderStatus { get; set; } = "Active"; // Active, Completed, Cancelled
    }
}
