using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneBlog.Application.Modules.Client;
using OneBlog.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneBlog.WebUI.Controllers
{
    [AllowAnonymous]
    public class BlogController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<Blog>>> Index()
        {
            return View(await Mediator.Send(new BlogListCommand()));
        }

        [HttpGet("Id")]
        public async Task<ActionResult<Blog>> Detail(BlogDetailsQuery query)
        {
            return View(await Mediator.Send(query));
        }
    }
}
