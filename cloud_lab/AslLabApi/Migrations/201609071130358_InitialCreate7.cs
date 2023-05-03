namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GL_ACCHARMST",
                c => new
                    {
                        ACCHARMSTId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        HEADTP = c.Int(nullable: false),
                        HEADCD = c.Long(),
                        HEADNM = c.String(nullable: false),
                        REMARKS = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.ACCHARMSTId);
            
            CreateTable(
                "dbo.GL_ACCHART",
                c => new
                    {
                        ACCHARTId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        HEADTP = c.Int(nullable: false),
                        HEADCD = c.Long(),
                        ACCOUNTCD = c.Long(),
                        ACCOUNTNM = c.String(),
                        REMARKS = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.ACCHARTId);
            
            CreateTable(
                "dbo.GL_COSTPMST",
                c => new
                    {
                        CostMstID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        COSTCID = c.Long(),
                        COSTCNM = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.CostMstID);
            
            CreateTable(
                "dbo.GL_COSTPOOL",
                c => new
                    {
                        CostPLID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        COSTCID = c.Long(),
                        COSTPID = c.Long(),
                        COSTPNM = c.String(nullable: false),
                        COSTPSID = c.String(),
                        REMARKS = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.CostPLID);
            
            CreateTable(
                "dbo.GL_MASTER",
                c => new
                    {
                        GL_MasterID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        TRANSTP = c.String(),
                        TRANSDT = c.DateTime(nullable: false),
                        TRANSMY = c.String(),
                        TRANSNO = c.Long(),
                        TRANSSL = c.Long(),
                        TRANSDRCR = c.String(),
                        TRANSFOR = c.String(),
                        TRANSMODE = c.String(),
                        COSTPID = c.Long(),
                        CREDITCD = c.Long(),
                        DEBITCD = c.Long(),
                        CHEQUENO = c.String(),
                        CHEQUEDT = c.DateTime(),
                        REMARKS = c.String(),
                        DEBITAMT = c.Decimal(precision: 18, scale: 2),
                        CREDITAMT = c.Decimal(precision: 18, scale: 2),
                        TABLEID = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.GL_MasterID);
            
            CreateTable(
                "dbo.GL_STRANS",
                c => new
                    {
                        Gl_StransID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        TRANSTP = c.String(),
                        TRANSDT = c.DateTime(nullable: false),
                        TRANSMY = c.String(),
                        TRANSNO = c.Long(),
                        TRANSFOR = c.String(nullable: false),
                        TRANSMODE = c.String(),
                        COSTPID = c.Long(),
                        CREDITCD = c.Long(),
                        DEBITCD = c.Long(),
                        CHEQUENO = c.String(),
                        CHEQUEDT = c.DateTime(),
                        REMARKS = c.String(),
                        AMOUNT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.Gl_StransID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GL_STRANS");
            DropTable("dbo.GL_MASTER");
            DropTable("dbo.GL_COSTPOOL");
            DropTable("dbo.GL_COSTPMST");
            DropTable("dbo.GL_ACCHART");
            DropTable("dbo.GL_ACCHARMST");
        }
    }
}
