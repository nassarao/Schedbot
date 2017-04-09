namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyToRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "OriginalShift_ShiftID", c => c.Int());
            AddColumn("dbo.Requests", "TradingShift_ShiftID", c => c.Int());
            CreateIndex("dbo.Requests", "OriginalShift_ShiftID");
            CreateIndex("dbo.Requests", "TradingShift_ShiftID");
            AddForeignKey("dbo.Requests", "OriginalShift_ShiftID", "dbo.Shifts", "ShiftID");
            AddForeignKey("dbo.Requests", "TradingShift_ShiftID", "dbo.Shifts", "ShiftID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "TradingShift_ShiftID", "dbo.Shifts");
            DropForeignKey("dbo.Requests", "OriginalShift_ShiftID", "dbo.Shifts");
            DropIndex("dbo.Requests", new[] { "TradingShift_ShiftID" });
            DropIndex("dbo.Requests", new[] { "OriginalShift_ShiftID" });
            DropColumn("dbo.Requests", "TradingShift_ShiftID");
            DropColumn("dbo.Requests", "OriginalShift_ShiftID");
        }
    }
}
