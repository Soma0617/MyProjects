using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public Guid MemberID { get; set; }

        public Member MemberInfo { get; set; } = null!;
    }
}
