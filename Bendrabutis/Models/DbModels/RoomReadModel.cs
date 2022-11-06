namespace Bendrabutis.Models.DbModels
{
    public class RoomReadModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int NumberOfLivingPlaces { get; set; }
        public double Area { get; set; }
        public List<UserReadModel> Residents { get; set; }
    }
}
