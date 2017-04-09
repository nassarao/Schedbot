namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedVirtual : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "ReceivingUser_UserId", "dbo.Users");
            DropForeignKey("dbo.Requests", "SendingUser_UserId", "dbo.Users");
            DropForeignKey("dbo.Requests", "TradingShift_ShiftID", "dbo.Shifts");
            DropIndex("dbo.Requests", new[] { "ReceivingUser_UserId" });
            DropIndex("dbo.Requests", new[] { "SendingUser_UserId" });
            DropIndex("dbo.Requests", new[] { "TradingShift_ShiftID" });
         //   DropColumn("dbo.Requests", "ReceivingUser_UserId");
          //  DropColumn("dbo.Requests", "SendingUser_UserId");
         //   DropColumn("dbo.Requests", "TradingShift_ShiftID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "TradingShift_ShiftID", c => c.Int());
            AddColumn("dbo.Requests", "SendingUser_UserId", c => c.Int());
            AddColumn("dbo.Requests", "ReceivingUser_UserId", c => c.Int());
            CreateIndex("dbo.Requests", "TradingShift_ShiftID");
            CreateIndex("dbo.Requests", "SendingUser_UserId");
            CreateIndex("dbo.Requests", "ReceivingUser_UserId");
            AddForeignKey("dbo.Requests", "TradingShift_ShiftID", "dbo.Shifts", "ShiftID");
            AddForeignKey("dbo.Requests", "SendingUser_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Requests", "ReceivingUser_UserId", "dbo.Users", "UserId");
        }
    }
}
