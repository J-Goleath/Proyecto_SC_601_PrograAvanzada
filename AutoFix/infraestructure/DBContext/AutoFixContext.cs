using AutoFix.Entities;
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

            base.OnModelCreating(modelBuilder);
        }
    }
}