using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API1.Models
{
    public partial class VideoCart
    {
        public int Id { get; set; }
        public int? Categoryid { get; set; }
        [Required]
        public string? NameProduct { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }

        public virtual Category? Category { get; set; }
        public ICollection<Cart> Carts { get; set; }
        //public List<CartVideoCart> CartVideoCarts { get; set; }
    }
}
