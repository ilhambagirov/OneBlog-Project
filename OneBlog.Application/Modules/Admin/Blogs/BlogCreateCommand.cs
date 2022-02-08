using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using OneBlog.Domain.Entities;
using OneBlog.Persistence.DataContext;
using Portfolio.Application.Core.Extension;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OneBlog.Application.Modules.Admin.Blogs
{
    public class BlogCreateCommand : IRequest<Blog>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Body { get; set; }
        public IFormFile File { get; set; }
        public class BlogCreateCommandHandler : IRequestHandler<BlogCreateCommand, Blog>
        {

            readonly OneBlogDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IHostEnvironment env;
            public BlogCreateCommandHandler(OneBlogDbContext db, IActionContextAccessor ctx, IHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }
            public async Task<Blog> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.File == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "Not chosen");
                };

                if (ctx.IsModelStateValid())
                {
                    var blog = new Blog();
                    blog.Title = request.Title;
                    blog.ShortDescription = request.ShortDescription;
                    blog.Body = request.Body;
                    var extension = Path.GetExtension(request.File.FileName);
                    blog.FilePath = $"{Guid.NewGuid()}{extension}";
                    var physicalAddress = Path.Combine(env.ContentRootPath,
                        "wwwroot",
                        "Images",
                        "BlogImages",
                        blog.FilePath);

                    using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                    {
                        await request.File.CopyToAsync(stream);
                    }

                    db.Add(blog);
                    await db.SaveChangesAsync(cancellationToken);
                    return blog;
                }
                return null;

            }
        }
    }
}
