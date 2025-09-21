using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; } = Guid.NewGuid();

        public string OrderNumber { get; set; } = null!;

        public Guid MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member MemberInfo { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public string RecipientName { get; set; } = null!;

        public string RecipientPhone { get; set; } = null!;

        public string RecipientAddress { get; set; } = null!;

        public string PaymentStatus { get; set; } = "Unpaid";

        public string? PaymentMethod { get; set; }

        public string ShippingStatus { get; set; } = "Pending";

        public string OrderStatus { get; set; } = "Active";

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
