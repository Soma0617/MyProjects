using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Admin
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

        [Display(Name = "密碼雜湊")]
        [Required]
        public string PasswordHash { get; set; } = null!;
        // 不存明碼，只存 Hash（如 SHA256 + Salt）

        [Display(Name = "角色")]
        [StringLength(20)]
        [Required]
        public string Role { get; set; } = "Staff";
        // e.g. "SuperAdmin", "Manager", "Staff"

        [Display(Name = "帳號是否啟用")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後登入時間")]
        public DateTime? LastLoginDate { get; set; }
    }
}
