using Microsoft.EntityFrameworkCore;
using backend_crud.Models;

namespace backend_crud.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<SalaDeJuntas> SalaDeJuntas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones adicionales de los modelos si es necesario
        }
    }
}