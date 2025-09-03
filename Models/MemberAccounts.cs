using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class MemberAccounts
    {
        [Key]
        public int AccountID { get; set; }

        [Display(Name = "帳號( Email )")]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required]
        public string Password { get; set; } = null!;

        [Display(Name = "會員編號")]
        [StringLength(36, MinimumLength = 36)]
        [HiddenInput]
        public int MemberID { get; set; }

        // 一對一：帳號對應回會員
        public Members MemberInfo { get; set; } = null!;
    }
}
