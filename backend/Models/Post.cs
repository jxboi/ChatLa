using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatlaapp.Backend.Models
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [ForeignKey("Username")]
        public User? User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
} 