using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneBlog.Domain.Entities;

namespace OneBlog.Persistence.DataContext
{
    public class OneBlogDbContext : IdentityDbContext<OneBlogUser, OneBlogRole, int>
    {
        public OneBlogDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
    }
}
