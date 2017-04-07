namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDatesRequests : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "StartTimeOff", c => c.DateTime());
            AlterColumn("dbo.Requests", "EndTimeOff", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "EndTimeOff", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requests", "StartTimeOff", c => c.DateTime(nullable: false));
        }
    }
}
