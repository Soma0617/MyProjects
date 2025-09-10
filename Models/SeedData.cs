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
                    context.Member.AddRange(
                        new Member
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
                        },
                        new Member
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
                        }
                    );

                    context.SaveChanges();


                    context.Admin.AddRange(

                        new Admin
                        {
                            AdminID = Guid.NewGuid(),
                            UserName = "SomaNN",
                            Email = "12121125@gmail.com",
                            PasswordHash= "445621231",
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
