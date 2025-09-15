using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminID { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "Staff";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }
    }
}
