using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class MaterialUsadoConfiguration : EntityTypeConfiguration<MaterialUsado>
    {
        public MaterialUsadoConfiguration()
        {
            ToTable("MaterialesUsados");
            HasKey(m => m.Id);
            Property(m => m.CostoUnitario).HasPrecision(18, 2);
            Property(m => m.Observaciones).HasMaxLength(500);

            HasRequired(m => m.OrdenTrabajo)
                .WithMany(o => o.MaterialesUsados)
                .HasForeignKey(m => m.OrdenTrabajoId)
                .WillCascadeOnDelete(false);

            HasRequired(m => m.Repuesto)
                .WithMany(r => r.MaterialesUsados)
                .HasForeignKey(m => m.RepuestoId)
                .WillCascadeOnDelete(false);
        }
    }
}
