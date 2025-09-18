using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class InvoiceQueryVm
    {
        public Guid? OrderID { get; set; }
        public string? InvoiceNumber { get; set; }
    }

    public class InvoiceCreateVm
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required, StringLength(20)]
        public string InvoiceNumber { get; set; } = null!;

        [Required, DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Tax { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceEditVm : InvoiceCreateVm
    {
        [Key]
        public Guid InvoiceID { get; set; }
    }
}
