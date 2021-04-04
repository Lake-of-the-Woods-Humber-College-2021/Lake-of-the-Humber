namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class staffinfoanddepartmentmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        DepartmentAddress = c.String(),
                        DepartmentPhone = c.String(),
                        DepartmentCreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.AspNetUsers", t => t.DepartmentCreatorId)
                .Index(t => t.DepartmentCreatorId);
            
            CreateTable(
                "dbo.StaffInfoes",
                c => new
                    {
                        SatffId = c.Int(nullable: false, identity: true),
                        StaffFirstName = c.String(),
                        StaffLastName = c.String(),
                        StaffLanguage = c.String(),
                        StaffHasPic = c.Boolean(nullable: false),
                        StaffImagePath = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        StaffCreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SatffId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StaffCreatorId)
                .Index(t => t.DepartmentId)
                .Index(t => t.StaffCreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "DepartmentCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffInfoes", "StaffCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffInfoes", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.StaffInfoes", new[] { "StaffCreatorId" });
            DropIndex("dbo.StaffInfoes", new[] { "DepartmentId" });
            DropIndex("dbo.Departments", new[] { "DepartmentCreatorId" });
            DropTable("dbo.StaffInfoes");
            DropTable("dbo.Departments");
        }
    }
}
