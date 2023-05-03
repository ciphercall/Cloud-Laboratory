using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AslLabApi.Controllers.HM
{
    public class ListController : AppController
    {
        //
        // GET: /List/
        public ActionResult ListofDepartment()
        {
            return View();
        }
        public ActionResult VaccumInfo()
        {
            return View();
        }
        public ActionResult ListofTest()
        {
            return View();
        }

        public ActionResult TestNormalValues()
        {
            return View();
        }
        public ActionResult ReferInfo()
        {
            return View();
        }

        public ActionResult ManagementInfo()
        {
            return View();
        }

	}
}