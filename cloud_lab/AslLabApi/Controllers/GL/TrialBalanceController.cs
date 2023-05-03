using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.GL;
using AslLabApi.Models;

namespace AslLabApi.Controllers.GL
{
    public class TrialBalanceController : AppController
    {
      

        // GET: /TrialBalance/

        public ActionResult Index()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageModel model,string command)
        {
           if (command == "Show")
            {
                string date = Convert.ToString(model.AGlMaster.TRANSDT);
                DateTime myDateTime = DateTime.Parse(date);
                string converttoString = myDateTime.ToString("dd-MMM-yyyy");
                TempData["Trial_Balance_Date"] = converttoString;
                TempData["Trial Balance"] = model;
                return RedirectToAction("Index");
            }
            else //if (command == "Print")
            {
                TempData["Trial Balance"] = model;
                return RedirectToAction("TrialBalanceReport");
            }
        }

        public ActionResult TrialBalanceReport()
        {

            PageModel model = (PageModel)TempData["Trial Balance"];
            return View(model);
        }


    }
}
