namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ASL_PSMS", "MSGTP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ASL_PSMS", "MSGTP");
        }
    }
}
