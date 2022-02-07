namespace OneBlog.Domain.Entities
{
    public class Blog  :BaseEntity
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Body { get; set; }
    }
}
