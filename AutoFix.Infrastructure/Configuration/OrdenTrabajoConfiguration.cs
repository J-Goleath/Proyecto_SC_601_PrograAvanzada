using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class OrdenTrabajoConfiguration : EntityTypeConfiguration<OrdenTrabajo>
    {
        public OrdenTrabajoConfiguration()
        {
            ToTable("OrdenesTrabajo");
            HasKey(o => o.Id);
            Property(o => o.Estado).IsRequired().HasMaxLength(50);
            Property(o => o.DescripcionTrabajo).HasMaxLength(500);
            Property(o => o.Diagnostico).HasMaxLength(500);
            Property(o => o.Observaciones).HasMaxLength(500);

            HasRequired(o => o.CitaSolicitud)
                .WithMany()
                .HasForeignKey(o => o.CitaSolicitudId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Cliente)
                .WithMany()
                .HasForeignKey(o => o.ClienteId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Mecanico)
                .WithMany()
                .HasForeignKey(o => o.MecanicoId)
                .WillCascadeOnDelete(false);
        }
    }
}
