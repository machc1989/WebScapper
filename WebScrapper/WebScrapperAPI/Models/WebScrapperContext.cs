using Microsoft.EntityFrameworkCore;

namespace WebScrapperAPI.Models
{
    public class WebScrapperContext : DbContext
    {
        public WebScrapperContext(DbContextOptions<WebScrapperContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ScrapedPage> ScrapedPages { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>()
                .HasOne(l => l.ScrapedPage)
                .WithMany(p => p.Links)
                .HasForeignKey(l => l.ScrapedPageId);

            modelBuilder.Entity<Link>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<ScrapedPage>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.UserName)
                .IsUnique();

        }
    }
}
