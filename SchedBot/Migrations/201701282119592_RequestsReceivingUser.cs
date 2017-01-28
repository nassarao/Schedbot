namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestsReceivingUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "RequestTypeId", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "UserId", "dbo.Users");
            DropIndex("dbo.Requests", new[] { "RequestTypeId" });
            DropIndex("dbo.Requests", new[] { "UserId" });
            RenameColumn(table: "dbo.Requests", name: "UserId", newName: "ReceivingUser_UserId");
            AddColumn("dbo.Requests", "RequestType", c => c.String());
            AddColumn("dbo.Requests", "SendingUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "ReceivingUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "SendingUser_UserId", c => c.Int());
            AlterColumn("dbo.Requests", "ReceivingUser_UserId", c => c.Int());
            CreateIndex("dbo.Requests", "ReceivingUser_UserId");
            CreateIndex("dbo.Requests", "SendingUser_UserId");
            AddForeignKey("dbo.Requests", "SendingUser_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Requests", "ReceivingUser_UserId", "dbo.Users", "UserId");
            DropColumn("dbo.Requests", "RequestTypeId");
            DropTable("dbo.RequestTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        RequestTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RequestTypeId);
            
            AddColumn("dbo.Requests", "RequestTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Requests", "ReceivingUser_UserId", "dbo.Users");
            DropForeignKey("dbo.Requests", "SendingUser_UserId", "dbo.Users");
            DropIndex("dbo.Requests", new[] { "SendingUser_UserId" });
            DropIndex("dbo.Requests", new[] { "ReceivingUser_UserId" });
            AlterColumn("dbo.Requests", "ReceivingUser_UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "SendingUser_UserId");
            DropColumn("dbo.Requests", "ReceivingUserId");
            DropColumn("dbo.Requests", "SendingUserId");
            DropColumn("dbo.Requests", "RequestType");
            RenameColumn(table: "dbo.Requests", name: "ReceivingUser_UserId", newName: "UserId");
            CreateIndex("dbo.Requests", "UserId");
            CreateIndex("dbo.Requests", "RequestTypeId");
            AddForeignKey("dbo.Requests", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "RequestTypeId", "dbo.RequestTypes", "RequestTypeId", cascadeDelete: true);
        }
    }
}
