namespace GUI_Programmering_WebApi.Models
{
    public class CategoryDTO
    {
        public string CategoryName { get; set; }
    }

    public class CategoryWithIdDTO : CategoryDTO
    {
        public int CategoryId { get; set; }
    }
}
