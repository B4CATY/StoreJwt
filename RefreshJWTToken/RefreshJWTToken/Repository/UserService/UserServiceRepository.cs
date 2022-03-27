using Microsoft.AspNetCore.Identity;
using RefreshJWTToken.Data;
using RefreshJWTToken.Models;
using RefreshJWTToken.Models.UserRoleModel;
using RefreshJWTToken.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefreshJWTToken.Repository.UserService
{
    public class UserServiceRepository : IUserServiceRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _db;

        public UserServiceRepository(UserManager<IdentityUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
            
        }
        public UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user)
        {
            _db.UserRefreshToken.Add(user);
            return user;
        }

        public bool EditName(ChangeUserNameViewModel model)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == model.OldUserName);
            if (user != null)
            {
                var checkName = _db.Users.FirstOrDefault(x => x.UserName == model.NewUserName);
                if (checkName is null)
                {
                    user.UserName = model.NewUserName;
                    user.NormalizedUserName = _userManager.NormalizeEmail(model.NewUserName);
                    _db.Users.Update(user);
                    return true;

                }

                throw new Exception("A user with this nickname already exists");


            }
            throw new Exception("User is not found");
            
        }

        public void DeleteUserRefreshTokens(string username, string refreshToken)
        {
            var item = _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken);
            
            if (item != null)
            {
                item.IsActive = false;
                _db.UserRefreshToken.Update(item);
            }
        }

        public UserRefreshTokens GetSavedRefreshTokens(string username, string refreshToken)
        {
            return _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true);
        }

        public async Task<UserRefreshWithrRolesModel> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await GetRoles(username);
            return new UserRefreshWithrRolesModel { Email = user.Email, Name = user.UserName, Role = roles[0] };
        }
        public async Task<List<string>> GetRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        public async Task<bool> IsValidUserAsync(UserModel user)
        {
            var dbUSer = _userManager.Users.FirstOrDefault(o => o.UserName == user.Name);
            var result = await _userManager.CheckPasswordAsync(dbUSer, user.Password);
            return result;
        }
        public int SaveCommit()
        {
            return _db.SaveChanges();
        }
    }
}
