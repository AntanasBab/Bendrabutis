using System.ComponentModel.DataAnnotations;
using Bendrabutis.Models.Enums;
using Bendrabutis.Models.Interfaces;

namespace Bendrabutis.Entities
{
    public class Request : IUserOwnedResource
    {
        public int Id { get; set; }
        public RequestType RequestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        public User Author { get; set; }
    }
}
