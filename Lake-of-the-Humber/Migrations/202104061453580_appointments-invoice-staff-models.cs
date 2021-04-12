namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentsinvoicestaffmodels : DbMigration
    {
        public override void Up()
        {
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
                "dbo.LatestPosts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        PostTitle = c.String(),
                        PostSummary = c.String(),
                        PostCategory = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        PostImagePath = c.String(),
                        PostContent = c.String(),
                        Postlink = c.String(),
                        PostCreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.PostCreatorId)
                .Index(t => t.PostCreatorId);
            
            AddColumn("dbo.Appointments", "StaffId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "StaffId");
            AddForeignKey("dbo.Appointments", "StaffId", "dbo.StaffInfoes", "SatffId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LatestPosts", "PostCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "StaffId", "dbo.StaffInfoes");
            DropForeignKey("dbo.StaffInfoes", "StaffCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffInfoes", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "DepartmentCreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.LatestPosts", new[] { "PostCreatorId" });
            DropIndex("dbo.Departments", new[] { "DepartmentCreatorId" });
            DropIndex("dbo.StaffInfoes", new[] { "StaffCreatorId" });
            DropIndex("dbo.StaffInfoes", new[] { "DepartmentId" });
            DropIndex("dbo.Appointments", new[] { "StaffId" });
            DropColumn("dbo.Appointments", "StaffId");
            DropTable("dbo.LatestPosts");
            DropTable("dbo.Departments");
            DropTable("dbo.StaffInfoes");
        }
    }
}
