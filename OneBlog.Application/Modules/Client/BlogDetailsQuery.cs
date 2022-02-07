using MediatR;
using Microsoft.EntityFrameworkCore;
using OneBlog.Domain.Entities;
using OneBlog.Persistence.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace OneBlog.Application.Modules.Client
{
    public class BlogDetailsQuery : IRequest<Blog>
    {
        public int Id { get; set; }
    }
    public class BlogDetailsQueryHandler : IRequestHandler<BlogDetailsQuery, Blog>
    {
        private readonly OneBlogDbContext db;

        public BlogDetailsQueryHandler(OneBlogDbContext db)
        {
            this.db = db;
        }
        public async Task<Blog> Handle(BlogDetailsQuery request, CancellationToken cancellationToken)
        {
            return await db.Blogs.FirstOrDefaultAsync(x=>x.Id == request.Id);
        }
    }
}
