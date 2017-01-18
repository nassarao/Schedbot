namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvailabilityUserID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Availabilities", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Availabilities", "UserId");
            AddForeignKey("dbo.Availabilities", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availabilities", "UserId", "dbo.Users");
            DropIndex("dbo.Availabilities", new[] { "UserId" });
            DropColumn("dbo.Availabilities", "UserId");
        }
    }
}
