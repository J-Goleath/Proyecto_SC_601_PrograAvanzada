using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class RepuestoConfiguration : EntityTypeConfiguration<Repuesto>
    {
        public RepuestoConfiguration()
        {
            ToTable("Repuestos");
            HasKey(r => r.Id);
            Property(r => r.Precio).HasPrecision(18, 2);
            Property(r => r.Codigo).IsRequired().HasMaxLength(50);
            Property(r => r.Nombre).IsRequired().HasMaxLength(100);
            Property(r => r.Categoria).HasMaxLength(50);
            Property(r => r.Ubicacion).HasMaxLength(100);
            Property(r => r.Descripcion).HasMaxLength(500);
        }
    }
}
