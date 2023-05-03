namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HMS_OPDMST", "TRANSMY", c => c.String());
            AddColumn("dbo.HMS_OPDMST", "TRANSNO", c => c.Long());
            AddColumn("dbo.HMS_OPDMST", "REMARKS", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HMS_OPDMST", "REMARKS");
            DropColumn("dbo.HMS_OPDMST", "TRANSNO");
            DropColumn("dbo.HMS_OPDMST", "TRANSMY");
        }
    }
}
