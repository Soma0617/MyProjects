using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Report
    {
        [Key]
        public Guid ReportID { get; set; } = Guid.NewGuid();

        public string ReportName { get; set; } = null!;

        public string ReportType { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
