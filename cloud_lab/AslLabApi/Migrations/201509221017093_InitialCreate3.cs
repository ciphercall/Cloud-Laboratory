namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HMS_RESULT", "RESTSL", c => c.Long());
            AddColumn("dbo.HMS_RESULT", "RESTGRV", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HMS_RESULT", "RESTGRV");
            DropColumn("dbo.HMS_RESULT", "RESTSL");
        }
    }
}
