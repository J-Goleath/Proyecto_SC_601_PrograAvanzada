namespace AutoFix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarInventarioYOrdenesTrabajo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdenesTrabajo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CitaSolicitudId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        MecanicoId = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                        DescripcionTrabajo = c.String(maxLength: 500),
                        Diagnostico = c.String(maxLength: 500),
                        Observaciones = c.String(maxLength: 500),
                        FechaAsignacion = c.DateTime(nullable: false),
                        FechaInicio = c.DateTime(),
                        FechaFinalizacion = c.DateTime(),
                        Prioridad = c.Int(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CitasSolicitud", t => t.CitaSolicitudId)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Clientes", t => t.MecanicoId)
                .Index(t => t.CitaSolicitudId)
                .Index(t => t.ClienteId)
                .Index(t => t.MecanicoId);
            
            CreateTable(
                "dbo.Repuestos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Codigo = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(maxLength: 500),
                        Stock = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Categoria = c.String(maxLength: 50),
                        Ubicacion = c.String(maxLength: 100),
                        FechaRegistro = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialesUsados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrdenTrabajoId = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        CostoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observaciones = c.String(maxLength: 500),
                        FechaUso = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdenesTrabajo", t => t.OrdenTrabajoId)
                .ForeignKey("dbo.Repuestos", t => t.RepuestoId)
                .Index(t => t.OrdenTrabajoId)
                .Index(t => t.RepuestoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialesUsados", "RepuestoId", "dbo.Repuestos");
            DropForeignKey("dbo.MaterialesUsados", "OrdenTrabajoId", "dbo.OrdenesTrabajo");
            DropForeignKey("dbo.OrdenesTrabajo", "MecanicoId", "dbo.Clientes");
            DropForeignKey("dbo.OrdenesTrabajo", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.OrdenesTrabajo", "CitaSolicitudId", "dbo.CitasSolicitud");
            DropIndex("dbo.MaterialesUsados", new[] { "RepuestoId" });
            DropIndex("dbo.MaterialesUsados", new[] { "OrdenTrabajoId" });
            DropIndex("dbo.OrdenesTrabajo", new[] { "MecanicoId" });
            DropIndex("dbo.OrdenesTrabajo", new[] { "ClienteId" });
            DropIndex("dbo.OrdenesTrabajo", new[] { "CitaSolicitudId" });
            DropTable("dbo.MaterialesUsados");
            DropTable("dbo.Repuestos");
            DropTable("dbo.OrdenesTrabajo");
        }
    }
}
