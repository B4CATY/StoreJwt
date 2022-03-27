using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace API1.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        [DisplayName("Category Name")]
        public string? Name { get; set; }
        //[JsonIgnore]
        public virtual ICollection<VideoCart> Products { get; set; }
    }
}
