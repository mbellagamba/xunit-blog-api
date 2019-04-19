using BlogApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Tests.Integration
{
    public class BlogsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task GetBlogsAsync_Get_ListBlogs()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var connection = factory.ConnectionString;

            // Act
            var httpResponse = await client.GetAsync("/api/blogs");

            // Assert
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var blogs = JsonConvert.DeserializeObject<IEnumerable<Blog>>(stringResponse);
            Assert.Equal(3, blogs.Count());
            Assert.Contains(blogs, b => b.Url.Contains("devblogs.microsoft.com"));
            Assert.Contains(blogs, b => b.Url.Contains("developer.apple.com"));
            Assert.Contains(blogs, b => b.Url.Contains("developer.android.com"));

            // Dispose
            client.Dispose();
            Utilities.DropDbContext(connection);
        }

        [Fact]
        public async Task GetBlogAsync_Get_ReturnsTheBlog()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var connection = factory.ConnectionString;
            var httpResponse = await client.GetAsync("/api/blogs/1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var blog = JsonConvert.DeserializeObject<Blog>(stringResponse);
            Assert.Equal("https://devblogs.microsoft.com/dotnet/", blog.Url);

            // Dispose
            client.Dispose();
            Utilities.DropDbContext(connection);
        }
    }
}
