using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class TransController : AppController
    {
        
        // GET: /PosTrans/

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
            return RedirectToAction("TransReport");
        }
        public ActionResult TransReport()
        {
            PageModel model = (PageModel)TempData["POS_Ledger"];
            return View(model);
        }

    }
}
