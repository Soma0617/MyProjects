using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Members
    {
        [Display(Name = "會員編號")]
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        public string MemberID { get; set; } = null!;

        [Display(Name = "會員名稱")]
        [StringLength(20, ErrorMessage = "名稱最長20個字")]
        [Required(ErrorMessage = "必填欄位")]
        public string Name { get; set; } = null!;

        [Display(Name = "電話號碼")]
        [Phone(ErrorMessage = "請輸入正確的電話號碼格式")]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "必填欄位")]
        public DateTime Birthday { get; set; }

        [Display(Name = "性別")]
        public string? Sex { get; set; }

        [Display(Name = "居住城市")]
        [StringLength(10, ErrorMessage = "城市名稱請勿超過10個字")]
        [Required(ErrorMessage = "必填欄位")]
        public string City { get; set; } = null!;

        [Display(Name = "居住地址")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "請輸入完整的居住地址")]
        [Required(ErrorMessage = "必填欄位")]
        public string Address { get; set; } = null!;


    }
}
