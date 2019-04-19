using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Abstractions;
using BlogApp.Exceptions;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;

        public BlogsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogsAsync()
        {
            return await _blogRepository.ListAsync();
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlogAsync(int id)
        {
            var blog = await _blogRepository.GetById(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        // POST: api/Blogs
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlogAsync(Blog blog)
        {
            try
            {
                var newBlog = await _blogRepository.CreateAsync(blog);
                return Ok(newBlog);
            }
            catch(AppException e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogAsync(int id)
        {
            await _blogRepository.RemoveAsync(id);
            return Ok();
        }
    }
}
