namespace VechicleRegistration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QRCodes",
                c => new
                    {
                        StudentID = c.Int(nullable: false),
                        QRImage = c.Binary(),
                        date_created = c.DateTime(nullable: false),
                        last_modified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StudentID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        MatricNumber = c.Int(nullable: false),
                        StudentImage = c.Binary(),
                        ImageText = c.String(),
                        QrText = c.String(),
                        Department = c.String(),
                        Faculty = c.String(),
                        Email = c.String(),
                        date_created = c.DateTime(nullable: false),
                        last_modified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StudentID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        Plate_number = c.String(),
                        Chasis_number = c.String(),
                        Model = c.String(),
                        Year = c.String(),
                        Drivers_License = c.Binary(),
                        vehicle_front = c.Binary(),
                        vehicle_back = c.Binary(),
                        vehicle_side = c.Binary(),
                        Make = c.String(),
                        Color = c.String(),
                        proof_of_ownership = c.Binary(),
                        date_created = c.DateTime(nullable: false),
                        last_modified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleID)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.Int(nullable: false, identity: true),
                        VerificationCode = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TokenID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QRCodes", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Vehicles", "StudentID", "dbo.Students");
            DropIndex("dbo.Vehicles", new[] { "StudentID" });
            DropIndex("dbo.QRCodes", new[] { "StudentID" });
            DropTable("dbo.Tokens");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Students");
            DropTable("dbo.QRCodes");
        }
    }
}
