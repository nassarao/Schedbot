namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Availabilities", "UserId", "dbo.Users");
            DropIndex("dbo.Availabilities", new[] { "UserId" });
            DropColumn("dbo.Availabilities", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Availabilities", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Availabilities", "UserId");
            AddForeignKey("dbo.Availabilities", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
