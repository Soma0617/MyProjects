using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = null!;

        public string CategoryCode { get; set; } = null!;
    }
}
