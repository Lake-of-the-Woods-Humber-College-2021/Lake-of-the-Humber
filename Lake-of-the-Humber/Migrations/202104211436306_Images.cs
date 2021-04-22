namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ProductPicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductPicExtension");
            DropColumn("dbo.Products", "ProductHasPic");
        }
    }
}
