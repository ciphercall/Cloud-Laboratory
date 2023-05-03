using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.HM
{
    public class PatientWiseTestController : AppController
    {
        //
        // GET: /PatientWiseTest/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OpdDTO model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["PatientWiseTest"] = model;
            return RedirectToAction("PatientWiseTestReport");
        }
        public ActionResult PatientWiseTestReport()
        {
            OpdDTO model = (OpdDTO)TempData["PatientWiseTest"];
            return View(model);
        }



    }
}