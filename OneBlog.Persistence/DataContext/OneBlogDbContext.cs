using Microsoft.EntityFrameworkCore;
using OneBlog.Domain.Entities;

namespace OneBlog.Persistence.DataContext
{
    public class OneBlogDbContext : DbContext
    {
        public OneBlogDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
    }
}
