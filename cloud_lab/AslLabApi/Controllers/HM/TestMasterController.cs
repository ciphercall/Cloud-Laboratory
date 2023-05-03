using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.HM
{
    public class TestMasterController : AppController
    {
        //
        // GET: /TestMaster/
        public ActionResult Index()
        {
            return View();
        }

        // [AcceptVerbs("POST")]
        // [ActionName("Index")]
        //public ActionResult IndexPost(TestMasterDTO model, string command)
        //{
        //     if (command == "Update Head")
        //     {
        //         return View("Update");
        //     }
        //     else
        //     {
        //         return View("Index");
        //     }
           
        //}
        public ActionResult Update()
        {
            return View();
        }
	}
}