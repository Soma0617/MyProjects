using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Models
{
    public class GoShopContext : DbContext
    {
        public GoShopContext(DbContextOptions<GoShopContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<EmailVerification> EmailVerification { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Shipping> Shipping { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Report> Report { get; set; }
    }
}
