namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Faq3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqId = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        Publish = c.Boolean(nullable: false),
                        FaqDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FaqId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Faqs", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Faqs", new[] { "CreatorId" });
            DropTable("dbo.Faqs");
        }
    }
}
