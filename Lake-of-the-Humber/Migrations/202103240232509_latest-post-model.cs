namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latestpostmodel : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LatestPosts", "PostCreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.LatestPosts", new[] { "PostCreatorId" });
            DropTable("dbo.LatestPosts");
        }
    }
}
