using System;
using System.ComponentModel.DataAnnotations;

namespace RefreshJWTToken.Models
{
    public class UserRefreshTokens
    {
		[Key]
		public int Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public string RefreshToken { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime ValidityPeriod { get; set; } = DateTime.Now.AddMonths(6);
	}
}
