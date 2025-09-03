using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Models
{
    public class GoShopContext : DbContext
    {
        public GoShopContext(DbContextOptions<GoShopContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Members> Member { get; set; }
        public virtual DbSet<Products> Product { get; set; }

    }
}
