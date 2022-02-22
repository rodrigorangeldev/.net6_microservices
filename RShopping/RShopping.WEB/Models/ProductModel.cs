namespace RShopping.WEB.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Categoryname { get; set; } = String.Empty;
        public string ImageURL { get; set; } = String.Empty;
    }
}
