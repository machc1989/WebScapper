namespace WebScrapper.Models
{
    public class ScrappedPageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TotalLinks { get; set; }
        public bool isProcessed { get; set; }
    }

    public class Link
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }

    public class ScrappedPageDTO
    {
        public string PageName { get; set; }
        public string Url { get; set; }
    }
}
