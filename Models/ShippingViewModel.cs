using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class ShippingQueryVm
    {
        public Guid? OrderID { get; set; }
        public string? Status { get; set; }
    }

    // 建立用
    public class ShippingCreateVm
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required, StringLength(30)]
        public string TrackingNumber { get; set; } = null!;

        [Required, StringLength(50)]
        public string Carrier { get; set; } = null!;

        [Required, StringLength(50)]
        public string RecipientName { get; set; } = null!;

        [Required, StringLength(200)]
        public string ShippingAddress { get; set; } = null!;

        [StringLength(20)]
        public string? RecipientPhone { get; set; }

        public DateTime ShippedDate { get; set; } = DateTime.Now;
        public DateTime? EstimatedArrivalDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Processing";
    }

    // 編輯用
    public class ShippingEditVm : ShippingCreateVm
    {
        [Key]
        public Guid ShippingID { get; set; }
    }
}
