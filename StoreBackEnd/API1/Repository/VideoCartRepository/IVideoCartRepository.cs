using API1.Models;
using API1.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API1.Repository.VideoCartRepository
{
    public interface IVideoCartRepository : ISaveChanges
    {
        Task<VideoCart> GetVideoCart(int id);
        Task<List<VideoCart>> GetAllVideoCarts();
        Task<List<VideoCart>> GetVideoCartsByCategory(string category);
    }
}
