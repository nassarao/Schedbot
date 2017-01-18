namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserneedAvilabilityFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Availabilities", "UserId", "dbo.Users");
            DropIndex("dbo.Availabilities", new[] { "UserId" });
            AddColumn("dbo.Users", "AvailabilityId", c => c.Int(nullable: true));
            CreateIndex("dbo.Users", "AvailabilityId");
            AddForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities", "AvailabilityId", cascadeDelete: true);
            DropColumn("dbo.Availabilities", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Availabilities", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities");
            DropIndex("dbo.Users", new[] { "AvailabilityId" });
            DropColumn("dbo.Users", "AvailabilityId");
            CreateIndex("dbo.Availabilities", "UserId");
            AddForeignKey("dbo.Availabilities", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
