// Models/CheckoutVm.cs
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class CheckoutVm
    {
        [Required, Display(Name = "收件人")]
        [StringLength(50)]
        public string RecipientName { get; set; } = null!;

        [Required, Display(Name = "收件人電話")]
        [StringLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, Display(Name = "收件地址")]
        [StringLength(200)]
        public string RecipientAddress { get; set; } = null!;

        [Required, Display(Name = "付款方式")]
        public string PaymentMethod { get; set; } = "CreditCard";
    }
}
