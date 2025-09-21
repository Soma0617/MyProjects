// Models/AccountAuthViewModels.cs
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class LoginVm
    {
        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required, Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }

    public class RegisterVm
    {
        [Required, Display(Name = "姓名")]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone, Display(Name = "手機")]
        public string? Phone { get; set; }

        [Display(Name = "地址")]
        public string? Address { get; set; }

        [Required, Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "兩次密碼不一致")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
