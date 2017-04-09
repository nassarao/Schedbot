namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedVirtual2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "OriginalShift_ShiftID", "dbo.Shifts");
            DropIndex("dbo.Requests", new[] { "OriginalShift_ShiftID" });
           // DropColumn("dbo.Requests", "OriginalShift_ShiftID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "OriginalShift_ShiftID", c => c.Int());
            CreateIndex("dbo.Requests", "OriginalShift_ShiftID");
            AddForeignKey("dbo.Requests", "OriginalShift_ShiftID", "dbo.Shifts", "ShiftID");
        }
    }
}
