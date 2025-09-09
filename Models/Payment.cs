using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Payment
    {
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order OrderInfo { get; set; } = null!;

        public string PaymentMethod { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public string? TransactionID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
