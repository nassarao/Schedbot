namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountForeignKeyString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AccountId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AccountId", c => c.Int(nullable: false));
        }
    }
}
