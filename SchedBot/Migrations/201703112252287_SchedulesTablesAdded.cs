namespace SchedBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchedulesTablesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shifts", "ShiftTypeId", "dbo.ShiftTypes");
            DropIndex("dbo.Shifts", new[] { "ShiftTypeId" });
            CreateTable(
                "dbo.Schedule_Shift",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleId = c.Int(nullable: false),
                        ShiftId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .ForeignKey("dbo.Shifts", t => t.ShiftId, cascadeDelete: true)
                .Index(t => t.ScheduleId)
                .Index(t => t.ShiftId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Flag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Shifts", "Day", c => c.Int(nullable: false));
            AddColumn("dbo.Shifts", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Shifts", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Shifts", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shifts", "MyProperty", c => c.Int(nullable: false));
            AddColumn("dbo.Shifts", "JobRoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Shifts", "JobRoleId");
            AddForeignKey("dbo.Shifts", "JobRoleId", "dbo.JobRoles", "JobRoleId", cascadeDelete: true);
            DropColumn("dbo.Shifts", "Date");
            DropColumn("dbo.Shifts", "ShiftTime");
            DropColumn("dbo.Shifts", "ShiftTypeId");
            DropTable("dbo.ShiftTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShiftTypes",
                c => new
                    {
                        ShiftTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ShiftTypeId);
            
            AddColumn("dbo.Shifts", "ShiftTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Shifts", "ShiftTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Shifts", "Date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Schedule_Shift", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.Shifts", "JobRoleId", "dbo.JobRoles");
            DropForeignKey("dbo.Schedule_Shift", "ScheduleId", "dbo.Schedules");
            DropIndex("dbo.Shifts", new[] { "JobRoleId" });
            DropIndex("dbo.Schedule_Shift", new[] { "ShiftId" });
            DropIndex("dbo.Schedule_Shift", new[] { "ScheduleId" });
            DropColumn("dbo.Shifts", "JobRoleId");
            DropColumn("dbo.Shifts", "MyProperty");
            DropColumn("dbo.Shifts", "Active");
            DropColumn("dbo.Shifts", "End");
            DropColumn("dbo.Shifts", "Start");
            DropColumn("dbo.Shifts", "Day");
            DropTable("dbo.Schedules");
            DropTable("dbo.Schedule_Shift");
            CreateIndex("dbo.Shifts", "ShiftTypeId");
            AddForeignKey("dbo.Shifts", "ShiftTypeId", "dbo.ShiftTypes", "ShiftTypeId", cascadeDelete: true);
        }
    }
}
