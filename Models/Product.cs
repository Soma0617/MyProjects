using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Product
    {
        [Key] 
        public Guid ProductID { get; set; } = Guid.NewGuid(); 
        
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string ProductCode { get; set; } = null!; 
        
        [Display(Name = "產品名稱")] [StringLength(20, ErrorMessage ="最大字數20個字")] 
        [Required(ErrorMessage ="必填欄位")] 
        public string ProductName { get; set; } = null!; 
        
        [Display(Name = "產品描述")] 
        [StringLength(200, ErrorMessage = "最大字數200個字")] 
        [Required(ErrorMessage = "必填欄位")] 
        [DataType(DataType.MultilineText)] 
        public string Description { get; set; } = null!; 
        
        [Display(Name = "產品圖片")] 
        [StringLength(200)] 
        public string? Photo { get; set; } 
        
        [Display(Name = "產品價格")] 
        [Range(0.01, double.MaxValue, ErrorMessage = "價格需大於0")] 
        [DataType(DataType.Currency)] 
        public decimal Price { get; set; }

        [Display(Name = "是否上架")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "發布時間")] 
        [DataType(DataType.DateTime)] 
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")] 
        [HiddenInput] 
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public int CategoryID { get; set; }

        public Category CategoryInfo { get; set; } = null!;
    }
}
