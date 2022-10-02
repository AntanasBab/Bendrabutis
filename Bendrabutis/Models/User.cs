using Bendrabutis.Models.Enums;

namespace Bendrabutis.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Room? Room { get; set; }
    }
}
