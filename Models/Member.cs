using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Member
    {
        public Guid MemberID { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public string? Sex { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        // 一對一：一個會員對應一個帳號
        public Account AccountInfo { get; set; } = null!;
    }
}
