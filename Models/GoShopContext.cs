using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Models
{
    public class GoShopContext : DbContext
    {
        public GoShopContext(DbContextOptions<GoShopContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Product> Product { get; set; }

    }
}
