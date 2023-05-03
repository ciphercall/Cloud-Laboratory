using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.DataAccess;

namespace AslLabApi.Controllers.ASL
{
    public class DashBoardController : AppController
    {
        //
        // GET: /ShowPatienttoday
        TimeZoneInfo timeZoneInfo;
        AslLabApiContext db = new AslLabApiContext();

        public HttpCookie userCookie;
        public Int64 loggedcompid;
        public DashBoardController()
        {
            //if (System.Web.HttpContext.Current.Request.Cookies["user"] != null)
            //{
            //    userCookie = System.Web.HttpContext.Current.Request.Cookies["user"];
            //    loggedcompid = Convert.ToInt64(userCookie.Values["loggedCompID"]);
            //}
            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);

            if (System.Web.HttpContext.Current.Session["loggedCompID"] != null)
            {
                loggedcompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            }
            else
            {
                Index();
            }

            ViewData["HighLight_Menu_DashBoard"] = "High Light DashBoard";
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Today()
        {

            return View();
        }



        public ActionResult LastOneDay()
        {
            return View();
        }
        public ActionResult Last7Day()
        {
            return View();
        }
        public ActionResult LastOneMonth()
        {
            return View();
        }

        public ActionResult LastOneYear()
        {
            return View();
        }



        public JsonResult Topcategories(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }



            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopcategoriesListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }



        public JsonResult TopItemsByQty(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }

            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopItemsByQtyListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }








        public JsonResult TopItemsByValue(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }


            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TopItemsByValueListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }








        public JsonResult CollectionData(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }


            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.CollectionDataListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }









        public JsonResult TimeWiseSellData(string d)
        {
            Int64 n = Convert.ToInt64(d);
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string frdate = dt.ToString("yyyy-MM-dd");
            string todate = "";

            if (n == 364)
            {
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                todate = firstDay.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dt2 = TimeZoneInfo.ConvertTime(DateTime.Today.AddDays(-n), timeZoneInfo);
                todate = dt2.ToString("yyyy-MM-dd");
            }

            mydataservice objMD = new mydataservice();
            var chartsdata = objMD.TimeWiseSellDataListdata(loggedcompid, todate, frdate); // calling method Listdata
            return Json(chartsdata, JsonRequestBehavior.AllowGet); // returning list from here.
        }


        [HttpPost]
        public ActionResult Index(PageModel model)
        {
            TempData["GetMisMatchReport"] = model;

            return RedirectToAction("MisMatchReport");


        }
        //public ActionResult MissMatch(PageModel model)
        //{

        //    TempData["GetMissMatchReport"] = model;

        //    return RedirectToAction("MissMatchReport");


        //}
        public ActionResult MisMatchReport()
        {
            PageModel model = (PageModel)TempData["GetMisMatchReport"];
            return View(model);
        }

        public ActionResult IndexPost(Int64 id, Int64 compid, string patientid)
        {
            OpdDTO model = new OpdDTO();
            var data_fetch = (from n in db.HMS_OPDMST where n.ID == id && n.COMPID == compid && n.PATIENTID == patientid select n).ToList();
            foreach(var x in data_fetch)
            {
                var search_doctor = (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID == x.DOCTORID select n).ToList();
                var searh_refer = (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID == x.REFERID select n).ToList();
                string doctorname = "",refername="";
                foreach(var d in search_doctor)
                {
                    doctorname = d.REFERNM;
                }
                foreach (var d in searh_refer)
                {
                    refername = d.REFERNM;
                }
                model.ID = x.ID;
                model.CompanyID = x.COMPID;
                model.PatientID = x.PATIENTID;
                model.PatientName = x.PATIENTNM;
                model.TransactionDate = x.TRANSDT;
                model.Gender = x.GENDER;
                model.Age = x.AGE;
                model.MObileNo = x.MOBNO;
                model.Address = x.ADDRESS;
                model.DoctorID = x.DOCTORID;
                model.ReferID = x.REFERID;
                model.Discountnet = x.DISCNET;
                model.NetAmount = x.NETAMT;
                model.ReceiveAmount = x.RCVAMT;
                model.DueAmount = Convert.ToDecimal(x.DUEAMT);
                model.DoctorName = doctorname;
                model.Refername = refername;
                model.DvDate = x.DVDT;
                model.Dvtm = x.DVTM;
              
            }
            TempData["GetOpdBillReport"] = model;

            return RedirectToAction("GetOpdBillReport");
        }

          public ActionResult GetOpdBillReport()
          {
              OpdDTO model = (OpdDTO)TempData["GetOpdBillReport"];
              return View(model);
          }
    }
}
