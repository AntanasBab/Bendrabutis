using Bendrabutis.Models;
using Microsoft.EntityFrameworkCore;

namespace Bendrabutis.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Dormatory> Dormatories { get; set; }
    }
}
