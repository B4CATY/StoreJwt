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
using API1.Repository.VideoCartRepository;

namespace API1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoCartsController : ControllerBase
    {
        private readonly IVideoCartRepository _videoCartRepository;

        public VideoCartsController(IVideoCartRepository videoCartRepository)
        {
            _videoCartRepository = videoCartRepository;
        }

        // GET: /VideoCarts
        [HttpGet]
        public async Task<IActionResult> GetVideocarts()
        {
            var videoCarts = await _videoCartRepository.GetAllVideoCarts();
            return Ok(videoCarts);
        }

        // GET: /VideoCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoCart>> GetVideoCart(int id)
        {
            var videoCart = await _videoCartRepository.GetVideoCart(id);
            
            if (videoCart == null)
            {
                return NotFound();
            }

            return Ok(videoCart);
        }
        
        
        

        
    }
}
