using System;

namespace API1.Models
{
    public class CartVideoCart
    {
        public DateTime OrderDate { get; set; }

        public int VideoCartId { get; set; }
        public VideoCart VideoCart { get; set; }

        public int CartId {  get; set; }
        public Cart Cart { get; set; }
    }
}
