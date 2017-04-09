namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "OriginalUSSId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "TradingUSSId", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "OriginalShiftId");
            DropColumn("dbo.Requests", "TradingShiftId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "TradingShiftId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "OriginalShiftId", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "TradingUSSId");
            DropColumn("dbo.Requests", "OriginalUSSId");
        }
    }
}
