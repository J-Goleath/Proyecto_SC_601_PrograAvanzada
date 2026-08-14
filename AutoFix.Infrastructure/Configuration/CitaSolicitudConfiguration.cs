using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class CitaSolicitudConfiguration : EntityTypeConfiguration<CitaSolicitud>
    {
        public CitaSolicitudConfiguration()
        {
            ToTable("CitasSolicitud");
            HasKey(c => c.Id);
            Property(c => c.DescripcionFallos).IsRequired().HasMaxLength(500);

            HasRequired(c => c.Vehiculo)
                .WithMany()
                .HasForeignKey(c => c.VehiculoId)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.Mecanico)
                .WithMany()
                .HasForeignKey(c => c.MecanicoId)
                .WillCascadeOnDelete(false);
        }
    }
}
