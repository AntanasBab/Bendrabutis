namespace Bendrabutis.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int NumberOfLivingPlaces { get; set; }
        public double Area { get; set; }
        public IEnumerable<User> Residents { get; set; }
    }
}
