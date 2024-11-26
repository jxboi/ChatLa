using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatlaapp.Backend.Models
{
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [ForeignKey("PostId")]
        public Post? Post { get; set; }

        [ForeignKey("Username")]
        public User? User { get; set; }
    }
} 