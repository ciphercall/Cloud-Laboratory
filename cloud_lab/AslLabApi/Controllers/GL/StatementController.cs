using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class StatementController : AppController
    {
        //
        // GET: /Statement/

        public ActionResult RPDetailsIndex()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult RPDetailsIndex(PageModel model)
        {
            TempData["Details"] = model;
            return RedirectToAction("StatementDetailsReport");
        }

        public ActionResult StatementDetailsReport()
        {
            PageModel model = (PageModel)TempData["Details"];
            return View(model);
        }



        public ActionResult RPSummaryIndex()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult RPSummaryIndex(PageModel model)
        {
            TempData["Summary"] = model;
            return RedirectToAction("StatementSummaryReport");
        }

        public ActionResult StatementSummaryReport()
        {
            PageModel model = (PageModel)TempData["Summary"];
            return View(model);
        }

    }
}
