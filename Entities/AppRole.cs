using System.ComponentModel.DataAnnotations;

namespace BookstoreMS.Entities
{
    public class AppRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}