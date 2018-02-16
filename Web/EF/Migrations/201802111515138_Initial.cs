namespace Web.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        SSN = c.String(nullable: false, maxLength: 11),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        StreetAddress = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StaffId);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Reason = c.String(),
                        Treatment = c.String(),
                        FollowUpDate = c.DateTime(),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.StaffAssignments",
                c => new
                    {
                        StaffId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StaffId, t.PatientId })
                .ForeignKey("dbo.Staff", t => t.StaffId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.StaffId)
                .Index(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visits", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.StaffAssignments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.StaffAssignments", "StaffId", "dbo.Staff");
            DropIndex("dbo.StaffAssignments", new[] { "PatientId" });
            DropIndex("dbo.StaffAssignments", new[] { "StaffId" });
            DropIndex("dbo.Visits", new[] { "PatientId" });
            DropTable("dbo.StaffAssignments");
            DropTable("dbo.Visits");
            DropTable("dbo.Staff");
            DropTable("dbo.Patients");
        }
    }
}
