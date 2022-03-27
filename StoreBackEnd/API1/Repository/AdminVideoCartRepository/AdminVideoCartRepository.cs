using API1.Data;
using API1.Models;
using API1.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API1.Repository.AdminVideoCartRepository
{
    public class AdminVideoCartRepository : IAdminVideoCartRepository
    {
        private readonly VideoCardDbContext _context;
        public AdminVideoCartRepository(VideoCardDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RemoveVideocartCart(int id)
        {
            var videoCart = await _context.Videocarts.FindAsync(id);
            if (videoCart != null)
            {
                _context.Videocarts.Remove(videoCart);
                await SaveChanges();
                
                return true;
            }
            return false;

        }
        public async Task<bool> AddVideoCart(VideoCartViewModel videoCart)
        {
            var category = FindCategory(videoCart.Category);
            if(category != null)
            {
                VideoCart videoCartDb = new VideoCart
                {
                    NameProduct = videoCart.Name,
                    Description = videoCart.Description,
                    Price = videoCart.Price,
                    Category = category,
                };
                _context.Videocarts.Add(videoCartDb);
                await SaveChanges();
                return true;
            }
            return false;

        }

        public async Task<bool> UpdateVideoCart(VideoCartViewModel videoCart)
        {
            
            var videoCartDb = _context.Videocarts.Include(s => s.Category).FirstOrDefault(x => x.Id == videoCart.Id);
            if (videoCartDb != null)
            {
                if(videoCartDb.Category.Name != videoCart.Category)
                {
                    videoCartDb.Category = FindCategory(videoCart.Category);
                }
                videoCartDb.Description = videoCart.Description;
                videoCartDb.NameProduct = videoCart.Name;
                videoCartDb.Price = videoCart.Price;
                _context.Videocarts.Update(videoCartDb);
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
        public Category FindCategory(string name)
        {
            return _context.Categories.FirstOrDefault(context => context.Name == name);
        }
       
    }
}
