using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 列表 / 查詢 / 分頁
    public class EmailVerificationQueryVm
    {
        /// <summary>關鍵字（驗證碼 / 會員姓名 / 會員 Email）</summary>
        public string? Q { get; set; }

        /// <summary>是否已驗證（true/false），不填 = 全部</summary>
        public bool? IsVerified { get; set; }

        /// <summary>到期日(起)</summary>
        [DataType(DataType.Date)]
        public DateTime? ExpireFrom { get; set; }

        /// <summary>到期日(迄)</summary>
        [DataType(DataType.Date)]
        public DateTime? ExpireTo { get; set; }

        /// <summary>排序鍵：created / created_desc / expire / expire_desc / status / status_desc</summary>
        public string Sort { get; set; } = "created_desc";

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(5, 50)]
        public int PageSize { get; set; } = 10;
    }

    // 建立
    public class EmailVerificationCreateVm
    {
        [Display(Name = "會員")]
        [Required]
        public Guid MemberID { get; set; }

        [Display(Name = "驗證碼")]
        [Required, StringLength(100)]
        public string VerificationCode { get; set; } = null!;

        [Display(Name = "到期時間")]
        [Required, DataType(DataType.DateTime)]
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddMinutes(30);

        [Display(Name = "是否已驗證")]
        public bool IsVerified { get; set; } = false;
    }

    // 編輯
    public class EmailVerificationEditVm
    {
        [Required]
        public Guid VerificationID { get; set; }

        [Display(Name = "會員")]
        [Required]
        public Guid MemberID { get; set; }

        [Display(Name = "驗證碼")]
        [Required, StringLength(100)]
        public string VerificationCode { get; set; } = null!;

        [Display(Name = "到期時間")]
        [Required, DataType(DataType.DateTime)]
        public DateTime ExpirationTime { get; set; }

        [Display(Name = "是否已驗證")]
        public bool IsVerified { get; set; }
    }
}
