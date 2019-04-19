using BlogApp.Abstractions;
using BlogApp.Controllers;
using BlogApp.Exceptions;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Tests.Controllers
{
    public class BlogsControllerTests
    {
        [Fact]
        public async Task GetBlogAsync_ValidId_ReturnTheBlogAsync()
        {
            // Arrange
            var blog = new Blog { Id = 1, Url = "https://sample.com" };
            var stubRepository = new Mock<IBlogRepository>();
            stubRepository
                .Setup(r => r.GetById(1))
                .ReturnsAsync(blog);
            var controller = new BlogsController(stubRepository.Object);

            // Act
            var result = await controller.GetBlogAsync(1);

            // Assert
            Assert.Equal(blog, result.Value);
        }

        [Fact]
        public async Task GetBlogAsync_UnkonwnId_ReturnNotFoundAsync()
        {
            // Arrange
            var blog = new Blog { Id = 1, Url = "https://sample.com" };
            var stubRepository = new Mock<IBlogRepository>();
            stubRepository
                .Setup(r => r.GetById(1))
                .ReturnsAsync(blog);
            var controller = new BlogsController(stubRepository.Object);

            // Act
            var result = await controller.GetBlogAsync(2);

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PostBlogAsync_ABlog_CreateANewBlogAsync()
        {
            // Arrange
            var blog = new Blog { Url = "https://sample.com" };
            var mockRepository = new Mock<IBlogRepository>();
            mockRepository
                .Setup(r => r.CreateAsync(blog))
                .ReturnsAsync((Blog b) =>
                {
                    b.Id = 1;
                    return b;
                });
            var controller = new BlogsController(mockRepository.Object);

            // Act
            var response = await controller.PostBlogAsync(blog);

            // Assert
            var actual = (response.Result as OkObjectResult).Value as Blog;
            Assert.NotEqual(0, actual.Id);
            Assert.Equal(blog.Url, actual.Url);
            mockRepository.Verify(r => r.CreateAsync(blog), Times.Once);
        }

        [Fact]
        public async Task PostBlogAsync_AppException_ReturnBadRequest()
        {
            // Arrange
            var message = "Bad request";
            var blog = new Blog { Url = "https://sample.com" };
            var mockRepository = new Mock<IBlogRepository>();
            mockRepository
                .Setup(r => r.CreateAsync(blog))
                .ThrowsAsync(new AppException(message));
            var controller = new BlogsController(mockRepository.Object);

            // Act
            var result = await controller.PostBlogAsync(blog);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
