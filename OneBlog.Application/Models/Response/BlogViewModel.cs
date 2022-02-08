using Microsoft.AspNetCore.Http;

namespace OneBlog.Application.Models.Response
{
    public class BlogViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public IFormFile File { get; set; }
        public string Body { get; set; }
        public string fileTemp { get; set; }
    }
}