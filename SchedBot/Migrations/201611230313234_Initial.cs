namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvailableShifts",
                c => new
                    {
                        AvailableShiftsId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AvailableShiftsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AvailableShifts");
        }
    }
}
