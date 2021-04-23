namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropIndex("dbo.CartItems", new[] { "ProductId" });
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
            
            DropTable("dbo.CartItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CartId = c.String(),
                        Quantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
            DropForeignKey("dbo.ProductOrders", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.ProductOrders", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.ProductOrders", new[] { "Order_OrderID" });
            DropIndex("dbo.ProductOrders", new[] { "Product_ProductID" });
            DropTable("dbo.ProductOrders");
            DropTable("dbo.Orders");
            CreateIndex("dbo.CartItems", "ProductId");
            AddForeignKey("dbo.CartItems", "ProductId", "dbo.Products", "ProductID", cascadeDelete: true);
        }
    }
}
