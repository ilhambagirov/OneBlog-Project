using MediatR;
using Microsoft.EntityFrameworkCore;
using Oneblog.Application.Core.Infrastructor;
using OneBlog.Persistence.DataContext;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OneBlog.Application.Modules.Admin.Blogs
{
    public class BlogDeleteCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
    }
    public class BlogDeleteCommandHandler : IRequestHandler<BlogDeleteCommand, CommandJsonResponse>
    {
        private readonly OneBlogDbContext db;
        public BlogDeleteCommandHandler(OneBlogDbContext db)
        {
            this.db = db;
        }
        public async Task<CommandJsonResponse> Handle(BlogDeleteCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandJsonResponse();
            if (request.Id <= 0)
            {
                response.Error = true;
                response.Message = "Item is not defined";
                goto end;
            }

            var entity = await db.Blogs.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedDate == null);

            if (entity == null)
            {
                response.Error = true;
                response.Message = "Item is not found";
                goto end;
            }

            entity.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync(cancellationToken);
            response.Error = false;
            response.Message = "Item is deleted";
        end:
            return response;
        }
    }
}

