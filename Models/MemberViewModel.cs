using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 列表 / 查詢 / 分頁參數
    public class MemberQueryVm
    {
        /// <summary>關鍵字（姓名 / Email / 電話）</summary>
        public string? Q { get; set; }

        /// <summary>Email 驗證狀態（true/false），不填 = 全部</summary>
        public bool? IsEmailConfirmed { get; set; }

        /// <summary>排序鍵：name / name_desc / email / email_desc / created / created_desc(預設)</summary>
        public string Sort { get; set; } = "created_desc";

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(5, 50)]
        public int PageSize { get; set; } = 10;
    }

    // 建立會員
    public class MemberCreateVm
    {
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名為必填")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Display(Name = "生日")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "性別")]
        [StringLength(5)]
        public string? Sex { get; set; }

        [Display(Name = "電話號碼")]
        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Display(Name = "地址")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Display(Name = "電子郵件")]
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Display(Name = "Email 是否驗證")]
        public bool IsEmailConfirmed { get; set; } = false;
    }

    // 編輯會員
    public class MemberEditVm
    {
        [Required]
        public Guid MemberID { get; set; }

        [Display(Name = "姓名")]
        [Required, StringLength(20)]
        public string Name { get; set; } = null!;

        [Display(Name = "生日")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "性別")]
        [StringLength(5)]
        public string? Sex { get; set; }

        [Display(Name = "電話號碼")]
        [Phone, StringLength(15)]
        public string? Phone { get; set; }

        [Display(Name = "地址")]
        [StringLength(200)]
        public string? Address { get; set; }

        [Display(Name = "電子郵件")]
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Display(Name = "Email 是否驗證")]
        public bool IsEmailConfirmed { get; set; }
    }
}
