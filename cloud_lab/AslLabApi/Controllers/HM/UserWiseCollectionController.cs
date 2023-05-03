using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.HM
{
    public class UserWiseCollectionController : AppController
    {
        //
        // GET: /UserWiseCollection/
        public ActionResult UserWiseCollectionSummary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserWiseCollectionSummary(OpdReceiveDTO model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["UserWiseCollection"] = model;
            return RedirectToAction("Summary");
        }
        public ActionResult Summary()
        {
            OpdReceiveDTO model = (OpdReceiveDTO)TempData["UserWiseCollection"];
            return View(model);
        }


        public ActionResult UserWiseCollectionDetails()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UserWiseCollectionDetails(OpdReceiveDTO model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["UserWiseCollectionDetails"] = model;
            return RedirectToAction("Details");
        }
        public ActionResult Details()
        {
            OpdReceiveDTO model = (OpdReceiveDTO)TempData["UserWiseCollectionDetails"];
            return View(model);
        }
	}
}