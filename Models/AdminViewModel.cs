using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // Index 查詢/篩選/排序/分頁
    public class AdminQueryVm
    {
        /// <summary>關鍵字（名稱或 Email）</summary>
        public string? Q { get; set; }

        /// <summary>角色篩選（精準匹配）</summary>
        public string? Role { get; set; }

        /// <summary>啟用狀態篩選（true/false），不填 = 全部</summary>
        public bool? IsActive { get; set; }

        /// <summary>排序鍵：name / name_desc / email / email_desc / created / created_desc(預設)</summary>
        public string Sort { get; set; } = "created_desc";

        /// <summary>頁碼（從 1 開始）</summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數（5~50）</summary>
        [Range(5, 50)]
        public int PageSize { get; set; } = 10;
    }

    // Create 表單（可輸入密碼）
    public class AdminCreateVm
    {
        [Display(Name = "使用者名稱")]
        [Required, StringLength(50)]
        public string UserName { get; set; } = null!;

        [Display(Name = "電子郵件")]
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required, DataType(DataType.Password)]
        public string PasswordHash { get; set; } = null!; // 之後可改成 Hash

        [Display(Name = "員工職稱")]
        [Required, StringLength(20)]
        public string Role { get; set; } = "Staff";

        [Display(Name = "帳號是否啟用")]
        public bool IsActive { get; set; } = true;
    }

    // Edit 表單（不含密碼；密碼改用獨立「重設密碼」動作）
    public class AdminEditVm
    {
        [Required]
        public Guid AdminID { get; set; }

        [Display(Name = "使用者名稱")]
        [Required, StringLength(50)]
        public string UserName { get; set; } = null!;

        [Display(Name = "電子郵件")]
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Display(Name = "員工職稱")]
        [Required, StringLength(20)]
        public string Role { get; set; } = "Staff";

        [Display(Name = "帳號是否啟用")]
        public bool IsActive { get; set; } = true;
    }
}
