using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    // 放在購物車中的每一筆商品
    public class ShoppingCarItem
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Photo { get; set; }
        public decimal UnitPrice { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; } = 1;

        public decimal Subtotal => UnitPrice * Quantity;
    }

    // 整個購物車
    public class ShoppingCar
    {
        public List<ShoppingCarItem> Items { get; set; } = new();
        public int TotalQty => Items.Sum(x => x.Quantity);
        public decimal TotalAmount => Items.Sum(x => x.Subtotal);
    }
}
