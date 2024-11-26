using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chatlaapp.Backend.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FollowerUsername { get; set; } = string.Empty;

        [Required]
        public string FollowingUsername { get; set; } = string.Empty;

        [ForeignKey("FollowerUsername")]
        public User? Follower { get; set; }

        [ForeignKey("FollowingUsername")]
        public User? Following { get; set; }
    }
} 