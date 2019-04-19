using BlogApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Abstractions
{
    public interface IBlogRepository
    {
        Task<List<Blog>> ListAsync();
        Task<Blog> CreateAsync(Blog blog);
        Task<Blog> GetById(int id);
        Task RemoveAsync(int id);
    }
}
