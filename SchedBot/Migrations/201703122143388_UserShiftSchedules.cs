namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserShiftSchedules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User_Shift_Schedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShiftId = c.Int(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .ForeignKey("dbo.Shifts", t => t.ShiftId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ShiftId)
                .Index(t => t.ScheduleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Shift_Schedule", "UserId", "dbo.Users");
            DropForeignKey("dbo.User_Shift_Schedule", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.User_Shift_Schedule", "ScheduleId", "dbo.Schedules");
            DropIndex("dbo.User_Shift_Schedule", new[] { "UserId" });
            DropIndex("dbo.User_Shift_Schedule", new[] { "ScheduleId" });
            DropIndex("dbo.User_Shift_Schedule", new[] { "ShiftId" });
            DropTable("dbo.User_Shift_Schedule");
        }
    }
}
