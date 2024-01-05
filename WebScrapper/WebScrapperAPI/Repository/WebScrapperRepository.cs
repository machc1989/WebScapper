using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebScrapperAPI.Models;

namespace WebScrapperAPI.Repository
{
    public class WebScrapperRepository : IWebScrapperRepository
    {
        WebScrapperContext _dbContext;
        public WebScrapperRepository(WebScrapperContext dbContext) 
        {
            _dbContext = dbContext;  
        }
        public async Task AddWebPage(ScrapedPage page)
        {
            _dbContext.ScrapedPages.Add(page);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await Task.Run(() => _dbContext.Users.Any(x => x.Email == email));
        }

        public async Task<bool> CheckUserName(string userName)
        {
            return await Task.Run(() => _dbContext.Users.Any(x => x.UserName == userName));
        }

        public async Task CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<ScrapedPage>> GetPages(int userId)
        {
            var pages = await Task.Run(() => _dbContext.ScrapedPages.Where(x => x.UserId == userId).ToList());
            return pages;
        }

        public async Task<IList<ScrapedPage>> GetPagesForProcess()
        {
            return await Task.Run(() => _dbContext.ScrapedPages.Where(x => x.Processed == false).ToList());
        }

        public async Task<User> Login(string userName, string password)
        {
            return await _dbContext.Users.Where(e => (e.UserName == userName && e.PasswordHash == password)
            || (e.Email == userName && e.PasswordHash == password)).FirstOrDefaultAsync();
        }

        public async Task UpdatePage(ScrapedPage page)
        {
            _dbContext.ScrapedPages.Update(page);
            await _dbContext.SaveChangesAsync();
        }
    }
}
