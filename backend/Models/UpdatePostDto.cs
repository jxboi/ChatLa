namespace chatlaapp.Backend.Models
{
    public class UpdatePostDto
    {
        public string Content { get; set; }
        public string Username { get; set; }
        public IFormFile? Image { get; set; }
    }
} 