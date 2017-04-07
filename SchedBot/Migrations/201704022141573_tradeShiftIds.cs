namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeShiftIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "OriginalShiftId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "TradingShiftId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "TradingShiftId");
            DropColumn("dbo.Requests", "OriginalShiftId");
        }
    }
}
