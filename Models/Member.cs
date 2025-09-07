using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Member
    {
        [Display(Name = "會員編號")]
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput] 
        public Guid MemberID { get; set; }

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

        // 一對一：一個會員對應一個帳號
        public Account AccountData { get; set; } = null!;
    }
}
