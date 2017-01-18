namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hopethisFixesSomething : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities");
            DropIndex("dbo.Users", new[] { "AvailabilityId" });
            AddColumn("dbo.Availabilities", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Availabilities", "UserId");
            AddForeignKey("dbo.Availabilities", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            DropColumn("dbo.Users", "AvailabilityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AvailabilityId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Availabilities", "UserId", "dbo.Users");
            DropIndex("dbo.Availabilities", new[] { "UserId" });
            DropColumn("dbo.Availabilities", "UserId");
            CreateIndex("dbo.Users", "AvailabilityId");
            AddForeignKey("dbo.Users", "AvailabilityId", "dbo.Availabilities", "AvailabilityId", cascadeDelete: true);
        }
    }
}
