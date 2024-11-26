namespace chatlaapp.Backend.Models
{
    public class CreatePostDto
    {
        public required string Content { get; set; }
        public required string Username { get; set; }
        public IFormFile? Image { get; set; }
    }
} 