using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<GoShopContext>>();

            using var context = new GoShopContext(options);
            context.Database.Migrate();

            using var tx = context.Database.BeginTransaction();

            var now = DateTime.UtcNow;

            // Admin
            if (!context.Admin.Any())
            {
                context.Admin.AddRange(
                    new Admin
                    {
                        AdminID = Guid.NewGuid(),
                        UserName = "SomaNN",
                        Email = "12121125@gmail.com",
                        PasswordHash = "445621231",
                        Role = "倉庫管理員",
                        IsActive = true,
                        CreatedDate = now,
                        LastLoginDate = now
                    },
                    new Admin
                    {
                        AdminID = Guid.NewGuid(),
                        UserName = "SomaCC",
                        Email = "66521125@gmail.com",
                        PasswordHash = "445621231",
                        Role = "業務",
                        IsActive = true,
                        CreatedDate = now,
                        LastLoginDate = now
                    }
                );
                context.SaveChanges();
            }

            // Member
            Member? member1 = context.Member.FirstOrDefault(m => m.Email == "12121320@gmail.com" && m.Name == "Soma");
            Member? member2 = context.Member.FirstOrDefault(m => m.Email == "12121125@gmail.com" && m.Name == "Amos");

            if (member1 == null || member2 == null)
            {
                member1 ??= new Member
                {
                    MemberID = Guid.NewGuid(),
                    Name = "Soma",
                    Birthday = new DateTime(1994, 06, 17),
                    Sex = "男",
                    Phone = "0928153146",
                    Address = "高雄市楠梓區",
                    IsEmailConfirmed = true,
                    CreatedDate = now,
                    LastLoginDate = now,
                    Email = "12121320@gmail.com"
                };
                member2 ??= new Member
                {
                    MemberID = Guid.NewGuid(),
                    Name = "Amos",
                    Birthday = new DateTime(1998, 08, 22),
                    Sex = "女",
                    Phone = "0928153146",
                    Address = "高雄市楠梓區",
                    IsEmailConfirmed = false,
                    CreatedDate = now,
                    LastLoginDate = now,
                    Email = "12121125@gmail.com"
                };

                if (!context.Member.Any(m => m.MemberID == member1.MemberID)) context.Member.Add(member1);
                if (!context.Member.Any(m => m.MemberID == member2.MemberID)) context.Member.Add(member2);
                context.SaveChanges();
            }

            // Account
            if (!context.Account.Any(a => a.AccountID == "s102211037"))
            {
                context.Account.Add(new Account
                {
                    AccountID = "s102211037",
                    PasswordHash = "sksdnhye",
                    MemberID = member1!.MemberID,
                });
            }
            if (!context.Account.Any(a => a.AccountID == "66255612"))
            {
                context.Account.Add(new Account
                {
                    AccountID = "66255612",
                    PasswordHash = "5365hye",
                    MemberID = member2!.MemberID,
                });
            }
            context.SaveChanges();

            // EmailVerification（修正第二筆要指到 Member2）
            if (!context.EmailVerification.Any())
            {
                context.EmailVerification.AddRange(
                    new EmailVerification
                    {
                        VerificationID = Guid.NewGuid(),
                        MemberID = member1!.MemberID,
                        VerificationCode = "sksdnhye",
                        ExpirationTime = now.AddSeconds(180),
                        IsVerified = true,
                        CreatedDate = now
                    },
                    new EmailVerification
                    {
                        VerificationID = Guid.NewGuid(),
                        MemberID = member2!.MemberID, // <-- 修正
                        VerificationCode = "5365hye",
                        ExpirationTime = now.AddSeconds(180),
                        IsVerified = true,
                        CreatedDate = now
                    }
                );
                context.SaveChanges();
            }

            // Category
            if (!context.Category.Any())
            {
                var category1 = new Category { CategoryName = "3C產品", CategoryCode = "A" };
                var category2 = new Category { CategoryName = "家電", CategoryCode = "B" };
                context.Category.AddRange(category1, category2);
                context.SaveChanges();
            }
            var catA = context.Category.First(c => c.CategoryCode == "A");

            // Product
            if (!context.Product.Any())
            {
                var p1 = new Product
                {
                    ProductID = Guid.NewGuid(),
                    ProductCode = "A1205",
                    ProductName = "手機",
                    Description = "高效能智慧型手機",
                    Photo = $"{Guid.NewGuid()}.jpg",
                    Price = 12000,
                    IsActive = true,
                    CreatedDate = now,
                    UpdatedDate = now,
                    CategoryID = catA.CategoryID
                };
                var p2 = new Product
                {
                    ProductID = Guid.NewGuid(),
                    ProductCode = "C1665",
                    ProductName = "筆電",
                    Description = "輕薄高效能筆電",
                    Photo = $"{Guid.NewGuid()}.jpg",
                    Price = 30000,
                    IsActive = false,
                    CreatedDate = now,
                    UpdatedDate = now,
                    CategoryID = catA.CategoryID
                };
                context.Product.AddRange(p1, p2);
                context.SaveChanges();
            }
            var prod1 = context.Product.First(p => p.ProductCode == "A1205");
            var prod2 = context.Product.First(p => p.ProductCode == "C1665");

            // Order
            if (!context.Order.Any())
            {
                var order1 = new Order
                {
                    OrderID = Guid.NewGuid(),
                    OrderNumber = "202501010009",
                    MemberID = member1!.MemberID,
                    CreatedDate = now,
                    UpdatedDate = now,
                    RecipientName = "Soma",
                    RecipientPhone = "0928153146",
                    RecipientAddress = "高雄市楠梓區",
                    PaymentStatus = "Unpaid",
                    PaymentMethod = "貨到付款",
                    ShippingStatus = "Pending",
                    OrderStatus = "Active"
                };
                var order2 = new Order
                {
                    OrderID = Guid.NewGuid(),
                    OrderNumber = "202501010013",
                    MemberID = member2!.MemberID,
                    CreatedDate = now,
                    UpdatedDate = now,
                    RecipientName = "Amos",
                    RecipientPhone = "0928153146",
                    RecipientAddress = "高雄市楠梓區",
                    PaymentStatus = "Paid", // 統一格式
                    PaymentMethod = "信用卡",
                    ShippingStatus = "Pending",
                    OrderStatus = "Active"
                };
                context.Order.AddRange(order1, order2);
                context.SaveChanges();

                // OrderDetail
                context.OrderDetail.AddRange(
                    new OrderDetail
                    {
                        OrderDetailID = Guid.NewGuid(),
                        OrderID = order1.OrderID,
                        ProductID = prod1.ProductID,
                        Quantity = 20,
                        UnitPrice = 12000,
                    },
                    new OrderDetail
                    {
                        OrderDetailID = Guid.NewGuid(),
                        OrderID = order2.OrderID,
                        ProductID = prod2.ProductID,
                        Quantity = 5,
                        UnitPrice = 30000,
                    }
                );
                context.SaveChanges();

                // Shipping
                context.Shipping.AddRange(
                    new Shipping
                    {
                        ShippingID = Guid.NewGuid(),
                        OrderID = order1.OrderID,
                        TrackingNumber = "123456789123456789123456789123",
                        Carrier = "黑貓宅急便",
                        RecipientName = "Soma",
                        ShippingAddress = "高雄市楠梓區",
                        RecipientPhone = "0928153146",
                        ShippedDate = now,
                        EstimatedArrivalDate = now.AddDays(2),
                        DeliveredDate = now.AddDays(3),
                        Status = "Processing",
                        CreatedDate = now
                    },
                    new Shipping
                    {
                        ShippingID = Guid.NewGuid(),
                        OrderID = order2.OrderID,
                        TrackingNumber = "123456789123456789123456780000",
                        Carrier = "黑貓宅急便",
                        RecipientName = "Amos",
                        ShippingAddress = "高雄市楠梓區",
                        RecipientPhone = "0928153146",
                        ShippedDate = now,
                        EstimatedArrivalDate = now.AddDays(2),
                        DeliveredDate = now.AddDays(3),
                        Status = "Processing",
                        CreatedDate = now
                    }
                );
                context.SaveChanges();

                // Invoice
                context.Invoice.AddRange(
                    new Invoice
                    {
                        InvoiceID = Guid.NewGuid(),
                        OrderID = order1.OrderID,
                        InvoiceNumber = "00000000000000000020",
                        InvoiceDate = now,
                        Amount = 120000,
                        Tax = 600,
                        TotalAmount = 120600,
                        CreatedDate = now
                    },
                    new Invoice
                    {
                        InvoiceID = Guid.NewGuid(),
                        OrderID = order2.OrderID,
                        InvoiceNumber = "00000000000000000021",
                        InvoiceDate = now,
                        Amount = 30000,
                        Tax = 800,
                        TotalAmount = 30800,
                        CreatedDate = now
                    }
                );
                context.SaveChanges();

                // Payment（修正第二筆 TotalAmount）
                context.Payment.AddRange(
                    new Payment
                    {
                        PaymentID = Guid.NewGuid(),
                        OrderID = order1.OrderID,
                        PaymentMethod = "到貨後60天",
                        TotalAmount = 120600,
                        PaymentDate = now,
                        Status = "Pending",
                        TransactionID = "451324512",
                        CreatedDate = now
                    },
                    new Payment
                    {
                        PaymentID = Guid.NewGuid(),
                        OrderID = order2.OrderID,
                        PaymentMethod = "到貨後30天",
                        TotalAmount = 30800, // <-- 修正
                        PaymentDate = now,
                        Status = "Pending",
                        TransactionID = "86224512",
                        CreatedDate = now
                    }
                );
                context.SaveChanges();
            }

            // Inventory
            if (!context.Inventory.Any())
            {
                context.Inventory.AddRange(
                    new Inventory
                    {
                        InventoryID = Guid.NewGuid(),
                        ProductID = prod1.ProductID,
                        Quantity = 20,
                        SafetyStock = 15,
                        LastUpdated = now
                    },
                    new Inventory
                    {
                        InventoryID = Guid.NewGuid(),
                        ProductID = prod2.ProductID,
                        Quantity = 4200,
                        SafetyStock = 800,
                        LastUpdated = now
                    }
                );
                context.SaveChanges();
            }

            // PurchaseOrder & Details
            if (!context.PurchaseOrder.Any())
            {
                var po1 = new PurchaseOrder
                {
                    PurchaseOrderID = Guid.NewGuid(),
                    PurchaseOrderCode = "00000000000000000030",
                    SupplierName = "ABC.LTD.",
                    Status = "Pending",
                    CreatedDate = now,
                    UpdatedDate = now
                };
                var po2 = new PurchaseOrder
                {
                    PurchaseOrderID = Guid.NewGuid(),
                    PurchaseOrderCode = "00000000000000000050",
                    SupplierName = "ABC.LTD.",
                    Status = "Pending",
                    CreatedDate = now,
                    UpdatedDate = now
                };
                context.PurchaseOrder.AddRange(po1, po2);
                context.SaveChanges();

                context.PurchaseOrderDetail.AddRange(
                    new PurchaseOrderDetail
                    {
                        PurchaseOrderDetailID = Guid.NewGuid(),
                        PurchaseOrderID = po1.PurchaseOrderID,
                        ProductID = prod1.ProductID,
                        Quantity = 40,
                        UnitPrice = 16000,
                    },
                    new PurchaseOrderDetail
                    {
                        PurchaseOrderDetailID = Guid.NewGuid(),
                        PurchaseOrderID = po2.PurchaseOrderID,
                        ProductID = prod2.ProductID,
                        Quantity = 50,
                        UnitPrice = 30000,
                    }
                );
                context.SaveChanges();
            }

            // Report
            if (!context.Report.Any())
            {
                context.Report.AddRange(
                    new Report
                    {
                        ReportID = Guid.NewGuid(),
                        ReportName = "應收帳款報表",
                        ReportType = "財務",
                        Content = "今天100000-52000",
                        CreatedBy = "SomaNN",
                        CreatedDate = now
                    },
                    new Report
                    {
                        ReportID = Guid.NewGuid(),
                        ReportName = "應收帳款報表",
                        ReportType = "財務",
                        Content = "今天100000-52000",
                        CreatedBy = "SomaNN",
                        CreatedDate = now
                    }
                );
                context.SaveChanges();
            }

            tx.Commit();
        }
    }
}
