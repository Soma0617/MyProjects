using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class EmailVerification
    {
        [Key]
        public Guid VerificationID { get; set; } = Guid.NewGuid();

        // 關聯到會員（外鍵）
        [Required]
        public Guid MemberID { get; set; }

        [ForeignKey("MemberID")]
        public Member MemberInfo { get; set; } = null!;

        // 驗證碼（通常用隨機字串或 GUID 轉換）
        [Required]
        [StringLength(100)]
        public string VerificationCode { get; set; } = null!;

        // 驗證碼有效期限
        [Required]
        public DateTime ExpirationTime { get; set; }

        // 是否已驗證
        [Required]
        public bool IsVerified { get; set; } = false;

        // 建立時間
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
