using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.HM
{
    public class ReferStatementController : AppController
    {
        //
        // GET: /ReferStatement/
        public ActionResult Index()
        {
            return View();
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(OpdDTO model, string command)
        {
            if (command == "Refer Statement - Patient")
            {
                TempData["ReferStatementP"] = model;
                return RedirectToAction("ReferStatementPatient");
            }

            else if (command == "Refer Statement - Summary")
            {
                TempData["Refer_Statement_Summary"] = model;
                return RedirectToAction("ReferStatementSummary");
            }
            else if (command == "Refer Statement - Details")
            {
                TempData["Refer_statement_details"] = model;
                return RedirectToAction("ReferStatementDetails");
            }

            else
            {
                return null;
            }

        }

        public ActionResult ReferStatementPatient()
        {

            OpdDTO model = (OpdDTO)TempData["ReferStatementP"];
            return View(model);
        }

        public ActionResult ReferStatementSummary()
        {
            OpdDTO model = (OpdDTO)TempData["Refer_Statement_Summary"];
            return View(model);
        }
        public ActionResult ReferStatementDetails()
        {
            OpdDTO model = (OpdDTO)TempData["Refer_statement_details"];
            return View(model);
        }
	}
}