namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeOnShiftTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Shifts", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shifts", "MyProperty", c => c.Int(nullable: false));
            DropColumn("dbo.Shifts", "Type");
        }
    }
}
