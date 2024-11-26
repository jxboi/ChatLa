namespace chatlaapp.Backend.Models
{
    public class UpdatePostDto
    {
        public required string Content { get; set; }
        public required string Username { get; set; }
        public IFormFile? Image { get; set; }
    }
} 