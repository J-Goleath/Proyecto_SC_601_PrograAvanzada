using AutoFix.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace AutoFix.Infrastructure.Configuration
{
    public class NotificacionConfiguration : EntityTypeConfiguration<Notificacion>
    {
        public NotificacionConfiguration()
        {
            ToTable("Notificaciones");
            HasKey(n => n.Id);
            Property(n => n.Mensaje).IsRequired().HasMaxLength(250);

            HasRequired(n => n.Cliente)
                .WithMany()
                .HasForeignKey(n => n.ClienteId)
                .WillCascadeOnDelete(false);
        }
    }
}
