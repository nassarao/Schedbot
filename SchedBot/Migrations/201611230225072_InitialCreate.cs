namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        AvailabilityId = c.Int(nullable: false, identity: true),
                        Sunday = c.Int(nullable: false),
                        Monday = c.Int(nullable: false),
                        Tuesday = c.Int(nullable: false),
                        Wednesday = c.Int(nullable: false),
                        Thursday = c.Int(nullable: false),
                        Friday = c.Int(nullable: false),
                        Saturday = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AvailabilityId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        Status = c.String(),
                        StatusExplanation = c.String(),
                        RequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .Index(t => t.RequestTypeId);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        RequestTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequestTypeId);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ShiftID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ShiftTime = c.Time(nullable: false, precision: 7),
                        ShiftTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShiftID)
                .ForeignKey("dbo.ShiftTypes", t => t.ShiftTypeId, cascadeDelete: true)
                .Index(t => t.ShiftTypeId);
            
            CreateTable(
                "dbo.ShiftTypes",
                c => new
                    {
                        ShiftTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ShiftTypeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "ShiftTypeId", "dbo.ShiftTypes");
            DropForeignKey("dbo.Requests", "RequestTypeId", "dbo.RequestTypes");
            DropIndex("dbo.Shifts", new[] { "ShiftTypeId" });
            DropIndex("dbo.Requests", new[] { "RequestTypeId" });
            DropTable("dbo.Users");
            DropTable("dbo.ShiftTypes");
            DropTable("dbo.Shifts");
            DropTable("dbo.RequestTypes");
            DropTable("dbo.Requests");
            DropTable("dbo.Availabilities");
        }
    }
}
