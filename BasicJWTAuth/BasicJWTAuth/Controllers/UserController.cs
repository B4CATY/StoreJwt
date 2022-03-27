using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasicJWTAuth.Repository;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BasicJWTAuth.Models;

namespace BasicJWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IJWTManagerRepository _repository;
        public UserController(IJWTManagerRepository repository)
        {
            _repository = repository;

        }
        [HttpGet]
        [Route("userlist")]
        public List<string> Get()
        {
            var users = new List<string>
            {
                "Satinder Singh",
                "Amit Sarna",
                "Davin Jon"
            };

            return users;
        }
        [HttpGet]
        [Route("user")]
        public string GetUser()
        {
            return User.Identity.Name;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(Users usersdata)
        {
            var token = _repository.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
