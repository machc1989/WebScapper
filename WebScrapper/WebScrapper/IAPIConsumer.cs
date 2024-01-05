using WebScrapper.Models;

namespace WebScrapper
{
    public interface IAPIConsumer
    {
        Task<string> Login(string userName, string password);
        Task<bool> CreateUser(UserModel user);
        Task<List<ScrappedPageModel>> GetPages(string token);
        Task PostPage(ScrappedPageDTO pageDTO, string token);
    }
}
