using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class CheckoutViewModel
    {
        [Display(Name = "收件人"), Required, StringLength(20)]
        public string RecipientName { get; set; } = null!;

        [Display(Name = "手機"), Required, StringLength(15)]
        public string RecipientPhone { get; set; } = null!;

        [Display(Name = "地址"), Required, StringLength(100)]
        public string RecipientAddress { get; set; } = null!;

        [Display(Name = "付款方式"), Required, StringLength(20)]
        public string PaymentMethod { get; set; } = "CreditCard"; // CreditCard, ATM, COD…
    }
}

