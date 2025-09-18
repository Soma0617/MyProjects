using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 列表 / 查詢參數
    public class AccountQueryVm
    {
        public string? Q { get; set; } // 關鍵字（帳號或會員 Email）

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(5, 50)]
        public int PageSize { get; set; } = 10;

        public string Sort { get; set; } = "account"; // account / account_desc
    }

    // 建立帳號
    public class AccountCreateVm
    {
        [Display(Name = "帳號 ID")]
        [Required, StringLength(50)]
        public string AccountID { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required, DataType(DataType.Password)]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "會員編號")]
        [Required]
        public Guid MemberID { get; set; }
    }

    // 編輯帳號
    public class AccountEditVm
    {
        [Required]
        public string AccountID { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required, DataType(DataType.Password)]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "會員編號")]
        [Required]
        public Guid MemberID { get; set; }
    }
}
