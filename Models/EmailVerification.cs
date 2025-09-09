using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class EmailVerification
    {
        public Guid VerificationID { get; set; } = Guid.NewGuid();

        public Guid MemberID { get; set; }

        [ForeignKey("MemberID")]
        public Member MemberInfo { get; set; } = null!;

        public string VerificationCode { get; set; } = null!;

        public DateTime ExpirationTime { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
