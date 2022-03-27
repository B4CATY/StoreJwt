using API1.Models;
using API1.Repository.AdminVideoCartRepository;
using API1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API1.Controllers
{
    [Route("admin/videocarts")]
    [ApiController]
    public class AdminVideoCartsController : ControllerBase
    {
        private readonly IAdminVideoCartRepository _adminVideoCartRepository;
        public AdminVideoCartsController(IAdminVideoCartRepository adminVideoCartRepository)
        {
            _adminVideoCartRepository = adminVideoCartRepository;
        }
        // PUT: /VideoCarts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVideoCart(int id, VideoCartViewModel videoCart)
        {

            if (id != videoCart.Id)
                return BadRequest();

            bool isUpdate = await _adminVideoCartRepository.UpdateVideoCart(videoCart);
            if (isUpdate)
            {
                return Ok();
            }
            return NoContent();
        }
        // POST: /VideoCarts/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<VideoCart>> CreateNewVideoCart(VideoCartViewModel videoCart)
        {
            await _adminVideoCartRepository.AddVideoCart(videoCart);

            return Ok();
        }

        // DELETE: /VideoCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoCart([FromBody]int id)
        {
            var isRemove = await _adminVideoCartRepository.RemoveVideocartCart(id);
            if (!isRemove)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
