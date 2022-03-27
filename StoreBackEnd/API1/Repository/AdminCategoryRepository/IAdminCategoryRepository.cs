using API1.Models;
using System.Threading.Tasks;

namespace API1.Repository.AdminCategoryRepository
{
    public interface IAdminCategoryRepository : ISaveChanges
    {
        Task<bool> AddCategory(Category category);
        Task<bool> RemoveCategory(Category category);
        Task<bool> RemoveCategoryById(int id);
        Category GetCategoryByName(string name);
    }
}
