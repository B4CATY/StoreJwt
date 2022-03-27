using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API1.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<VideoCart> VideoCarts { get; set; }
    }
}
