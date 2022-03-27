using RefreshJWTToken.Models;
using RefreshJWTToken.Models.UserRoleModel;
using System.Security.Claims;

namespace RefreshJWTToken.Repository.JWT
{
	public interface IJWTManagerRepository
	{
		Tokens GenerateToken(UserRefreshWithrRolesModel userAndRoleModel);
		Tokens GenerateRefreshToken(UserRefreshWithrRolesModel userAndRoleModel);
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}
