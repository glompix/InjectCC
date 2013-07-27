using System;
using System.Data.Entity.Migrations;

namespace InjectCC.Model.EntityFramework.Migrations
{    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Ordinal = c.Int(nullable: false),
                        MinutesUntilNextInjection = c.Int(nullable: false),
                        ReferenceImageUrl = c.String(nullable: false, maxLength: 250),
                        InjectionPointX = c.Double(nullable: false),
                        InjectionPointY = c.Double(nullable: false),
                        MedicationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Medications", t => t.MedicationId, cascadeDelete: true)
                .Index(t => t.MedicationId);
            
            CreateTable(
                "dbo.Medications",
                c => new
                    {
                        MedicationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 1000),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.LocationModifiers",
                c => new
                    {
                        LocationModifierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Ordinal = c.Int(nullable: false),
                        MedicationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationModifierId)
                .ForeignKey("dbo.Medications", t => t.MedicationId, cascadeDelete: true)
                .Index(t => t.MedicationId);
            
            CreateTable(
                "dbo.Injections",
                c => new
                    {
                        InjectionId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        LocationId = c.Int(nullable: false),
                        MedicationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InjectionId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Medications", t => t.MedicationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.LocationId)
                .Index(t => t.MedicationId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Injections", new[] { "UserId" });
            DropIndex("dbo.Injections", new[] { "MedicationId" });
            DropIndex("dbo.Injections", new[] { "LocationId" });
            DropIndex("dbo.LocationModifiers", new[] { "MedicationId" });
            DropIndex("dbo.Medications", new[] { "UserId" });
            DropIndex("dbo.Locations", new[] { "MedicationId" });
            DropForeignKey("dbo.Injections", "UserId", "dbo.Users");
            DropForeignKey("dbo.Injections", "MedicationId", "dbo.Medications");
            DropForeignKey("dbo.Injections", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LocationModifiers", "MedicationId", "dbo.Medications");
            DropForeignKey("dbo.Medications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "MedicationId", "dbo.Medications");
            DropTable("dbo.Injections");
            DropTable("dbo.LocationModifiers");
            DropTable("dbo.Users");
            DropTable("dbo.Medications");
            DropTable("dbo.Locations");
        }
    }
}
