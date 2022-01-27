using System.ComponentModel.DataAnnotations;

namespace RShopping.ProductAPI.Models.Base
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; } 
    }
}
