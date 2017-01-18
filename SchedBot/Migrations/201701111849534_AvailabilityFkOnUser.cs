namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvailabilityFkOnUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AvailabilityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "AvailabilityId");
            AddForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities", "AvailabilityId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities");
            DropIndex("dbo.Users", new[] { "AvailabilityId" });
            DropColumn("dbo.Users", "AvailabilityId");
        }
    }
}
