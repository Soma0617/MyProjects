using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class OrderQueryVm
    {
        public string? Q { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ShippingStatus { get; set; }
        public string? OrderStatus { get; set; }
        [DataType(DataType.Date)] public DateTime? CreatedFrom { get; set; }
        [DataType(DataType.Date)] public DateTime? CreatedTo { get; set; }
        public string Sort { get; set; } = "created_desc";
        [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
        [Range(5, 50)] public int PageSize { get; set; } = 10;
    }

    public class OrderCreateVm
    {
        [Required, StringLength(12, MinimumLength = 12)]
        public string OrderNumber { get; set; } = DateTime.Now.ToString("yyyyMMddHHmm");
        [Required] public Guid MemberID { get; set; }
        [Required, StringLength(20)] public string RecipientName { get; set; } = null!;
        [Required, StringLength(15)] public string RecipientPhone { get; set; } = null!;
        [Required, StringLength(100)] public string RecipientAddress { get; set; } = null!;
        [Required] public string PaymentStatus { get; set; } = "Unpaid";
        public string? PaymentMethod { get; set; }
        [Required] public string ShippingStatus { get; set; } = "Pending";
        [Required] public string OrderStatus { get; set; } = "Active";
    }

    public class OrderEditVm : OrderCreateVm
    {
        [Required] public Guid OrderID { get; set; }
    }
}
