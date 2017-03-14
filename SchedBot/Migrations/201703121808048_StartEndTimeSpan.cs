namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartEndTimeSpan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "Start", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Shifts", "End", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "End");
            DropColumn("dbo.Shifts", "Start");
        }
    }
}
