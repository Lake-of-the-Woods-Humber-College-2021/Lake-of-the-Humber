namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wellwishes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WellWishes",
                c => new
                    {
                        WishId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        RoomNumber = c.String(),
                        ReceiverName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsReceived = c.Boolean(nullable: false),
                        ReceivedDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WishId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            AddColumn("dbo.InfoSections", "IsArchive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WellWishes", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.WellWishes", new[] { "CreatorId" });
            DropColumn("dbo.InfoSections", "IsArchive");
            DropTable("dbo.WellWishes");
        }
    }
}
