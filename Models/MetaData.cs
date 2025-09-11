using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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

        [Display(Name = "員工職稱")]
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

        [Display(Name = "電子郵件")]
        [StringLength(100)]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

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
        [Display(Name = "帳號 (Email)")]
        [EmailAddress]
        [Required]
        public string AccountID { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required]
        [PasswordPropertyText]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "會員編號")]
        [Required]
        public Guid MemberID { get; set; }
    }

    public class EmailVerificationData
    {
        [Key]
        public Guid VerificationID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid MemberID { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationCode { get; set; } = null!;

        [Required]
        public DateTime ExpirationTime { get; set; } // DateTime.Now.AddSeconds(180)

        [Required]
        public bool IsVerified { get; set; } = false;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
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

    public class CategoryData
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;

        [Required]
        [StringLength(1)]
        public string CategoryCode { get; set; } = null!;
    }

    public class OrderDetailData
    {
        [Key]
        public Guid OrderDetailID { get; set; } = Guid.NewGuid();

        public Guid OrderID { get; set; }

        public Guid ProductID { get; set; }

        [Display(Name = "數量")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        [Required(ErrorMessage = "必填欄位")]
        public int Quantity { get; set; }

        [Display(Name = "單價")]
        [Range(0.01, double.MaxValue, ErrorMessage = "單價必須大於0")]
        [Required(ErrorMessage = "必填欄位")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "小計")]
        [Required]
        public decimal Subtotal => Quantity * UnitPrice;
    }

    public class ShippingData
    {
        [Key]
        public Guid ShippingID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderID { get; set; }

        [Display(Name = "物流單號")]
        [StringLength(30)]
        [Required]
        public string TrackingNumber { get; set; } = null!;

        [Display(Name = "物流公司")]
        [StringLength(50)]
        [Required]
        public string Carrier { get; set; } = null!;

        [Display(Name = "收件人姓名")]
        [StringLength(50)]
        [Required]
        public string RecipientName { get; set; } = null!;

        [Display(Name = "收件地址")]
        [StringLength(200)]
        [Required]
        public string ShippingAddress { get; set; } = null!;

        [Display(Name = "收件電話")]
        [StringLength(20)]
        public string? RecipientPhone { get; set; }

        [Display(Name = "出貨日期")]
        [DataType(DataType.DateTime)]
        public DateTime ShippedDate { get; set; } = DateTime.Now;

        [Display(Name = "預計送達日期")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedArrivalDate { get; set; }

        [Display(Name = "實際送達日期")]
        [DataType(DataType.DateTime)]
        public DateTime? DeliveredDate { get; set; }

        [Display(Name = "出貨狀態")]
        [StringLength(20)]
        public string Status { get; set; } = "Processing";

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class InvoiceData
    {
        [Key]
        public Guid InvoiceID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderID { get; set; }

        [Display(Name = "發票號碼")]
        [StringLength(20)]
        [Required(ErrorMessage = "必填欄位")]
        public string InvoiceNumber { get; set; } = null!;

        [Display(Name = "發票日期")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Display(Name = "金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "稅額")]
        [Range(0, double.MaxValue)]
        public decimal Tax { get; set; }

        [Display(Name = "總金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "總金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class PaymentData
    {
        [Key]
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderID { get; set; }

        [Display(Name = "付款方式")]
        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; } = null!;

        [Display(Name = "付款金額")]
        [Range(0.01, double.MaxValue, ErrorMessage = "付款金額需大於0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "付款日期")]
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Display(Name = "付款狀態")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "交易編號")]
        [StringLength(50)]
        public string? TransactionID { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class InventoryData
    {
        [Key]
        public Guid InventoryID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductID { get; set; }

        [Display(Name = "庫存數量")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存數量不能小於0")]
        public int Quantity { get; set; }

        [Display(Name = "安全庫存量")]
        [Range(0, int.MaxValue, ErrorMessage = "安全庫存量不能小於0")]
        public int SafetyStock { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class PurchaseOrderData
    {
        [Key]
        public Guid PurchaseOrderID { get; set; } = Guid.NewGuid();

        [Display(Name = "採購單號")]
        [Required]
        [StringLength(20)]
        public string PurchaseOrderCode { get; set; } = null!;

        [Display(Name = "供應商名稱")]
        [Required]
        [StringLength(50)]
        public string SupplierName { get; set; } = null!;

        [Display(Name = "狀態")]
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Display(Name = "建立時間")]
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "更新時間")]
        public DateTime? UpdatedDate { get; set; }
    }

    public class PurchaseOrderDetailData
    {
        [Key]
        public Guid PurchaseOrderDetailID { get; set; } = Guid.NewGuid();

        public Guid PurchaseOrderID { get; set; }

        public Guid ProductID { get; set; }

        [Display(Name = "數量")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        [Required(ErrorMessage = "必填欄位")]
        public int Quantity { get; set; }

        [Display(Name = "單價")]
        [Range(0.01, double.MaxValue, ErrorMessage = "單價必須大於0")]
        [Required(ErrorMessage = "必填欄位")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "小計")]
        [Required]
        public decimal Subtotal => Quantity * UnitPrice;
    }

    public class ReportData
    {
        [Key]
        public Guid ReportID { get; set; } = Guid.NewGuid();

        [Display(Name = "報表名稱")]
        [StringLength(50)]
        [Required]
        public string ReportName { get; set; } = null!;

        [Display(Name = "報表類型")]
        [StringLength(20)]
        [Required]
        public string ReportType { get; set; } = null!;

        [Display(Name = "內容")]
        [Required]
        public string Content { get; set; } = null!;

        [Display(Name = "建立人員")]
        [StringLength(50)]
        public string CreatedBy { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
