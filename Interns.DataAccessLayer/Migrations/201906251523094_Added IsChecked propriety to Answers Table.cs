namespace Interns.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsCheckedproprietytoAnswersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "IsChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "IsChecked");
        }
    }
}
