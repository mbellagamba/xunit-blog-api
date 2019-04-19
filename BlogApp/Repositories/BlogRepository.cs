using BlogApp.Abstractions;
using BlogApp.Exceptions;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BloggingContext _context;

        public BlogRepository(BloggingContext context)
        {
            _context = context;
        }

        async Task<Blog> IBlogRepository.CreateAsync(Blog blog)
        {
            if (!Uri.IsWellFormedUriString(blog.Url, UriKind.Absolute)) throw new AppException("Bad url");

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        async Task<Blog> IBlogRepository.GetById(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        async Task<List<Blog>> IBlogRepository.ListAsync()
        {
            return await _context.Blogs.ToListAsync();
        }

        async Task IBlogRepository.RemoveAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) throw new AppException("Not found");

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
    }
}
