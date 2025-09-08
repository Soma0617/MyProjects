using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class AdminData
    {
        [Key]
        public Guid AdminID { get; set; } = Guid.NewGuid();

        [Display(Name = "使用者名稱")]
        [StringLength(50)]
        [Required]
        public string UserName { get; set; } = null!;

        [Display(Name = "電子郵件")]
        [StringLength(100)]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "角色")]
        [StringLength(20)]
        [Required]
        public string Role { get; set; } = "Staff";

        [Display(Name = "帳號是否啟用")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後登入時間")]
        public DateTime? LastLoginDate { get; set; }
    }

    public class MemberData
    {
        [Display(Name = "會員編號")]
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        public Guid MemberID { get; set; } = Guid.NewGuid();

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名為必填欄位")]
        [StringLength(20, ErrorMessage = "姓名最長 20 個字")]
        public string Name { get; set; } = null!;

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "必填欄位")]
        public DateTime Birthday { get; set; }

        [Display(Name = "性別")]
        public string? Sex { get; set; }

        [Display(Name = "電話號碼")]
        [Phone(ErrorMessage = "請輸入正確的電話號碼格式")]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Display(Name = "地址")]
        [StringLength(200, ErrorMessage = "地址最長 200 個字")]
        public string? Address { get; set; }

        [Display(Name = "Email 驗證狀態")]
        public bool IsEmailConfirmed { get; set; } = false;

        [Display(Name = "註冊日期")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後登入時間")]
        [DataType(DataType.DateTime)]
        public DateTime? LastLoginDate { get; set; }
    }

    public class AccountData
    {
        [Key]
        public int AccountID { get; set; }

        [Display(Name = "帳號 (Email)")]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "會員編號")]
        [Required]
        public Guid MemberID { get; set; }
    }

    public class OrderData
    {
        [Key]
        public Guid OrderID { get; set; } = Guid.NewGuid();

        [Display(Name = "訂單編號")]
        [StringLength(12, MinimumLength = 12)]
        [Required(ErrorMessage = "必填欄位")]
        public string OrderNumber { get; set; } = null!;

        [Required]
        public Guid MemberID { get; set; }

        [Display(Name = "建立時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name = "收件人姓名")]
        [StringLength(20)]
        [Required(ErrorMessage = "必填欄位")]
        public string RecipientName { get; set; } = null!;

        [Display(Name = "收件人電話")]
        [StringLength(15)]
        [Required(ErrorMessage = "必填欄位")]
        [DataType(DataType.PhoneNumber)]
        public string RecipientPhone { get; set; } = null!;

        [Display(Name = "收件地址")]
        [StringLength(100)]
        [Required(ErrorMessage = "必填欄位")]
        public string RecipientAddress { get; set; } = null!;

        [Display(Name = "付款狀態")]
        [Required]
        public string PaymentStatus { get; set; } = "Unpaid";

        [Display(Name = "付款方式")]
        [StringLength(20)]
        public string? PaymentMethod { get; set; }

        [Display(Name = "出貨狀態")]
        [Required]
        public string ShippingStatus { get; set; } = "Pending";

        [Display(Name = "訂單狀態")]
        [Required]
        public string OrderStatus { get; set; } = "Active";
    }

    public class ProductData
    {
        [Key]
        public Guid ProductID { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string ProductCode { get; set; } = null!;

        [Display(Name = "產品名稱")]
        [StringLength(20, ErrorMessage = "最大字數20個字")]
        [Required(ErrorMessage = "必填欄位")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "產品描述")]
        [StringLength(200, ErrorMessage = "最大字數200個字")]
        [Required(ErrorMessage = "必填欄位")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = null!;

        [Display(Name = "產品圖片")]
        [StringLength(200)]
        public string? Photo { get; set; }

        [Display(Name = "產品價格")]
        [Range(0.01, double.MaxValue, ErrorMessage = "價格需大於0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "是否上架")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "發布時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public int CategoryID { get; set; }
    }
}
