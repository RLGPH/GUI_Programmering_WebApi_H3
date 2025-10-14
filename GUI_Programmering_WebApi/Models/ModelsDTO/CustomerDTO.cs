namespace GUI_Programmering_WebApi.Models
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }
    }
}
