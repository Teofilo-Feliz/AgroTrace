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
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Finca> Fincas { get; set; }
        public DbSet<Animal> Animales { get; set; }
        public DbSet<TipoAnimal> TipoAnimales { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<EstadoAnimal> EstadoAnimales { get; set; }
        public DbSet<Produccion> Producciones { get; set; }
        public DbSet<ProduccionDetalle> ProduccionDetalles { get; set; }
        public DbSet<TipoProduccion> TipoProducciones { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<TipoGasto> TipoGastos { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios", "Seguridad");
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
                entity.ToTable("Fincas", "Ganaderia");
                entity.HasIndex(e => e.UsuarioPropietarioId);
                entity.HasKey(e => e.Id);
                entity.HasOne(f => f.UsuarioPropietario)
               .WithMany(u => u.Fincas)
               .HasForeignKey(f => f.UsuarioPropietarioId)
               .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Animal>(entity =>
           {
               entity.ToTable("Animales", "Ganaderia");
               entity.HasKey(e => e.Id);
               entity.HasIndex(e => e.FincaId);
               entity.HasIndex(e => e.TipoAnimalId);
               entity.HasIndex(e => e.RazaId);
               entity.HasIndex(e => e.EstadoAnimalId);
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
                entity.ToTable("Producciones", "Produccion");
                entity.HasIndex(e => e.FincaId);
                entity.HasIndex(e => e.TipoProduccionId);
                entity.HasOne(p => p.Finca)
                .WithMany(p => p.Producciones)
                .HasForeignKey(p => p.FincaId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(p => new { p.FincaId, p.Fecha });
                entity.HasOne(p => p.TipoProduccion)
                    .WithMany()
                    .HasForeignKey(p => p.TipoProduccionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProduccionDetalle>(entity =>
            {
                entity.ToTable("ProduccionDetalles", "Produccion");
                entity.HasIndex(e => e.ProduccionId);
                entity.HasIndex(e => e.AnimalId);
                entity.Property(p => p.Cantidad)
                .HasPrecision(10, 2);
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
                entity.ToTable("Gastos", "Finanzas");
                entity.HasIndex(e => e.FincaId);
                entity.HasIndex(e => e.TipoGastoId);
                entity.HasOne(g => g.TipoGasto)
                 .WithMany(g => g.Gastos)
                 .HasForeignKey(g => g.TipoGastoId)
                 .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(g => new { g.FincaId, g.Fecha });
                entity.HasOne(g => g.Finca)
                    .WithMany(g => g.Gastos)
                    .HasForeignKey(g => g.FincaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tratamiento>(entity =>
            {
                entity.ToTable("Tratamientos", "Sanidad");
                entity.HasIndex(e => e.AnimalId);
                entity.Property(t => t.Costo)
                .HasPrecision(10,2);
                entity.HasOne(t => t.Animal)
                 .WithMany(t => t.Tratamientos)
                 .HasForeignKey(t => t.AnimalId)
                 .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<EstadoAnimal>(entity =>
            {
                entity.ToTable("EstadoAnimales", "Ganaderia");
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
                entity.ToTable("Razas", "Ganaderia");
                entity.HasKey (r => r.Id);
                entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasOne(r => r.TipoAnimal)
                .WithMany(t => t.Razas)
                .HasForeignKey(r => r.TipoAnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.ToTable("Ingresos", "Finanzas");
                entity.HasIndex(e => e.FincaId);
                entity.HasKey(r => r.Id);
                entity.Property (r => r.Monto)
                .IsRequired()
                .HasPrecision(10,2);
                entity.Property(r => r.Fecha)
                .IsRequired();
                entity.HasIndex(i => new { i.FincaId, i.Fecha });
                entity.Property (r => r.Descripcion)
                .IsRequired()
                .HasMaxLength(255);
                entity.HasOne(i => i.Finca)
               .WithMany(f => f.Ingresos)
               .HasForeignKey(i => i.FincaId)
               .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Roles", "Seguridad");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Nombre) 
                .IsRequired()
                .HasMaxLength (50);
                entity.HasIndex(r => r.Nombre)
                .IsUnique();
                entity.Property(r => r.Descripcion)
                .HasMaxLength(255);


            });

            modelBuilder.Entity<TipoAnimal>(entity =>
            {
                entity.ToTable("TiposAnimales", "Ganaderia");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasIndex(r => r.Nombre)
                .IsUnique();
                entity.HasMany(t => t.Animales)
                .WithOne(a => a.TipoAnimal)
                .HasForeignKey(a => a.TipoAnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TipoGasto>(entity =>
            {
                entity.ToTable("TiposGastos", "Finanzas");
                entity.HasKey(R => R.Id);
                entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasIndex (r => r.Nombre)
                .IsUnique();
                entity.HasMany(t => t.Gastos)
                .WithOne(g => g.TipoGasto)
                .HasForeignKey(g => g.TipoGastoId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TipoProduccion>(entity =>
            {
                entity.ToTable("TiposProduciones", "Produccion");
                entity.HasKey (r => r.Id);
                entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                entity.HasIndex(r => r.Nombre)
                .IsUnique();
                entity.HasMany(t => t.Producciones)
                .WithOne(p => p.TipoProduccion)
                .HasForeignKey(p => p.TipoProduccionId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken", "Seguridad");
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


        }
    }   
}
