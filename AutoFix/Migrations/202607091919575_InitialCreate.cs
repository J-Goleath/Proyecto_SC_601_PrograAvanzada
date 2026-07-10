namespace AutoFix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CitasSolicitud",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Hora = c.Time(nullable: false, precision: 7),
                        DescripcionFallos = c.String(nullable: false, maxLength: 500),
                        Procesada = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                        VehiculoId = c.Int(nullable: false),
                        MecanicoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.MecanicoId)
                .ForeignKey("dbo.Vehiculos", t => t.VehiculoId)
                .Index(t => t.VehiculoId)
                .Index(t => t.MecanicoId);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(nullable: false, maxLength: 20),
                        Contraseña = c.String(nullable: false, maxLength: 100),
                        Rol = c.Int(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehiculos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Placa = c.String(nullable: false, maxLength: 20),
                        Marca = c.String(nullable: false, maxLength: 50),
                        Modelo = c.String(nullable: false, maxLength: 50),
                        Anio = c.Int(nullable: false),
                        Color = c.String(maxLength: 20),
                        FechaRegistro = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                        ClienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.Notificaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(nullable: false, maxLength: 250),
                        FechaEnvio = c.DateTime(nullable: false),
                        Leida = c.Boolean(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                        ClienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notificaciones", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.CitasSolicitud", "VehiculoId", "dbo.Vehiculos");
            DropForeignKey("dbo.CitasSolicitud", "MecanicoId", "dbo.Clientes");
            DropForeignKey("dbo.Vehiculos", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Notificaciones", new[] { "ClienteId" });
            DropIndex("dbo.Vehiculos", new[] { "ClienteId" });
            DropIndex("dbo.CitasSolicitud", new[] { "MecanicoId" });
            DropIndex("dbo.CitasSolicitud", new[] { "VehiculoId" });
            DropTable("dbo.Notificaciones");
            DropTable("dbo.Vehiculos");
            DropTable("dbo.Clientes");
            DropTable("dbo.CitasSolicitud");
        }
    }
}
