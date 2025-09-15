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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Admin
        //    modelBuilder.Entity<Admin>(entity =>
        //    {
        //        entity.HasKey(e => e.AdminID).HasName("PK_AdminID");
        //        entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
        //        entity.Property(e => e.PasswordHash).IsRequired();
        //        entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
        //    });

        //    // Member
        //    modelBuilder.Entity<Member>(entity =>
        //    {
        //        entity.HasKey(e => e.MemberID).HasName("PK_MemberID");
        //        entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
        //        entity.Property(e => e.Phone).HasMaxLength(15);
        //        entity.Property(e => e.Address).HasMaxLength(200);
        //    });

        //    // Account
        //    modelBuilder.Entity<Account>(entity =>
        //    {
        //        entity.HasKey(e => e.AccountID).HasName("PK_AccountID");
        //        entity.Property(e => e.AccountID).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.PasswordHash).IsRequired();
        //        entity.HasOne(e => e.MemberInfo)
        //              .WithMany()
        //              .HasForeignKey(e => e.MemberID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // EmailVerification
        //    modelBuilder.Entity<EmailVerification>(entity =>
        //    {
        //        entity.HasKey(e => e.VerificationID).HasName("PK_VerificationID");
        //        entity.Property(e => e.VerificationCode).IsRequired().HasMaxLength(100);
        //        entity.HasOne(e => e.MemberInfo)
        //              .WithMany()
        //              .HasForeignKey(e => e.MemberID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Category
        //    modelBuilder.Entity<Category>(entity =>
        //    {
        //        entity.HasKey(e => e.CategoryID).HasName("PK_CategoryID");
        //        entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.CategoryCode).IsRequired().HasMaxLength(1);
        //    });

        //    // Product
        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.HasKey(e => e.ProductID).HasName("PK_ProductID");
        //        entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(5);
        //        entity.Property(e => e.ProductName).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
        //        entity.Property(e => e.Photo).HasMaxLength(200);
        //        entity.Property(e => e.Price).IsRequired();
        //        entity.HasOne<Category>()
        //              .WithMany()
        //              .HasForeignKey(e => e.CategoryID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Order
        //    modelBuilder.Entity<Order>(entity =>
        //    {
        //        entity.HasKey(e => e.OrderID).HasName("PK_OrderID");
        //        entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(12);
        //        entity.Property(e => e.RecipientName).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.RecipientPhone).IsRequired().HasMaxLength(15);
        //        entity.Property(e => e.RecipientAddress).IsRequired().HasMaxLength(100);
        //        entity.Property(e => e.PaymentStatus).IsRequired();
        //        entity.Property(e => e.ShippingStatus).IsRequired();
        //        entity.Property(e => e.OrderStatus).IsRequired();
        //        entity.HasOne<Member>()
        //              .WithMany()
        //              .HasForeignKey(e => e.MemberID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // OrderDetail
        //    modelBuilder.Entity<OrderDetail>(entity =>
        //    {
        //        entity.HasKey(e => e.OrderDetailID).HasName("PK_OrderDetailID");
        //        entity.Property(e => e.Quantity).IsRequired();
        //        entity.Property(e => e.UnitPrice).IsRequired();
        //        entity.HasOne<Order>()
        //              .WithMany()
        //              .HasForeignKey(e => e.OrderID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //        entity.HasOne<Product>()
        //              .WithMany()
        //              .HasForeignKey(e => e.ProductID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Shipping
        //    modelBuilder.Entity<Shipping>(entity =>
        //    {
        //        entity.HasKey(e => e.ShippingID).HasName("PK_ShippingID");
        //        entity.Property(e => e.TrackingNumber).IsRequired().HasMaxLength(30);
        //        entity.Property(e => e.Carrier).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.RecipientName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.ShippingAddress).IsRequired().HasMaxLength(200);
        //        entity.Property(e => e.RecipientPhone).HasMaxLength(20);
        //        entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        //        entity.HasOne<Order>()
        //              .WithMany()
        //              .HasForeignKey(e => e.OrderID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Payment
        //    modelBuilder.Entity<Payment>(entity =>
        //    {
        //        entity.HasKey(e => e.PaymentID).HasName("PK_PaymentID");
        //        entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.TransactionID).HasMaxLength(50);
        //        entity.HasOne<Order>()
        //              .WithMany()
        //              .HasForeignKey(e => e.OrderID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Invoice
        //    modelBuilder.Entity<Invoice>(entity =>
        //    {
        //        entity.HasKey(e => e.InvoiceID).HasName("PK_InvoiceID");
        //        entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(20);
        //        entity.HasOne<Order>()
        //              .WithMany()
        //              .HasForeignKey(e => e.OrderID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Inventory
        //    modelBuilder.Entity<Inventory>(entity =>
        //    {
        //        entity.HasKey(e => e.InventoryID).HasName("PK_InventoryID");
        //        entity.HasOne<Product>()
        //              .WithMany()
        //              .HasForeignKey(e => e.ProductID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // PurchaseOrder
        //    modelBuilder.Entity<PurchaseOrder>(entity =>
        //    {
        //        entity.HasKey(e => e.PurchaseOrderID).HasName("PK_PurchaseOrderID");
        //        entity.Property(e => e.PurchaseOrderCode).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.SupplierName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        //    });

        //    // PurchaseOrderDetail
        //    modelBuilder.Entity<PurchaseOrderDetail>(entity =>
        //    {
        //        entity.HasKey(e => e.PurchaseOrderDetailID).HasName("PK_PurchaseOrderDetailID");
        //        entity.Property(e => e.Quantity).IsRequired();
        //        entity.Property(e => e.UnitPrice).IsRequired();
        //        entity.HasOne<PurchaseOrder>()
        //              .WithMany()
        //              .HasForeignKey(e => e.PurchaseOrderID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //        entity.HasOne<Product>()
        //              .WithMany()
        //              .HasForeignKey(e => e.ProductID)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // Report
        //    modelBuilder.Entity<Report>(entity =>
        //    {
        //        entity.HasKey(e => e.ReportID).HasName("PK_ReportID");
        //        entity.Property(e => e.ReportName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.ReportType).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.Content).IsRequired();
        //        entity.Property(e => e.CreatedBy).HasMaxLength(50);
        //    });
        //}
    }
}
