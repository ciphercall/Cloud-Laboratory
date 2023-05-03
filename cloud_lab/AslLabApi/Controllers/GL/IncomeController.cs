using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class IncomeController : AppController
    {
        //
        // GET: /Income/

        public ActionResult Index()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageModel model)
        {
            TempData["Income Statement"] = model;
            return RedirectToAction("IncomeStatementReport");
        }

        public ActionResult IncomeStatementReport()
        {

            PageModel model = (PageModel)TempData["Income Statement"];
            return View(model);
        }
    }
}
