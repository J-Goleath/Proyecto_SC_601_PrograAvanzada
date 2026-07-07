namespace AutoFix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientesYVehiculos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Citas", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.Citas", "VehiculoId", "dbo.Vehiculos");
            DropIndex("dbo.Citas", new[] { "ClienteId" });
            DropIndex("dbo.Citas", new[] { "VehiculoId" });
            AddColumn("dbo.Clientes", "Rol", c => c.Int(nullable: false));
            DropTable("dbo.Citas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Citas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Hora = c.Time(nullable: false, precision: 7),
                        DescripcionProblema = c.String(maxLength: 500),
                        Estado = c.Int(nullable: false),
                        FechaSolicitud = c.DateTime(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        VehiculoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Clientes", "Rol");
            CreateIndex("dbo.Citas", "VehiculoId");
            CreateIndex("dbo.Citas", "ClienteId");
            AddForeignKey("dbo.Citas", "VehiculoId", "dbo.Vehiculos", "Id");
            AddForeignKey("dbo.Citas", "ClienteId", "dbo.Clientes", "Id");
        }
    }
}
