using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using AslLabApi.Models.HMS;
using AslLabApi.Models.GL;
using AslLabApi.Models.ASL;



namespace AslLabApi.Models
{
    public class AslLabApiContext : DbContext
    {
        public AslLabApiContext()
            : base("name=AslLabApiContext")
        {
            //base.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Department> HMS_DEPT { get; set; }
        public DbSet<TestMaster> HMS_TESTMST { get; set; }
        public DbSet<Test> HMS_TEST { get; set; }
        public DbSet<SignBy> HMS_SIGNBY { get; set; }
        public DbSet<ManagementRF> HMS_MGTRF { get; set; }
        public DbSet<Management> HMS_MGT { get; set; }
        public DbSet<RefPersonContact> HMS_RFPCNT { get; set; }
        public DbSet<Refer> HMS_REFER { get; set; }
        public DbSet<TestNV> HMS_TESTNV { get; set; }
        public DbSet<TestV> HMS_TESTV { get; set; }
        public DbSet<Opd> HMS_OPD { get; set; }
        public DbSet<OpdMaster> HMS_OPDMST { get; set; }
        public DbSet<OpdReceive> HMS_OPDRCV { get; set; }
        public DbSet<Result> HMS_RESULT { get; set; }


        public DbSet<AslCompany> AslCompanyDbSet { get; set; }
        public DbSet<AslUserco> AslUsercoDbSet { get; set; }
        public DbSet<ASL_LOG> AslLogDbSet { get; set; }
        public DbSet<ASL_DELETE> AslDeleteDbSet { get; set; }
        public DbSet<ASL_MENUMST> AslMenumstDbSet { get; set; }
        public DbSet<ASL_MENU> AslMenuDbSet { get; set; }
        public DbSet<ASL_ROLE> AslRoleDbSet { get; set; }



        //GL Db Set


        public DbSet<GL_COSTPMST> GLCostPMSTDbSet { get; set; }
        public DbSet<GL_COSTPOOL> GlCostPoolDbSet { get; set; }
        public DbSet<GL_STRANS> GlStransDbSet { get; set; }
        public DbSet<GL_MASTER> GlMasterDbSet { get; set; }
        public DbSet<GL_ACCHARMST> GlAccharmstDbSet { get; set; }
        public DbSet<GL_ACCHART> GlAcchartDbSet { get; set; }


        //Upload Excel File Data module
        public DbSet<ASL_PCONTACTS> UploadContactDbSet { get; set; }
        public DbSet<ASL_PGROUPS> UploadGroupDbSet { get; set; }
        public DbSet<ASL_PEMAIL> SendLogEmailDbSet { get; set; }
        public DbSet<ASL_PSMS> SendLogSMSDbSet { get; set; }


        //Promotional
        public DbSet<ASL_PCalendarImage> CalendarImageDbSet { get; set; }
        public DbSet<ASL_SchedularCalendar> SchedularCalendarDbSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }
}