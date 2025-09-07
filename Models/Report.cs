using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Report
    {
        [Key]
        public Guid ReportID { get; set; } = Guid.NewGuid();

        [Display(Name = "報表名稱")]
        [StringLength(50)]
        [Required]
        public string ReportName { get; set; } = null!;

        [Display(Name = "報表類型")]
        [StringLength(20)]
        [Required]
        public string ReportType { get; set; } = null!;
        // e.g. "Order", "Inventory", "Finance"

        [Display(Name = "內容")]
        [Required]
        public string Content { get; set; } = null!;
        // 可以存 JSON / HTML / Text 格式的報表內容

        [Display(Name = "建立人員")]
        [StringLength(50)]
        public string CreatedBy { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
