using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Tests.Integration
{
    public class Utilities
    {
        public static void InitializeDbForTests(BloggingContext context)
        {
            context.Add(new Blog { Url = "https://devblogs.microsoft.com/dotnet/" });
            context.Add(new Blog { Url = "https://developer.apple.com/develop/" });
            context.Add(new Blog { Url = "https://developer.android.com/news" });
            context.SaveChanges();
        }

        public static BloggingContext GetDbContext(string connectionString)
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseSqlServer(connectionString)
                .Options;
            return new BloggingContext(options);
        }

        public static void DropDbContext(string connectionString)
        {
            using (var db = GetDbContext(connectionString))
            {
                db.Database.EnsureDeleted();
            }
        }
    }
}
