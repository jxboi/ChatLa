using System.ComponentModel.DataAnnotations;

namespace chatlaapp.Backend.Models
{
    public class User
    {
        [Key]
        [Required]
        public string Username { get; set; } = string.Empty;
    }
} 