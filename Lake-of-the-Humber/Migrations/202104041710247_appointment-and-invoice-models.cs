namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentandinvoicemodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppId = c.Int(nullable: false, identity: true),
                        AppMethod = c.String(),
                        AppPurpose = c.String(),
                        AppDate = c.DateTime(nullable: false),
                        AppTime = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AppId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceTitle = c.String(),
                        InvoiceDesc = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        InvoiceCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invoices", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Invoices", new[] { "UserId" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropTable("dbo.Invoices");
            DropTable("dbo.Appointments");
        }
    }
}
