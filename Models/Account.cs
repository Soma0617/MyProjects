using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Account
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

        // 對應到 Member
        [ForeignKey("MemberID")]
        public Member MemberData { get; set; } = null!;
    }
}
