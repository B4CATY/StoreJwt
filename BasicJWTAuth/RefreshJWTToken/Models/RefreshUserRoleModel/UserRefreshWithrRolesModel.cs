using System.Collections.Generic;

namespace RefreshJWTToken.Models.UserRoleModel
{
    public class UserRefreshWithrRolesModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRefreshTokens RefreshToken { get; set; }
        public string Role{ get; set; }
    }
}
