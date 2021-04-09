namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoicemodelupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "InvoiceCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "InvoiceCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
