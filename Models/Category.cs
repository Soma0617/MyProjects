using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;

        [Required]
        [StringLength(1)]
        public string CategoryCode { get; set; } = null!;  // A, B, C...
    }
}
