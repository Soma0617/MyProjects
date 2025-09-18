using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class ReportQueryVm
    {
        [Display(Name = "報表名稱")]
        public string? ReportName { get; set; }

        [Display(Name = "報表類型")]
        public string? ReportType { get; set; }
    }

    // 新增用
    public class ReportCreateVm
    {
        [Required, StringLength(50)]
        [Display(Name = "報表名稱")]
        public string ReportName { get; set; } = null!;

        [Required, StringLength(20)]
        [Display(Name = "報表類型")]
        public string ReportType { get; set; } = null!;

        [Required]
        [Display(Name = "內容")]
        public string Content { get; set; } = null!;

        [Required, StringLength(50)]
        [Display(Name = "建立人員")]
        public string CreatedBy { get; set; } = null!;
    }

    // 編輯用
    public class ReportEditVm : ReportCreateVm
    {
        [Key]
        public Guid ReportID { get; set; }
    }
}
