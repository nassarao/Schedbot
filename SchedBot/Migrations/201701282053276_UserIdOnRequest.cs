namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdOnRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "UserId");
            AddForeignKey("dbo.Requests", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "UserId", "dbo.Users");
            DropIndex("dbo.Requests", new[] { "UserId" });
            DropColumn("dbo.Requests", "UserId");
        }
    }
}
