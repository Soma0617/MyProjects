using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用：Index 頁面篩選
    public class PaymentQueryVm
    {
        public Guid? OrderID { get; set; }

        [Display(Name = "付款狀態")]
        public string? Status { get; set; }

        [Display(Name = "付款方式")]
        public string? PaymentMethod { get; set; }
    }

    // 新增用
    public class PaymentCreateVm
    {
        [Required]
        [Display(Name = "訂單")]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage = "付款方式必填")]
        [StringLength(20)]
        [Display(Name = "付款方式")]
        public string PaymentMethod { get; set; } = null!;

        [Required(ErrorMessage = "付款金額必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額必須大於 0")]
        [Display(Name = "付款金額")]
        public decimal TotalAmount { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "付款日期")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        [Display(Name = "狀態")]
        public string Status { get; set; } = "Pending";

        [StringLength(50)]
        [Display(Name = "交易編號")]
        public string? TransactionID { get; set; }
    }

    // 修改用
    public class PaymentEditVm : PaymentCreateVm
    {
        [Key]
        public Guid PaymentID { get; set; }
    }

    // 詳細頁（Details 用）
    public class PaymentDetailsVm
    {
        public Guid PaymentID { get; set; }

        [Display(Name = "訂單編號")]
        public string OrderNumber { get; set; } = null!;

        [Display(Name = "付款方式")]
        public string PaymentMethod { get; set; } = null!;

        [Display(Name = "付款金額")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "付款日期")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "付款狀態")]
        public string Status { get; set; } = null!;

        [Display(Name = "交易編號")]
        public string? TransactionID { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; }
    }
}
