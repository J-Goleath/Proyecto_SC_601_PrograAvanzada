using AutoFix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
            
            modelBuilder.Entity<Vehiculo>()
                .HasRequired(v => v.Cliente)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.ClienteId)
                .WillCascadeOnDelete(false);

            
            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.Placa)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.Modelo)
                .IsRequired()
                .HasMaxLength(50);

            
            modelBuilder.Entity<Cliente>()
                .Property(c => c.Correo)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Telefono)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Contraseña)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<CitaSolicitud>()
    .HasRequired(c => c.Vehiculo)
    .WithMany()
    .HasForeignKey(c => c.VehiculoId)
    .WillCascadeOnDelete(false);

            modelBuilder.Entity<CitaSolicitud>()
                .HasOptional(c => c.Mecanico)
                .WithMany()
                .HasForeignKey(c => c.MecanicoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CitaSolicitud>()
                .Property(c => c.DescripcionFallos)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Notificacion>()
                .HasRequired(n => n.Cliente)
                .WithMany()
                .HasForeignKey(n => n.ClienteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notificacion>()
                .Property(n => n.Mensaje)
                .IsRequired()
                .HasMaxLength(250);
            // Configuración de OrdenTrabajo
            modelBuilder.Entity<OrdenTrabajo>()
                .HasRequired(o => o.CitaSolicitud)
                .WithMany()
                .HasForeignKey(o => o.CitaSolicitudId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrdenTrabajo>()
                .HasRequired(o => o.Cliente)
                .WithMany()
                .HasForeignKey(o => o.ClienteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrdenTrabajo>()
                .HasRequired(o => o.Mecanico)
                .WithMany()
                .HasForeignKey(o => o.MecanicoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrdenTrabajo>()
                .Property(o => o.Estado)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<OrdenTrabajo>()
                .Property(o => o.DescripcionTrabajo)
                .HasMaxLength(500);

            modelBuilder.Entity<OrdenTrabajo>()
                .Property(o => o.Diagnostico)
                .HasMaxLength(500);

            modelBuilder.Entity<OrdenTrabajo>()
                .Property(o => o.Observaciones)
                .HasMaxLength(500);

            // Configuración de Repuesto
            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Categoria)
                .HasMaxLength(50);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Ubicacion)
                .HasMaxLength(100);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Descripcion)
                .HasMaxLength(500);

            // Configuración de MaterialUsado
            modelBuilder.Entity<MaterialUsado>()
                .Property(m => m.CostoUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MaterialUsado>()
                .Property(m => m.Observaciones)
                .HasMaxLength(500);

            modelBuilder.Entity<MaterialUsado>()
                .HasRequired(m => m.OrdenTrabajo)
                .WithMany(o => o.MaterialesUsados)
                .HasForeignKey(m => m.OrdenTrabajoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MaterialUsado>()
                .HasRequired(m => m.Repuesto)
                .WithMany(r => r.MaterialesUsados)
                .HasForeignKey(m => m.RepuestoId)
                .WillCascadeOnDelete(false);



            base.OnModelCreating(modelBuilder);
        }
    }
}


