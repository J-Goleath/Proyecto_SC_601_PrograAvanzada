namespace AutoFix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModulosIntegrante2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistorialVehicular",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehiculoId = c.Int(nullable: false),
                        OrdenTrabajoId = c.Int(),
                        DescripcionServicio = c.String(nullable: false, maxLength: 500),
                        FechaServicio = c.DateTime(nullable: false),
                        Observaciones = c.String(maxLength: 500),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdenesTrabajo", t => t.OrdenTrabajoId)
                .ForeignKey("dbo.Vehiculos", t => t.VehiculoId)
                .Index(t => t.VehiculoId)
                .Index(t => t.OrdenTrabajoId);
            
            CreateTable(
                "dbo.OrdenesTrabajo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehiculoId = c.Int(nullable: false),
                        DescripcionTrabajo = c.String(nullable: false, maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 30),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaFinalizacion = c.DateTime(),
                        Observaciones = c.String(maxLength: 500),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehiculos", t => t.VehiculoId)
                .Index(t => t.VehiculoId);
            
            CreateTable(
                "dbo.Repuestos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 300),
                        CantidadDisponible = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaRegistro = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistorialVehicular", "VehiculoId", "dbo.Vehiculos");
            DropForeignKey("dbo.HistorialVehicular", "OrdenTrabajoId", "dbo.OrdenesTrabajo");
            DropForeignKey("dbo.OrdenesTrabajo", "VehiculoId", "dbo.Vehiculos");
            DropIndex("dbo.OrdenesTrabajo", new[] { "VehiculoId" });
            DropIndex("dbo.HistorialVehicular", new[] { "OrdenTrabajoId" });
            DropIndex("dbo.HistorialVehicular", new[] { "VehiculoId" });
            DropTable("dbo.Repuestos");
            DropTable("dbo.OrdenesTrabajo");
            DropTable("dbo.HistorialVehicular");
        }
    }
}
