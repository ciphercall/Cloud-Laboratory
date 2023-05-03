namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HMS_TESTNV", "RESTSL", c => c.Long());
            AddColumn("dbo.HMS_TESTNV", "RESTGRV", c => c.String());
            DropColumn("dbo.HMS_RESULT", "RESTSL");
            DropColumn("dbo.HMS_RESULT", "RESTGRV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HMS_RESULT", "RESTGRV", c => c.String());
            AddColumn("dbo.HMS_RESULT", "RESTSL", c => c.Long());
            DropColumn("dbo.HMS_TESTNV", "RESTGRV");
            DropColumn("dbo.HMS_TESTNV", "RESTSL");
        }
    }
}
