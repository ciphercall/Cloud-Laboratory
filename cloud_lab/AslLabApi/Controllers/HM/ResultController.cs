using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;


namespace AslLabApi.Controllers.HM
{
 
    public class ResultController : AppController
    {
        AslLabApiContext db = new AslLabApiContext();
        // GET: /Result/
        public ActionResult Index()
        {
            return View();
        }


    
        public JsonResult restidload(string TestID)
        {
            Int64 comid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
            Int64 testid = Convert.ToInt64(TestID);
            List<SelectListItem> restid = new List<SelectListItem>();
          

           



                var ans1 = from n in db.HMS_TESTNV where n.COMPID == comid && n.TESTID==testid  select new { n.RESTID, n.RESTNM };
                foreach (var x in ans1)
                {
                    restid.Add(new SelectListItem { Text = x.RESTNM, Value = Convert.ToString(x.RESTID) });
                }







                return Json(new SelectList(restid, "Value", "Text"));

        }

      //[Route("Result/grid/RestLoad2")]
      //[HttpPost]
      public JsonResult restidload2(string changedtxt)
      {
          Int64 comid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
          Int64 testid = Convert.ToInt64(changedtxt);
          List<SelectListItem> restid = new List<SelectListItem>();






          var ans1 = from n in db.HMS_TESTNV where n.COMPID == comid && n.TESTID == testid select new { n.RESTID, n.RESTNM };
          foreach(var x in ans1)
          {
              restid.Add(new SelectListItem { Text = x.RESTNM, Value = Convert.ToString(x.RESTID) });
          }







          return Json(new SelectList(restid, "Value", "Text"));

      }


        public JsonResult testidload(string patientid)
        {
            Int64 comid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
          
            List<SelectListItem> restid = new List<SelectListItem>();






            var ans = (from t1 in db.HMS_OPD
                           join t2 in db.HMS_TEST on t1.TESTID equals t2.TESTID
                       where t1.COMPID == comid && t1.PATIENTID == patientid 
                           select new
                           {
                               testid=t2.TESTID,
                               testname=t2.TESTNM


                           });
            foreach (var x in ans)
            {
                restid.Add(new SelectListItem { Text = x.testname, Value = Convert.ToString(x.testid) });
            }







            return Json(new SelectList(restid, "Value", "Text"));

        }

	}
}