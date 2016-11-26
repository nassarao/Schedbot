namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobRoleandPhoneNumber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobRoles",
                c => new
                    {
                        JobRoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.JobRoleId);
            
            CreateTable(
                "dbo.User_JobRole",
                c => new
                    {
                        User_JobRoleId = c.Int(nullable: false, identity: true),
                        JobRoleId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.User_JobRoleId)
                .ForeignKey("dbo.JobRoles", t => t.JobRoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.JobRoleId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_JobRole", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.User_JobRole", "JobRoleId", "dbo.JobRoles");
            DropIndex("dbo.User_JobRole", new[] { "User_UserId" });
            DropIndex("dbo.User_JobRole", new[] { "JobRoleId" });
            DropColumn("dbo.Users", "PhoneNumber");
            DropTable("dbo.User_JobRole");
            DropTable("dbo.JobRoles");
        }
    }
}
