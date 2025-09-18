using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 查詢用
    public class ProductQueryVm
    {
        public string? Q { get; set; }          // 關鍵字
        public int? CategoryID { get; set; }    // 分類篩選
        public bool? IsActive { get; set; }     // 是否上架

        public string Sort { get; set; } = "created_desc";

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(5, 50)]
        public int PageSize { get; set; } = 10;
    }

    // 建立用
    public class ProductCreateVm
    {
        [Required, StringLength(5, MinimumLength = 5)]
        public string ProductCode { get; set; } = null!;

        [Required, StringLength(20)]
        public string ProductName { get; set; } = null!;

        [Required, StringLength(200)]
        public string Description { get; set; } = null!;

        public string? Photo { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int CategoryID { get; set; }
    }

    // 編輯用
    public class ProductEditVm
    {
        [Key]
        public Guid ProductID { get; set; }

        [Required, StringLength(5, MinimumLength = 5)]
        public string ProductCode { get; set; } = null!;

        [Required, StringLength(20)]
        public string ProductName { get; set; } = null!;

        [Required, StringLength(200)]
        public string Description { get; set; } = null!;

        public string? Photo { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int CategoryID { get; set; }
    }
}
