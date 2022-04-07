using System.ComponentModel.DataAnnotations;

namespace two_realities.Models
{
    public class User
    {
        [Key]
        public string? UserId { get; set; }

        public byte[]? PasswordHash  { get; set; }
        public byte[]? PasswordSalt { get; set; }    
    }
}
