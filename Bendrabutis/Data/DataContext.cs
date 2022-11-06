using Bendrabutis.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bendrabutis.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Dormitory> Dormitories { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options){ }
    }
}
