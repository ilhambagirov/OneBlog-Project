using MediatR;
using Microsoft.EntityFrameworkCore;
using OneBlog.Domain.Entities;
using OneBlog.Persistence.DataContext;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OneBlog.Application.Modules.Client
{
    public class BlogListCommand : IRequest<List<Blog>>
    {
    }

    public class BlogListCommandHandler : IRequestHandler<BlogListCommand, List<Blog>>
    {
        private readonly OneBlogDbContext db;

        public BlogListCommandHandler(OneBlogDbContext db)
        {
            this.db = db;
        }
        public async Task<List<Blog>> Handle(BlogListCommand request, CancellationToken cancellationToken)
        {
            return await db.Blogs.ToListAsync();
        }
    }
}
