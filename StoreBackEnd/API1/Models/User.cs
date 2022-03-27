using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API1.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
