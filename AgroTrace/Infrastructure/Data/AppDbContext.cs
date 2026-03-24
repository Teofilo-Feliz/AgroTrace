using AgroTrace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace AgroTrace.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        //Seguridad
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

        //Finca
        public DbSet<Finca> Fincas { get; set; }

        //Ganado
        public DbSet<Animal> Animales { get; set; }
        public DbSet<TipoAnimal> TipoAnimales { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<EstadoAnimal> EstadoAnimales { get; set; }

        //Producción
        public DbSet<Produccion> Producciones { get; set; }
        public DbSet<ProduccionDetalle> ProduccionDetalles { get; set; }
        public DbSet<TipoProduccion> TipoProducciones { get; set; }

        //Finanzas
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<TipoGasto> TipoGastos { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }

        //Salud
        public DbSet<Tratamiento> Tratamientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Username)
                .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(e => e.RolId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            });

            modelBuilder.Entity<Finca>(entity =>
            {
                entity.HasOne(f => f.UsuarioPropietario)
               .WithMany(u => u.Fincas)
               .HasForeignKey(f => f.UsuarioPropietarioId)
               .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Animal>(entity =>
           {
               entity.HasKey(e => e.Id);

               entity.Property(e => e.Codigo)
               .IsRequired()
               .HasMaxLength(100);

               entity.Property(e => e.Nombre)
              .IsRequired()
              .HasMaxLength(100);

               entity.Property(e => e.Sexo)
               .IsRequired()
               .HasMaxLength(15);

               entity.Property(e => e.Peso)
               .HasPrecision(10,2);

               entity.Property(e => e.FechaNacimiento)
               .IsRequired();

               entity.HasOne(a => a.Finca)
              .WithMany(f => f.Animales)
              .HasForeignKey(a => a.FincaId)
              .OnDelete(DeleteBehavior.Restrict);
              

               entity.HasOne(a => a.TipoAnimal)
               .WithMany(a => a.Animales)
               .HasForeignKey(a => a.TipoAnimalId)
               .OnDelete(DeleteBehavior.Restrict);


               entity.HasOne(a => a.Raza)
                   .WithMany(r => r.Animales)
                   .HasForeignKey(a => a.RazaId)
                   .OnDelete(DeleteBehavior.Restrict);


               entity.HasOne(a => a.EstadoAnimal)
                   .WithMany(e => e.Animales)
                   .HasForeignKey(a => a.EstadoAnimalId)
                   .OnDelete(DeleteBehavior.Restrict);


               entity.HasIndex(e => new { e.Codigo, e.FincaId })
                .IsUnique();


           });

            modelBuilder.Entity<Produccion>(entity =>
            {
                entity.HasOne(p => p.Finca)
                .WithMany()
                .HasForeignKey(p => p.FincaId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.TipoProduccion)
                    .WithMany()
                    .HasForeignKey(p => p.TipoProduccionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProduccionDetalle>(entity =>
            {
                entity.HasOne(d => d.Produccion)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.ProduccionId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Animal)
                    .WithMany(f => f.ProduccionDetalles)
                    .HasForeignKey(d => d.AnimalId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.HasOne(g => g.TipoGasto)
                 .WithMany(g => g.Gastos)
                 .HasForeignKey(g => g.TipoGastoId)
                 .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.Finca)
                    .WithMany(g => g.Gastos)
                    .HasForeignKey(g => g.FincaId);
            });

            modelBuilder.Entity<Tratamiento>(entity =>
            {
                entity.HasOne(t => t.Animal)
                 .WithMany(t => t.Tratamientos)
                 .HasForeignKey(t => t.AnimalId)
                 .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EstadoAnimal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasIndex(e => e.Nombre)
                .IsUnique();
                entity.HasMany(e => e.Animales)
                 .WithOne(a => a.EstadoAnimal)
                 .HasForeignKey(a => a.EstadoAnimalId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Raza>(entity =>
            {
                entity.HasKey (r => r.Id);
                entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasOne(r => r.TipoAnimal)
                .WithMany(t => t.Razas)
                .HasForeignKey(r => r.TipoAnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            });






        }
    }   
}
