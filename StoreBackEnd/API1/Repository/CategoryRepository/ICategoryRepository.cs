using API1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API1.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int id);
        Task<List<Category>> GetCategories();
    }
}
