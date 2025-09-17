using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FinalExamProject.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (GoShopContext context = new GoShopContext(serviceProvider.GetRequiredService<DbContextOptions<GoShopContext>>()))
            {
                if (!context.Member.Any())
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
                            CreatedDate = DateTime.Now,
                            LastLoginDate = DateTime.Now
                        },
                        new Admin
                        {
                            AdminID = Guid.NewGuid(),
                            UserName = "SomaCC",
                            Email = "66521125@gmail.com",
                            PasswordHash = "445621231",
                            Role = "業務",
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            LastLoginDate = DateTime.Now
                        }

                    );
                    context.SaveChanges();

                    var Member1 = new Member
                    {
                        MemberID = Guid.NewGuid(),
                        Name = "Soma",
                        Birthday = new DateTime(1994, 06, 17),
                        Sex = "男",
                        Phone = "0928153146",
                        Address = "高雄市楠梓區",
                        IsEmailConfirmed = true,
                        CreatedDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        Email = "12121125@gmail.com"
                    };
                    var Member2 = new Member
                    {
                        MemberID = Guid.NewGuid(),
                        Name = "Amos",
                        Birthday = new DateTime(1998, 08, 22),
                        Sex = "女",
                        Phone = "0928153146",
                        Address = "高雄市楠梓區",
                        IsEmailConfirmed = false,
                        CreatedDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        Email = "12121125@gmail.com"
                    };
                    context.Member.AddRange(Member1, Member2);
                    context.SaveChanges();

                    if (!context.Account.Any(a => a.AccountID == "s102211037"))
                    {
                        context.Account.Add(new Account
                        {
                            AccountID = "s102211037",
                            PasswordHash = "sksdnhye",
                            MemberID = Member1.MemberID,
                        });
                    }
                    if (!context.Account.Any(a => a.AccountID == "66255612"))
                    {
                        context.Account.Add(new Account
                        {
                            AccountID = "66255612",
                            PasswordHash = "5365hye",
                            MemberID = Member2.MemberID,
                        });
                    }
                    context.SaveChanges();

                    context.EmailVerification.AddRange(

                        new EmailVerification
                        {
                            VerificationID = Guid.NewGuid(),
                            MemberID = Member1.MemberID,
                            VerificationCode = "sksdnhye",
                            ExpirationTime = DateTime.Now.AddSeconds(180),
                            IsVerified = true,
                            CreatedDate = DateTime.Now
                        },
                        new EmailVerification
                        {
                            VerificationID = Guid.NewGuid(),
                            MemberID = Member1.MemberID,
                            VerificationCode = "5365hye",
                            ExpirationTime = DateTime.Now.AddSeconds(180),
                            IsVerified = true,
                            CreatedDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();

                    var Order1 = new Order
                    {
                        OrderID = Guid.NewGuid(),
                        OrderNumber = "202501010009",
                        MemberID = Member1.MemberID,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        RecipientName = "Soma",
                        RecipientPhone = "0928153146",
                        RecipientAddress = "高雄市楠梓區",
                        PaymentStatus = "Unpaid",
                        PaymentMethod = "貨到付款",
                        ShippingStatus = "Pending",
                        OrderStatus = "Active"
                    };
                    var Order2 = new Order
                    {
                        OrderID = Guid.NewGuid(),
                        OrderNumber = "202501010013",
                        MemberID = Member2.MemberID,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        RecipientName = "Amos",
                        RecipientPhone = "0928153146",
                        RecipientAddress = "高雄市楠梓區",
                        PaymentStatus = "paid",
                        PaymentMethod = "信用卡",
                        ShippingStatus = "Pending",
                        OrderStatus = "Active"
                    };
                    context.Order.AddRange(Order1, Order2);
                    context.SaveChanges();

                    var category1 = new Category
                    {
                        CategoryName = "3C產品",
                        CategoryCode = "A"
                    };
                    var category2 = new Category
                    {
                        CategoryName = "家電",
                        CategoryCode = "B"
                    };
                    context.Category.AddRange(category1, category2);
                    context.SaveChanges();

                    var Product1 = new Product
                    {
                        ProductID = Guid.NewGuid(),
                        ProductCode = "A1205",
                        ProductName = "手機",
                        Description = "摳你機挖",
                        Photo = Guid.NewGuid().ToString() + ".jpg",
                        Price = 12000,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CategoryID = category1.CategoryID
                    };
                    var Product2 = new Product
                    {
                        ProductID = Guid.NewGuid(),
                        ProductCode = "C1665",
                        ProductName = "筆電",
                        Description = "摳你機挖",
                        Photo = Guid.NewGuid().ToString() + ".jpg",
                        Price = 30000,
                        IsActive = false,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CategoryID = category1.CategoryID
                    };
                    context.Product.AddRange(Product1, Product2);
                    context.SaveChanges();

                    context.OrderDetail.AddRange(

                        new OrderDetail
                        {
                            OrderDetailID = Guid.NewGuid(),
                            OrderID = Order1.OrderID,
                            ProductID = Product1.ProductID,
                            Quantity = 20,
                            UnitPrice = 12000,
                        },
                        new OrderDetail
                        {
                            OrderDetailID = Guid.NewGuid(),
                            OrderID = Order2.OrderID,
                            ProductID = Product2.ProductID,
                            Quantity = 5,
                            UnitPrice = 30000,
                        }
                    );
                    context.SaveChanges();

                    context.Shipping.AddRange(

                        new Shipping
                        {
                            ShippingID = Guid.NewGuid(),
                            OrderID = Order1.OrderID,
                            TrackingNumber = "123456789123456789123456789123",
                            Carrier = "黑貓宅急便",
                            RecipientName = "Soma",
                            ShippingAddress = "高雄市楠梓區",
                            RecipientPhone = "0928153146",
                            ShippedDate = DateTime.Now,
                            EstimatedArrivalDate = DateTime.Now.AddDays(2),
                            DeliveredDate = DateTime.Now.AddDays(3),
                            Status = "Processing ",
                            CreatedDate = DateTime.Now
                        },
                        new Shipping
                        {
                            ShippingID = Guid.NewGuid(),
                            OrderID = Order2.OrderID,
                            TrackingNumber = "123456789123456789123456780000",
                            Carrier = "黑貓宅急便",
                            RecipientName = "Amos",
                            ShippingAddress = "高雄市楠梓區",
                            RecipientPhone = "0928153146",
                            ShippedDate = DateTime.Now,
                            EstimatedArrivalDate = DateTime.Now.AddDays(2),
                            DeliveredDate = DateTime.Now.AddDays(3),
                            Status = "Processing ",
                            CreatedDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();

                    context.Invoice.AddRange(

                        new Invoice
                        {
                            InvoiceID = Guid.NewGuid(),
                            OrderID = Order1.OrderID,
                            InvoiceNumber = "00000000000000000020",
                            InvoiceDate = DateTime.Now,
                            Amount = 120000,
                            Tax = 600,
                            TotalAmount = 126000,
                            CreatedDate = DateTime.Now
                        },
                        new Invoice
                        {
                            InvoiceID = Guid.NewGuid(),
                            OrderID = Order2.OrderID,
                            InvoiceNumber = "00000000000000000021",
                            InvoiceDate = DateTime.Now,
                            Amount = 30000,
                            Tax = 800,
                            TotalAmount = 30800,
                            CreatedDate = DateTime.Now
                        }

                    );
                    context.SaveChanges();

                    context.Payment.AddRange(

                        new Payment
                        {
                            PaymentID = Guid.NewGuid(),
                            OrderID = Order1.OrderID,
                            PaymentMethod = "到貨後60天",
                            TotalAmount = 120600,
                            PaymentDate = DateTime.Now,
                            Status = "Pending",
                            TransactionID = "451324512",
                            CreatedDate = DateTime.Now
                        },
                        new Payment
                        {
                            PaymentID = Guid.NewGuid(),
                            OrderID = Order2.OrderID,
                            PaymentMethod = "到貨後30天",
                            TotalAmount = 120600,
                            PaymentDate = DateTime.Now,
                            Status = "Pending",
                            TransactionID = "86224512",
                            CreatedDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();

                    context.Inventory.AddRange(

                        new Inventory
                        {
                            InventoryID = Guid.NewGuid(),
                            ProductID = Product1.ProductID,
                            Quantity = 20,
                            SafetyStock = 15,
                            LastUpdated = DateTime.Now
                        },
                        new Inventory
                        {
                            InventoryID = Guid.NewGuid(),
                            ProductID = Product2.ProductID,
                            Quantity = 4200,
                            SafetyStock = 800,
                            LastUpdated = DateTime.Now
                        }
                    );
                    context.SaveChanges();

                    var PurchaseOrder1 = new PurchaseOrder
                    {
                        PurchaseOrderID = Guid.NewGuid(),
                        PurchaseOrderCode = "00000000000000000030",
                        SupplierName = "ABC.LTD.",
                        Status = "Pending",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    var PurchaseOrder2 = new PurchaseOrder
                    {
                        PurchaseOrderID = Guid.NewGuid(),
                        PurchaseOrderCode = "00000000000000000050",
                        SupplierName = "ABC.LTD.",
                        Status = "Pending",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    context.PurchaseOrder.AddRange(PurchaseOrder1, PurchaseOrder2);
                    context.SaveChanges();


                    context.PurchaseOrderDetail.AddRange(

                        new PurchaseOrderDetail
                        {
                            PurchaseOrderDetailID = Guid.NewGuid(),
                            PurchaseOrderID = PurchaseOrder1.PurchaseOrderID,
                            ProductID = Product1.ProductID,
                            Quantity = 40,
                            UnitPrice = 16000,
                        },
                        new PurchaseOrderDetail
                        {
                            PurchaseOrderDetailID = Guid.NewGuid(),
                            PurchaseOrderID = PurchaseOrder2.PurchaseOrderID,
                            ProductID = Product2.ProductID,
                            Quantity = 50,
                            UnitPrice = 30000,
                        }
                    );
                    context.SaveChanges();

                    context.Report.AddRange(

                        new Report
                        {
                            ReportID = Guid.NewGuid(),
                            ReportName = "應收帳款報表",
                            ReportType = "財務",
                            Content = "今天100000-52000",
                            CreatedBy = "SomaNN",
                            CreatedDate = DateTime.Now
                        },
                        new Report
                        {
                            ReportID = Guid.NewGuid(),
                            ReportName = "應收帳款報表",
                            ReportType = "財務",
                            Content = "今天100000-52000",
                            CreatedBy = "SomaNN",
                            CreatedDate = DateTime.Now
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
