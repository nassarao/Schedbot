namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "Type");
        }
    }
}
