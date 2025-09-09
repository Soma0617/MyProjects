using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExamProject.Models
{
    public class Product
    {
        public Guid ProductID { get; set; } = Guid.NewGuid(); 

        public string ProductCode { get; set; } = null!; 

        public string ProductName { get; set; } = null!; 

        public string Description { get; set; } = null!; 

        public string? Photo { get; set; } 

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category CategoryInfo { get; set; } = null!;
    }
}
