using WebScrapperAPI.Models;

namespace WebScrapperAPI.Repository
{
    public interface IWebScrapperRepository
    {
        public Task CreateUser(User user);
        public Task<User> Login(string userName, string password);
        public Task<bool> CheckEmail(string email);
        public Task<bool> CheckUserName(string userName);
        public Task AddWebPage(ScrapedPage page);
        public Task<IList<ScrapedPage>> GetPages(int userId);
        public Task<IList<ScrapedPage>> GetPagesForProcess();
        public Task UpdatePage(ScrapedPage page);
    }
}
