using Microsoft.EntityFrameworkCore;
using WebScrappersApplication.Models;

namespace WebScrappersApplication.Data
{
    public class AppdbContext:DbContext
    {
        public AppdbContext(DbContextOptions<AppdbContext> options) : base(options)
        {
                
        }

        public DbSet<Products> products { get; set; }
    }
}
