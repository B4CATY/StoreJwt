using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefreshJWTToken.Models;
using RefreshJWTToken.Models.UserRoleModel;
using RefreshJWTToken.Repository.JWT;
using RefreshJWTToken.Repository.UserService;
using RefreshJWTToken.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefreshJWTToken.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IJWTManagerRepository _jWTManager;
		private readonly IUserServiceRepository _userServiceRepository;
		private readonly UserManager<IdentityUser> _userManager;

		public UsersController(IJWTManagerRepository jWTManager, IUserServiceRepository userServiceRepository, UserManager<IdentityUser> userManager)
		{
			_jWTManager = jWTManager;
			_userServiceRepository = userServiceRepository;
			_userManager = userManager;
		}

		//[Authorize(Roles = "admin")]
		[HttpGet]
		[Route("userinfo")]
		public async Task<object> GetUser([FromBody] string userName)
		{
			var user = await _userServiceRepository.GetUser(userName);
			return new { Name = user.Name, Roles = user.Role, Email = user.Email };
		}

		[HttpPut]
		[Route("editname")]
		public IActionResult EditName(ChangeUserNameViewModel model)
		{
            try
            {
				_userServiceRepository.EditName(model);
				_userServiceRepository.SaveCommit();
				return Ok(model.NewUserName);
			}
            catch (Exception ex)
            {
				return BadRequest(ex.Message);
			}
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login(UserModel user)
		{
			if (user == null) 
				return NotFound();

			var token = await AuthenticateAsync(user);
			return Ok(new { Token = token, User = user.Name });

		}

		[AllowAnonymous]
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(UserModel user)
        {
			if (user == null) return BadRequest();

			var result = await _userManager.CreateAsync(new IdentityUser { UserName = user.Name, Email = user.Email}, user.Password);
			var userDb = await _userManager.FindByNameAsync(user.Name);
			await _userManager.AddToRoleAsync(userDb, "user");

			if (result.Succeeded)
			{
				var token = await AuthenticateAsync(user);
				return Ok(new { Token = token, User = user.Name });
			}
			return NotFound();	
		}

		
		private async Task<Tokens> AuthenticateAsync(UserModel usersdata)
		{
			var validUser = await _userServiceRepository.IsValidUserAsync(usersdata);

			if (!validUser)
			{
				return null;
			}
			
			var roles = await _userServiceRepository.GetRoles(usersdata.Name);
			var token = _jWTManager.GenerateToken(new UserRefreshWithrRolesModel { Name = usersdata.Name, Role = roles[0], Email = usersdata.Email });

			if (token == null)
			{
				return null;
			}

			// saving refresh token to the db
			UserRefreshTokens obj = new UserRefreshTokens
			{
				RefreshToken = token.Refresh_Token,
				UserName = usersdata.Name
			};
			_userServiceRepository.AddUserRefreshTokens(obj);
			_userServiceRepository.SaveCommit();
			return token;
		}

		[AllowAnonymous]
		[HttpDelete]
		[Route("logout")]
		public IActionResult LogOut(Tokens token)
        {
			var principal = _jWTManager.GetPrincipalFromExpiredToken(token.Access_Token);
			if (principal == null)
				return Unauthorized("Invalid JWT!");

			_userServiceRepository.DeleteUserRefreshTokens(principal.Identity?.Name, token.Refresh_Token);
			
			_userServiceRepository.SaveCommit();
			return Unauthorized("You are not log in");
		}

		/*[AllowAnonymous]
		[HttpPost]
		[Route("authenticate")]
		public async Task<IActionResult> AuthenticatEEAsync(Users usersdata)
		{
			var validUser = await _userServiceRepository.IsValidUserAsync(usersdata);

			if (!validUser)
			{
				return Unauthorized("Incorrect username or password!");
			}

			var token = _jWTManager.GenerateToken(usersdata.Name);

			if (token == null)
			{
				return Unauthorized("Invalid Attempt!");
			}

			// saving refresh token to the db
			UserRefreshTokens obj = new UserRefreshTokens
			{
				RefreshToken = token.Refresh_Token,
				UserName = usersdata.Name
			};

			_userServiceRepository.AddUserRefreshTokens(obj);
			_userServiceRepository.SaveCommit();
			return Ok(token);
		}*/

		[AllowAnonymous]
		[HttpPost]
		[Route("refresh")]
		public async Task <IActionResult> Refresh(Tokens token)
		{
			var principal = _jWTManager.GetPrincipalFromExpiredToken(token.Access_Token);
			if(principal == null) 
				return Unauthorized("Invalid JWT!");

			var username = principal.Identity?.Name;

			//retrieve the saved refresh token from database
			var savedRefreshToken = _userServiceRepository.GetSavedRefreshTokens(principal.Identity?.Name, token.Refresh_Token);

			if (savedRefreshToken is null || savedRefreshToken.RefreshToken != token.Refresh_Token )
			{
				return Unauthorized("Invalid attempt!");
			}
			
			var user = await _userServiceRepository.GetUser(username);
			user.RefreshToken = savedRefreshToken;

			var newJwtToken = _jWTManager.GenerateRefreshToken(user);

			if (newJwtToken == null)
			{
				return Unauthorized("Invalid attempt!");
			}


			if (token.Refresh_Token != newJwtToken.Refresh_Token)
			{
				_userServiceRepository.DeleteUserRefreshTokens(username, token.Refresh_Token); //disactiv old token
				_userServiceRepository.AddUserRefreshTokens(new UserRefreshTokens // saving refresh token to the db
				{
					RefreshToken = newJwtToken.Refresh_Token,
					UserName = username
				});
				_userServiceRepository.SaveCommit();
			}

			return Ok(newJwtToken);
		}

		
	}
}
