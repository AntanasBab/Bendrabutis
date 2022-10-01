namespace Bendrabutis.Models
{
    public class Dormitory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomCapacity { get; set; }
        public List<Floor> Floors { get; set; }
    }
}
