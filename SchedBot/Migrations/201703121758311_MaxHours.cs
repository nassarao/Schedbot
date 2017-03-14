namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Availabilities", "MaxHours", c => c.Int(nullable: false));
            DropColumn("dbo.Shifts", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Availabilities", "MaxHours");
        }
    }
}
