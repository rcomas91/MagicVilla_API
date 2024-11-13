using MagicVilla_API.Models;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Villa Playa",
                    Detail = "Detalles",
                    ImageUrl = "",
                    Ocupants = 3,
                    MetersCuadrados = 4,
                    Price = 233,
                    Amenity = "",
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                }

                );
        }
    }
}
