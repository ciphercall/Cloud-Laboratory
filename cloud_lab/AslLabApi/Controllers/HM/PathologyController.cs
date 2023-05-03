using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.HM
{
    public class PathologyController : AppController
    {
        //
        // GET: /Pathology/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OpdDTO model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["PathologyStatement"] = model;
            return RedirectToAction("PathologyStatement");
        }
        public ActionResult PathologyStatement()
        {
            OpdDTO model = (OpdDTO)TempData["PathologyStatement"];
            return View(model);
        }
	}
}