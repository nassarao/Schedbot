namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeOffFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "StartTimeOff", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requests", "EndTimeOff", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "EndTimeOff");
            DropColumn("dbo.Requests", "StartTimeOff");
        }
    }
}
