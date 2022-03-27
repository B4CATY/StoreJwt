using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API1.Data;
using API1.Models;
using API1.ViewModels;

namespace API1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoCartsController : ControllerBase
    {
        private readonly VideoCardDbContext _context;

        public VideoCartsController(VideoCardDbContext context)
        {
            _context = context;
        }

        // GET: /VideoCarts
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<VideoCart>>> GetVideocarts()
        {
            var VideoCarts = await _context.Videocarts
               .Join(_context.Categories,
               vid => vid.Categoryid,
               cat => cat.Id,
               (vid, cat) => new
               {
                   Id = vid.Id,
                   Name = vid.NameProduct,
                   Description = vid.Description,
                   Category = cat.Name,
                   Price = vid.Price
               }).ToListAsync();
            return Ok(VideoCarts);
        }

        // GET: /VideoCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoCart>> GetVideoCart(int id)
        {
            var videoCart = await _context.Videocarts.FindAsync(id);
            
            if (videoCart == null)
            {
                return NotFound();
            }

            return Ok(videoCart);
        }

        // PUT: /VideoCarts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideoCart(int id, VideoCart videoCart)
        {
            if (id != videoCart.Id)
            {
                return BadRequest();
            }

            _context.Entry(videoCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoCartExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: /VideoCarts
        [HttpPost]
        public async Task<ActionResult<VideoCart>> PostVideoCart(VideoCart videoCart)
        {
            _context.Videocarts.Add(videoCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoCart", new { id = videoCart.Id }, videoCart);
        }

        // DELETE: /VideoCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoCart(int id)
        {
            var videoCart = await _context.Videocarts.FindAsync(id);
            if (videoCart == null)
            {
                return NotFound();
            }

            _context.Videocarts.Remove(videoCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoCartExists(int id)
        {
            return _context.Videocarts.Any(e => e.Id == id);
        }
    }
}
