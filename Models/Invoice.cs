using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Invoice
    {
        public Guid InvoiceID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order OrderInfo { get; set; } = null!;

        public string InvoiceNumber { get; set; } = null!;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public decimal Amount { get; set; }

        public decimal Tax { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
