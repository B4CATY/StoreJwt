using API1.Models;
using API1.ViewModels;
using System.Threading.Tasks;

namespace API1.Repository.AdminVideoCartRepository
{
    public interface IAdminVideoCartRepository : ISaveChanges
    {
        Task<bool> RemoveVideocartCartById(int id);
        Task<bool> RemoveVideocartCart(VideoCart videoCart);
        Task<bool> AddVideoCart(VideoCartViewModel videoCart);
        Category FindCategory(string name);
    }
}
