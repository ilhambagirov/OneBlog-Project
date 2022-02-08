using Microsoft.AspNetCore.Mvc;
using OneBlog.Application.Models.Response;
using OneBlog.Application.Modules.Admin.Blogs;
using OneBlog.Application.Modules.Client;
using OneBlog.WebUI.Controllers;
using System.Threading.Tasks;

namespace OneBlog.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new BlogListCommand()));
        }
        [HttpGet]
        public async Task<IActionResult> Details(BlogDetailsQuery query)
        {
            return View(await Mediator.Send(query));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(BlogDeleteCommand command)
        {
            return Json(await Mediator.Send(command));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateCommand command)
        {
            var blog = await Mediator.Send(command);
            if (blog != null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }
        public async Task<IActionResult> Edit(BlogDetailsQuery query)
        {
            var blog = await Mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }
            BlogViewModel vm = new();
            vm.Id = blog.Id;
            vm.Title = blog.Title;
            vm.ShortDescription = blog.ShortDescription;
            vm.Body = blog.Body;
            vm.fileTemp = blog.FilePath;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, BlogEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var _id = await Mediator.Send(command);

            if (id > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }
    }
}
