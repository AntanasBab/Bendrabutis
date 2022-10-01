using Bendrabutis.Models.Enums;

namespace Bendrabutis.Models
{
    public class Request
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public RequestType RequestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
    }
}
