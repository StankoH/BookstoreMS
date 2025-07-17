using System.ComponentModel.DataAnnotations;

namespace BookstoreMS.Entities
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public AppRole Role { get; set; } = null!;
    }
}