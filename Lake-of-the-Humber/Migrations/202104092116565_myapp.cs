namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myapp : DbMigration
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
                        DepartmentName = c.String(),
                        StaffCreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SatffId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StaffCreatorId)
                .Index(t => t.DepartmentId)
                .Index(t => t.StaffCreatorId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderName = c.String(),
                        OrderMessage = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.ProductOrders",
                c => new
                    {
                        Product_ProductID = c.Int(nullable: false),
                        Order_OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductID, t.Order_OrderID })
                .ForeignKey("dbo.Products", t => t.Product_ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID, cascadeDelete: true)
                .Index(t => t.Product_ProductID)
                .Index(t => t.Order_OrderID);
            
            AddColumn("dbo.InfoSections", "LinkBtnName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.WellWishes", "ReceivedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOrders", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.ProductOrders", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.Departments", "DepartmentCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffInfoes", "StaffCreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffInfoes", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.ProductOrders", new[] { "Order_OrderID" });
            DropIndex("dbo.ProductOrders", new[] { "Product_ProductID" });
            DropIndex("dbo.StaffInfoes", new[] { "StaffCreatorId" });
            DropIndex("dbo.StaffInfoes", new[] { "DepartmentId" });
            DropIndex("dbo.Departments", new[] { "DepartmentCreatorId" });
            AlterColumn("dbo.WellWishes", "ReceivedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.InfoSections", "LinkBtnName");
            DropTable("dbo.ProductOrders");
            DropTable("dbo.Orders");
            DropTable("dbo.StaffInfoes");
            DropTable("dbo.Departments");
        }
    }
}
