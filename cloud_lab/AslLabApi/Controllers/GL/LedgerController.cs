using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class LedgerController : AppController
    {
        private AslLabApiContext db = new AslLabApiContext();

        // GET: /PosLedger/

        public ActionResult Index()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult Index(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["POS_Ledger"] = model;
            return RedirectToAction("LedgerReport");
        }
        public ActionResult LedgerReport()
        {
            PageModel model = (PageModel)TempData["POS_Ledger"];
            return View(model);
        }


        public ActionResult CashBookIndex()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult CashBookIndex(PageModel model)
        {
            TempData["CashBook"] = model;
            return RedirectToAction("CashBookReport");
        }
        public ActionResult CashBookReport()
        {
            PageModel model = (PageModel)TempData["CashBook"];
            return View(model);
        }

        public ActionResult BankBookIndex()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }

        [HttpPost]
        public ActionResult BankBookIndex(PageModel model)
        {
            TempData["BankBook"] = model;
            return RedirectToAction("BankBookReport");
        }
        public ActionResult BankBookReport()
        {
            PageModel model = (PageModel)TempData["BankBook"];
            return View(model);
        }
    }
}
