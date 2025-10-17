namespace GUI_Programmering_WebApi.Models
{
    public class ImageDTO
    {
        public string ImageName { get; set; } = null!;
        public string? ImageUrl { get; set; }   
    }

    public class ImageDTOWithId : ImageDTO
    {
        public int ImageId { get; set; }
    }
}
