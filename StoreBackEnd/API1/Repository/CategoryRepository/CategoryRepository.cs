using API1.Data;
using API1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API1.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly VideoCardDbContext _context;
        public CategoryRepository(VideoCardDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories.Include(x=>x.Products).ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories.Include(x=> x.Products).FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }
    }
}
