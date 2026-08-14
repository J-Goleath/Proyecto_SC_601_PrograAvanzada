using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class VehiculoConfiguration : EntityTypeConfiguration<Vehiculo>
    {
        public VehiculoConfiguration()
        {
            ToTable("Vehiculos");
            HasKey(v => v.Id);
            Property(v => v.Placa).IsRequired().HasMaxLength(20);
            Property(v => v.Marca).IsRequired().HasMaxLength(50);
            Property(v => v.Modelo).IsRequired().HasMaxLength(50);

            HasRequired(v => v.Cliente)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.ClienteId)
                .WillCascadeOnDelete(false);
        }
    }
}
