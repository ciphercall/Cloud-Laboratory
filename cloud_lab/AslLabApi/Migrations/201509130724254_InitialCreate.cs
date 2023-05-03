namespace AslLabApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AslCompanies",
                c => new
                    {
                        AslCompanyId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        COMPNM = c.String(nullable: false),
                        ADDRESS = c.String(nullable: false),
                        CONTACTNO = c.String(nullable: false),
                        EMAILID = c.String(nullable: false),
                        WEBID = c.String(),
                        STATUS = c.String(nullable: false),
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
                .PrimaryKey(t => t.AslCompanyId);
            
            CreateTable(
                "dbo.ASL_DELETE",
                c => new
                    {
                        Asl_DeleteID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        DELSLNO = c.Long(),
                        DELDATE = c.String(),
                        DELTIME = c.String(),
                        DELIPNO = c.String(),
                        DELLTUDE = c.String(),
                        TABLEID = c.String(),
                        DELDATA = c.String(),
                        USERPC = c.String(),
                    })
                .PrimaryKey(t => t.Asl_DeleteID);
            
            CreateTable(
                "dbo.ASL_LOG",
                c => new
                    {
                        Asl_LOGid = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        LOGTYPE = c.String(),
                        LOGSLNO = c.Long(),
                        LOGDATE = c.DateTime(),
                        LOGTIME = c.String(),
                        LOGIPNO = c.String(),
                        LOGLTUDE = c.String(),
                        TABLEID = c.String(),
                        LOGDATA = c.String(),
                        USERPC = c.String(),
                    })
                .PrimaryKey(t => t.Asl_LOGid);
            
            CreateTable(
                "dbo.ASL_MENU",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MODULEID = c.String(),
                        SERIAL = c.Long(),
                        MENUTP = c.String(),
                        MENUID = c.String(),
                        MENUNM = c.String(),
                        ACTIONNAME = c.String(),
                        CONTROLLERNAME = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ASL_MENUMST",
                c => new
                    {
                        MODULEID = c.String(nullable: false, maxLength: 128),
                        MODULENM = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.MODULEID);
            
            CreateTable(
                "dbo.ASL_ROLE",
                c => new
                    {
                        ASL_ROLEId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        USERID = c.Long(nullable: false),
                        MODULEID = c.String(nullable: false),
                        SERIAL = c.Long(),
                        MENUTP = c.String(nullable: false),
                        MENUID = c.String(),
                        STATUS = c.String(),
                        INSERTR = c.String(),
                        UPDATER = c.String(),
                        DELETER = c.String(),
                        MENUNAME = c.String(),
                        ACTIONNAME = c.String(),
                        CONTROLLERNAME = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.ASL_ROLEId);
            
            CreateTable(
                "dbo.AslUsercoes",
                c => new
                    {
                        AslUsercoId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        USERNM = c.String(nullable: false),
                        DEPTNM = c.String(),
                        OPTP = c.String(nullable: false),
                        ADDRESS = c.String(nullable: false),
                        MOBNO = c.String(nullable: false),
                        EMAILID = c.String(),
                        LOGINBY = c.String(nullable: false),
                        LOGINID = c.String(nullable: false),
                        LOGINPW = c.String(nullable: false),
                        TIMEFR = c.String(nullable: false),
                        TIMETO = c.String(nullable: false),
                        STATUS = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.AslUsercoId);
            
            CreateTable(
                "dbo.HMS_DEPT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        DEPTID = c.Long(nullable: false),
                        DEPTNM = c.String(),
                        REMARKS = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.DEPTID });
            
            CreateTable(
                "dbo.HMS_MGT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        MGTID = c.Long(nullable: false),
                        MGTNM = c.String(),
                        DESIG = c.String(),
                        ADDRESS = c.String(),
                        MOBNO1 = c.String(),
                        MOBNO2 = c.String(),
                        EMAILID = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.MGTID });
            
            CreateTable(
                "dbo.HMS_MGTRF",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        MGTID = c.Long(nullable: false),
                        REFERID = c.Long(),
                        RFPCNT = c.Decimal(precision: 18, scale: 2),
                        REMARKS = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.MGTID });
            
            CreateTable(
                "dbo.HMS_OPD",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        PATIENTID = c.String(),
                        REFERID = c.Long(),
                        TESTSL = c.Long(),
                        TCATID = c.Long(),
                        TESTID = c.Long(),
                        AMOUNT = c.Decimal(precision: 18, scale: 2),
                        PCNTR = c.Decimal(precision: 18, scale: 2),
                        PCNTD = c.Decimal(precision: 18, scale: 2),
                        DISCR = c.Decimal(precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.HMS_OPDMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        PATIENTID = c.String(),
                        PATIENTNM = c.String(),
                        GENDER = c.String(),
                        AGE = c.String(),
                        MOBNO = c.String(),
                        ADDRESS = c.String(),
                        DOCTORID = c.Long(),
                        REFERID = c.Long(),
                        RFPCNT = c.Decimal(precision: 18, scale: 2),
                        DVDT = c.DateTime(),
                        DVTM = c.String(),
                        TOTAMT = c.Decimal(precision: 18, scale: 2),
                        DISCREF = c.Decimal(precision: 18, scale: 2),
                        DISCHOS = c.Decimal(precision: 18, scale: 2),
                        DISCNET = c.Decimal(precision: 18, scale: 2),
                        NETAMT = c.Decimal(precision: 18, scale: 2),
                        RCVAMT = c.Decimal(precision: 18, scale: 2),
                        DUEAMT = c.Decimal(precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.HMS_OPDRCV",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        TRANSMY = c.String(),
                        TRANSNO = c.Long(),
                        PATIENTID = c.String(),
                        DUEAMTP = c.Decimal(precision: 18, scale: 2),
                        DISCHOS = c.Decimal(precision: 18, scale: 2),
                        NETAMT = c.Decimal(precision: 18, scale: 2),
                        RCVAMT = c.Decimal(precision: 18, scale: 2),
                        DUEAMT = c.Decimal(precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.HMS_REFER",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        REFERID = c.Long(nullable: false),
                        REFERNM = c.String(nullable: false, maxLength: 128),
                        REFERGID = c.Long(),
                        TITLE = c.String(),
                        ADDRESS = c.String(),
                        MOBNO1 = c.String(),
                        MOBNO2 = c.String(),
                        EMAILID = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.REFERID, t.REFERNM });
            
            CreateTable(
                "dbo.HMS_RESULT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        PATIENTID = c.String(),
                        TESTID = c.Long(),
                        RESTID = c.Long(),
                        RESULT = c.String(),
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
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.HMS_RFPCNT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        REFERID = c.Long(nullable: false),
                        TCATID = c.Long(),
                        RFPCNT = c.Decimal(precision: 18, scale: 2),
                        REMARKS = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.REFERID });
            
            CreateTable(
                "dbo.HMS_SIGNBY",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        SIGNID = c.Long(nullable: false),
                        AUTHNM = c.String(),
                        TITLE = c.String(),
                        DEPTNM = c.String(),
                        INSTITUTE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.SIGNID });
            
            CreateTable(
                "dbo.HMS_TEST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TCATID = c.Long(nullable: false),
                        TESTID = c.Long(nullable: false),
                        DEPTID = c.Long(),
                        TESTNM = c.String(),
                        RATE = c.Decimal(precision: 18, scale: 2),
                        PCNTD = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TCATID, t.TESTID });
            
            CreateTable(
                "dbo.HMS_TESTMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TCATID = c.Long(nullable: false),
                        TCATNM = c.String(),
                        DEPTID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TCATID });
            
            CreateTable(
                "dbo.HMS_TESTNV",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TESTID = c.Long(nullable: false),
                        RESTID = c.Long(),
                        RESTNM = c.String(),
                        RESTGR = c.String(),
                        RESTMU = c.String(),
                        SHOWTP = c.String(),
                        LENGTH = c.Long(),
                        DECIML = c.Long(),
                        NVALUE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TESTID });
            
            CreateTable(
                "dbo.HMS_TESTV",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TCATID = c.Long(nullable: false),
                        TESTID = c.Long(nullable: false),
                        TVACID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TCATID, t.TESTID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HMS_TESTV");
            DropTable("dbo.HMS_TESTNV");
            DropTable("dbo.HMS_TESTMST");
            DropTable("dbo.HMS_TEST");
            DropTable("dbo.HMS_SIGNBY");
            DropTable("dbo.HMS_RFPCNT");
            DropTable("dbo.HMS_RESULT");
            DropTable("dbo.HMS_REFER");
            DropTable("dbo.HMS_OPDRCV");
            DropTable("dbo.HMS_OPDMST");
            DropTable("dbo.HMS_OPD");
            DropTable("dbo.HMS_MGTRF");
            DropTable("dbo.HMS_MGT");
            DropTable("dbo.HMS_DEPT");
            DropTable("dbo.AslUsercoes");
            DropTable("dbo.ASL_ROLE");
            DropTable("dbo.ASL_MENUMST");
            DropTable("dbo.ASL_MENU");
            DropTable("dbo.ASL_LOG");
            DropTable("dbo.ASL_DELETE");
            DropTable("dbo.AslCompanies");
        }
    }
}
