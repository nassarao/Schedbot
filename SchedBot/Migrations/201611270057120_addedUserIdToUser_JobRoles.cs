namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserIdToUser_JobRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User_JobRole", "User_UserId", "dbo.Users");
            DropIndex("dbo.User_JobRole", new[] { "User_UserId" });
            RenameColumn(table: "dbo.User_JobRole", name: "User_UserId", newName: "UserId");
            AlterColumn("dbo.User_JobRole", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.User_JobRole", "UserId");
            AddForeignKey("dbo.User_JobRole", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_JobRole", "UserId", "dbo.Users");
            DropIndex("dbo.User_JobRole", new[] { "UserId" });
            AlterColumn("dbo.User_JobRole", "UserId", c => c.Int());
            RenameColumn(table: "dbo.User_JobRole", name: "UserId", newName: "User_UserId");
            CreateIndex("dbo.User_JobRole", "User_UserId");
            AddForeignKey("dbo.User_JobRole", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
