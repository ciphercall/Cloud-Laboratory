namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HMS_TESTMST", "TOPYN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HMS_TESTMST", "TOPYN");
        }
    }
}
