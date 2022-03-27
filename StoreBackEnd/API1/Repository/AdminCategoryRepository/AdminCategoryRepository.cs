using API1.Data;
using API1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace API1.Repository.AdminCategoryRepository
{
    public class AdminCategoryRepository : IAdminCategoryRepository
    {
        private readonly VideoCardDbContext _context;
        public AdminCategoryRepository(VideoCardDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCategory(Category category)
        {
            if(category == null) return false;

            var categoryCheck = GetCategoryByName(category.Name);
            if(categoryCheck == null)
            {
                _context.Categories.Add(category);
                await SaveChanges();
            }
            return true;
        }

        public Category GetCategoryByName(string name)
        {
            return _context.Categories.FirstOrDefault(x=> x.Name == name);
        }

        public async Task<bool> RemoveCategory(Category category)
        {
            if (category == null) 
                return false;

            _context.Categories.Remove(category);
            await SaveChanges();
            return true;
        }

        public async Task<bool> RemoveCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return false;

            await RemoveCategory(category);
            return true;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
