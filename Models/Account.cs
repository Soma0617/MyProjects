using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Account
    {
        [Key]
        public string AccountID { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public Guid MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member MemberInfo { get; set; } = null!;
    }
}
