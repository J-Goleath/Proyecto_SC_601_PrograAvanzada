using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class ClienteConfiguration : EntityTypeConfiguration<Cliente>
    {
        public ClienteConfiguration()
        {
            ToTable("Clientes");
            HasKey(c => c.Id);
            Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            Property(c => c.Correo).IsRequired().HasMaxLength(100);
            Property(c => c.Telefono).IsRequired().HasMaxLength(20);
            Property(c => c.Contraseña).IsRequired().HasMaxLength(100);
        }
    }
}
