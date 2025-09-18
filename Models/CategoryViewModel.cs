using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class CategoryQueryVm
    {
        public string? Q { get; set; }  // 關鍵字
        public string Sort { get; set; } = "name";
    }

    // 建立用
    public class CategoryCreateVm
    {
        [Required, StringLength(50)]
        public string CategoryName { get; set; } = null!;

        [Required, StringLength(1)]
        public string CategoryCode { get; set; } = null!;
    }

    // 編輯用
    public class CategoryEditVm : CategoryCreateVm
    {
        [Key]
        public int CategoryID { get; set; }
    }
}
