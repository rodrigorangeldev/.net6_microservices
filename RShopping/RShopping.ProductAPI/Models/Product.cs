using RShopping.ProductAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace RShopping.ProductAPI.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = String.Empty;
        [Required]
        public decimal Price { get; set; }
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        [StringLength(30)]
        public string Categoryname { get; set; } = String.Empty;
        [StringLength(300)]
        public string ImageURL { get; set; } = String.Empty;
    }
}
