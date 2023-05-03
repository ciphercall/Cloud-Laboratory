using AslLabApi.Models;
using AslLabApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AslLabApi.Controllers.HM
{
    public class DailyReportController : AppController
    {
        //
        // GET: /DailyReport/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(OpdDTO model,string command)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;
            if (command == "Print")
            {
                TempData["DailyOutdoorReport"] = model;
                return RedirectToAction("DailyOutdoorReport");
            }
            else if (command == "MisMatch Report Print")
            {

                TempData["GetMisMatchReport"] = model;

                return RedirectToAction("MisMatchReport");
               
            }
            else
                return null;
           
        }
        public ActionResult MisMatchReport()
        {
            OpdDTO model = (OpdDTO)TempData["GetMisMatchReport"];
            return View(model);
        }
        public ActionResult DailyOutdoorReport()
        {

            OpdDTO model = (OpdDTO)TempData["DailyOutdoorReport"];
            return View(model);
        }
	}
}