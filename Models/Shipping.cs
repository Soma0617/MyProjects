using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Shipping
    {
        [Key]
        public Guid ShippingID { get; set; } = Guid.NewGuid();

        // 🔗 關聯訂單
        [Required]
        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order OrderData { get; set; } = null!;

        [Display(Name = "物流單號")]
        [StringLength(30)]
        [Required]
        public string TrackingNumber { get; set; } = null!;

        [Display(Name = "物流公司")]
        [StringLength(50)]
        [Required]
        public string Carrier { get; set; } = null!;
        // e.g. 黑貓宅急便, 宅配通, DHL, FedEx

        [Display(Name = "收件人姓名")]
        [StringLength(50)]
        [Required]
        public string RecipientName { get; set; } = null!;

        [Display(Name = "收件地址")]
        [StringLength(200)]
        [Required]
        public string ShippingAddress { get; set; } = null!;

        [Display(Name = "收件電話")]
        [StringLength(20)]
        public string? RecipientPhone { get; set; }

        [Display(Name = "出貨日期")]
        [DataType(DataType.DateTime)]
        public DateTime ShippedDate { get; set; } = DateTime.Now;

        [Display(Name = "預計送達日期")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedArrivalDate { get; set; }

        [Display(Name = "實際送達日期")]
        [DataType(DataType.DateTime)]
        public DateTime? DeliveredDate { get; set; }

        [Display(Name = "出貨狀態")]
        [StringLength(20)]
        public string Status { get; set; } = "Processing";
        // e.g. "Processing", "Shipped", "InTransit", "Delivered", "Returned"

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
