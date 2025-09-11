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
                        LastLoginDate = DateTime.Now
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
                        LastLoginDate = DateTime.Now
                    };
                    context.Member.AddRange(Member1, Member2);
                    context.SaveChanges();

                    context.Account.AddRange(

                        new Account
                        {
                            AccountID = "s102211037",
                            PasswordHash = "sksdnhye",
                            MemberID = Member1.MemberID,
                        },
                        new Account
                        {
                            AccountID = "66255612",
                            PasswordHash = "5365hye",
                            MemberID = Member2.MemberID,
                        }
                    );
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
                    );
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







                    [Key]
                    public Guid InvoiceID { get; set; } = Guid.NewGuid();

                    [Required]
                    public Guid OrderID { get; set; }

                    [Display(Name = "發票號碼")]
                    [StringLength(20)]
                    [Required(ErrorMessage = "必填欄位")]
                    public string InvoiceNumber { get; set; } = null!;

                    [Display(Name = "發票日期")]
                    [DataType(DataType.Date)]
                    [Required]
                    public DateTime InvoiceDate { get; set; } = DateTime.Now;

                    [Display(Name = "金額")]
                    [Range(0.01, double.MaxValue, ErrorMessage = "金額需大於0")]
                    [DataType(DataType.Currency)]
                    public decimal Amount { get; set; }

                    [Display(Name = "稅額")]
                    [Range(0, double.MaxValue)]
                    public decimal Tax { get; set; }

                    [Display(Name = "總金額")]
                    [Range(0.01, double.MaxValue, ErrorMessage = "總金額需大於0")]
                    [DataType(DataType.Currency)]
                    public decimal TotalAmount { get; set; }

                    [Display(Name = "建立時間")]
                    public DateTime CreatedDate { get; set; } = DateTime.Now;







                    //(3)撰寫上傳圖片的程式
                    string SeedPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedPhotos");//取得來源照片路徑
                    string BookPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookPhotos");//取得目的路徑


                    string[] files = Directory.GetFiles(SeedPhotosPath);  //取得指定路徑中的所有檔案

                    for (int i = 0; i < files.Length; i++)
                    {
                        string destFile = Path.Combine(BookPhotosPath, guid[i] + ".jpg");


                        File.Copy(files[i], destFile);
                    }
                }
            } //using結束
        }
    }
}
