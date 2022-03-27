using Microsoft.AspNetCore.Identity;
using RefreshJWTToken.Models;
using RefreshJWTToken.Models.UserRoleModel;
using RefreshJWTToken.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefreshJWTToken.Repository.UserService
{
    public interface IUserServiceRepository
    {
		Task<bool> IsValidUserAsync(UserModel users);
		UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);
		bool EditName(ChangeUserNameViewModel model);
		UserRefreshTokens GetSavedRefreshTokens(string username, string refreshtoken);
		void DeleteUserRefreshTokens(string username, string refreshToken);
		Task<UserRefreshWithrRolesModel> GetUser(string username);
		Task<List<string>> GetRoles(string username);
		int SaveCommit();
	}
}
