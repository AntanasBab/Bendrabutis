namespace Bendrabutis.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public Floor Floor { get; set; }
        public int Number { get; set; }
        public int NumberOfLivingPlaces { get; set; }
        public double Area { get; set; }
        public List<User> Residents { get; set; } = new List<User>();
    }
}
