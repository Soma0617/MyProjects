using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 單一購物車項目
    public class CartItemVm
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string? Photo { get; set; }
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public decimal Subtotal => Price * Quantity;
    }

    // 整個購物車
    public class CartVm
    {
        public List<CartItemVm> Items { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
