namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.HMS_REFER");
            AlterColumn("dbo.HMS_REFER", "REFERNM", c => c.String());
            AddPrimaryKey("dbo.HMS_REFER", new[] { "ID", "COMPID", "REFERID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.HMS_REFER");
            AlterColumn("dbo.HMS_REFER", "REFERNM", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.HMS_REFER", new[] { "ID", "COMPID", "REFERID", "REFERNM" });
        }
    }
}
