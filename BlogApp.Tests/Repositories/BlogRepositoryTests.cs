using BlogApp.Abstractions;
using BlogApp.Exceptions;
using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Tests.Repositories
{
    public class BlogRepositoryTests
    {
        private DbContextOptions<BloggingContext> GetDbOptions(string name)
        {
            return new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }

        [Fact]
        public async Task CreateAsync_BlogWithValidUrl_CreateNewBlogAsync()
        {
            // Arrange
            var options = GetDbOptions("CreateAsync_BlogWithValidUrl_CreateNewBlog");
            var blog = new Blog { Url = "https://sample.com" };

            // Act
            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {
                IBlogRepository repository = new BlogRepository(context);
                await repository.CreateAsync(blog);
            }

            // Assert
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new BloggingContext(options))
            {
                Assert.Equal(1, context.Blogs.Count());
                Assert.Equal("https://sample.com", context.Blogs.Single().Url);
            }
        }

        [Theory]
        [InlineData("https://urlwith=?.invalid/chars")]
        [InlineData("not a url at all")]
        [InlineData("www.noturl. spaces")]
        public async Task CreateAsync_MalformedUrl_ThrowsAppExceptionAsync(string url)
        {
            // Arrange
            var options = GetDbOptions("CreateAsync_MalformedUrl_ThrowsAppExceptionAsync");
            var blog = new Blog { Url = url };

            // Act/Assert
            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {
                IBlogRepository repository = new BlogRepository(context);
                await Assert.ThrowsAsync<AppException>(async () => await repository.CreateAsync(blog));
            }

            //// Assert
            //// Use a separate instance of the context to verify correct data was saved to database
            //using (var context = new BloggingContext(options))
            //{
            //    Assert.Equal(1, context.Blogs.Count());
            //    Assert.Equal("http://sample.com", context.Blogs.Single().Url);
            //}
        }
    }
}
