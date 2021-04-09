namespace Lake_of_the_Humber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateonstaffInfomodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffInfoes", "DepartmentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffInfoes", "DepartmentName");
        }
    }
}
