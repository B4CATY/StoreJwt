using API1.Data;
using API1.Models;
using API1.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API1.Repository.VideoCartRepository
{
    public class VideoCartRepository : IVideoCartRepository
    {
        private readonly VideoCardDbContext _context;
        public VideoCartRepository(VideoCardDbContext context)
        {
            _context = context;
        }

        public async Task<List<VideoCart>> GetAllVideoCarts()
        {
            var carts = await _context.Videocarts.Include(s=> s.Category).ToListAsync();
            return carts;
        }

        public Task<VideoCart> GetVideoCart(int id)
        {
            var videoCart = _context.Videocarts.Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id);
            return videoCart;
        }

        public async Task<List<VideoCart>> GetVideoCartsByCategory(string category)
        {
            var carts = await _context.Videocarts.Include(s=> s.Category.Name == category).ToListAsync();
            return carts;
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
