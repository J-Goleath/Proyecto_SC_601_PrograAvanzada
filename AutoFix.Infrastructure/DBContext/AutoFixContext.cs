using AutoFix.Domain.Entities;
using AutoFix.Infrastructure.Configuration;
using System.Data.Entity;

namespace AutoFix.infraestructure.DBContext
{
    public class AutoFixContext : DbContext
    {
        public AutoFixContext() : base("name=AutoFixDB") { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<CitaSolicitud> CitasSolicitud { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<OrdenTrabajo> OrdenesTrabajo { get; set; }
        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<MaterialUsado> MaterialesUsados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClienteConfiguration());
            modelBuilder.Configurations.Add(new VehiculoConfiguration());
            modelBuilder.Configurations.Add(new CitaSolicitudConfiguration());
            modelBuilder.Configurations.Add(new NotificacionConfiguration());
            modelBuilder.Configurations.Add(new OrdenTrabajoConfiguration());
            modelBuilder.Configurations.Add(new RepuestoConfiguration());
            modelBuilder.Configurations.Add(new MaterialUsadoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
