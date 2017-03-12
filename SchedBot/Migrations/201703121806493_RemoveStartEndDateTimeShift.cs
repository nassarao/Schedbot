namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStartEndDateTimeShift : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Shifts", "Start");
            DropColumn("dbo.Shifts", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Shifts", "Start", c => c.DateTime(nullable: false));
        }
    }
}
