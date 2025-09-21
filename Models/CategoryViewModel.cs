using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class CategoryQueryVm
    {
        public string? Q { get; set; }  // 關鍵字
        public string Sort { get; set; } = "name";

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(5, 50)]
        public int PageSize { get; set; } = 10;
    }

    // 建立用
    public class CategoryCreateVm
    {
        [Required, StringLength(50, ErrorMessage = "分類名稱最長 50 個字")]
        public string CategoryName { get; set; } = null!;

        [Required, StringLength(10, MinimumLength = 1, ErrorMessage = "分類代碼需 1~10 個字")]
        public string CategoryCode { get; set; } = null!;
    }

    // 編輯用
    public class CategoryEditVm : CategoryCreateVm
    {
        [Key]
        public int CategoryID { get; set; }
    }
}
