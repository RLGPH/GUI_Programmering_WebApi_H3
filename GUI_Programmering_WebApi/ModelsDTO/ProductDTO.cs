namespace GUI_Programmering_WebApi.Models
{
    public class ProductDTO
    {
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; }
        
        public decimal ProductPrice { get; set; }

        public int CategoryId { get; set; }

        public int ImageId { get; set; }
    }

    public class ProductWithIdDTO : ProductDTO
    {
        public int ProductId { get; set; }
    }

    public class ProductWithRelationDTO : ProductWithIdDTO
    {
        public CategoryDTO? Category { get; set; }
    }

    public class ProductWithImageDTO : ProductWithRelationDTO
    {
        public ImageDTO? Image { get; set; } = null!;
    }
}
