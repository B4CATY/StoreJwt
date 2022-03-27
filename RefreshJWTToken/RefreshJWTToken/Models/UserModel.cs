using System.ComponentModel.DataAnnotations;

namespace RefreshJWTToken.Models
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }
    }
}
