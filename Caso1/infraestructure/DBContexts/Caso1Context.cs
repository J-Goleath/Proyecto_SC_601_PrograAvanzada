using Caso1.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Caso1.infraestructure.DBContexts
{
    public class Caso1Context : DbContext
    {
        public Caso1Context() : base("name=Caso1DB") { }

        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            base.OnModelCreating(modelBuilder);
        }
    }
}