using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatlaapp.Backend.Models
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [ForeignKey("PostId")]
        public Post? Post { get; set; }

        [ForeignKey("Username")]
        public User? User { get; set; }
    }
} 