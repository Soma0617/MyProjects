using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Shipping
    {
        public Guid ShippingID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order OrderInfo { get; set; } = null!;

        public string TrackingNumber { get; set; } = null!;

        public string Carrier { get; set; } = null!;

        public string RecipientName { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public string? RecipientPhone { get; set; }

        public DateTime ShippedDate { get; set; } = DateTime.Now;

        public DateTime? EstimatedArrivalDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public string Status { get; set; } = "Processing";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
