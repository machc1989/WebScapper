using System.ComponentModel.DataAnnotations;

namespace WebScrapperAPI.Models
{
    public class ScrapedPage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PageName { get; set; }
        public string Url { get; set; }
        public int LinkCount { get; set; }
        public bool Processed { get; set; }

        public ICollection<Link> Links { get; set; }
    }

    public class Link
    {
        public int Id { get; set; }
        public int ScrapedPageId { get; set; }
        public string Url { get; set; }
        public string LinkName { get; set; }

        public virtual ScrapedPage ScrapedPage { get; set; }
    }

    public class ScrapperPageDTO
    {
        [Required]
        public string PageName { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
