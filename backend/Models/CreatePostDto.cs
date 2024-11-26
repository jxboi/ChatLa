namespace chatlaapp.Backend.Models
{
    public class CreatePostDto
    {
        public string Content { get; set; }
        public string Username { get; set; }
        public IFormFile? Image { get; set; }
    }
} 