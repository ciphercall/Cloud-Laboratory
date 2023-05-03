using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class AppController : Controller
    {
      
        // GET: /App/
        AslLabApiContext db = new AslLabApiContext();

        public AslLabApiContext DataContext
        {
            get { return db; }
        }

        public AppController()
        {
            //var forslide = 1;
        }
        public static class Global
        {
            public static Int64 GlobalVariable = 1;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
           
            try
            {
                var userid = Convert.ToInt64(Session["loggedUserID"]);
                var comid = Convert.ToInt64(Session["loggedCompID"]);

              

           
                ViewData["validUserForm"] = from c in db.AslRoleDbSet
                                       where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP=="F" && c.MODULEID=="01")
                                       select c;

                ViewData["validUserReports"] = from c in db.AslRoleDbSet
                                            where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID=="01")
                                            select c;

                ViewData["validBillingForm"] = (from c in db.AslRoleDbSet
                                            where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "02")
                                               select c).OrderBy(x => x.SERIAL);

                ViewData["validBillingReports"] = (from c in db.AslRoleDbSet
                                               where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID == "02")
                                                   select c).OrderBy(x => x.SERIAL);
                ViewData["valid_GL_Form"] = from c in db.AslRoleDbSet
                                               where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "03")
                                               select c;

                ViewData["valid_GL_Report"] = from c in db.AslRoleDbSet
                                                  where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "R" && c.MODULEID == "03")
                                                  select c;


                ViewData["validPromotionForm"] = (from c in db.AslRoleDbSet
                                                  where (c.USERID == userid && c.COMPID == comid && c.STATUS == "A" && c.MENUTP == "F" && c.MODULEID == "04")
                                                  select c).OrderBy(x => x.SERIAL);

                var findCompanyName = from m in db.AslCompanyDbSet where m.COMPID == comid select new { m.COMPNM };
                string Name = "";
                foreach (var name in findCompanyName)
                {
                    ViewData["CompanyName"] = name.COMPNM;
                }

            }
            catch
            {

            }
        }

    }
}
