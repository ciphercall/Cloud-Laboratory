using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;

namespace AslLabApi.Controllers
{
    public class CostPoolReportController : AppController
    {
        AslLabApiContext db=new AslLabApiContext();
        // GET: /CostPoolReport/

        public ActionResult CostPoolSingle()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }


        [HttpPost]
        public ActionResult CostPoolSingle(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["CostPoolSingle"] = model;
            return RedirectToAction("CostPoolSingleReport");
        }
        public ActionResult CostPoolSingleReport()
        {
            PageModel model = (PageModel)TempData["CostPoolSingle"];
            return View(model);
        }






        public ActionResult CostPoolTranSummary()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }


        [HttpPost]
        public ActionResult CostPoolTranSummary(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["CostPoolTranSummary"] = model;
            return RedirectToAction("CostPoolTranSummaryReport");
        }
        public ActionResult CostPoolTranSummaryReport()
        {
            PageModel model = (PageModel)TempData["CostPoolTranSummary"];
            return View(model);
        }





        public ActionResult CostPoolTranDetails()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }


        [HttpPost]
        public ActionResult CostPoolTranDetails(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["CostPoolTranDetails"] = model;
            return RedirectToAction("CostPoolTranDetailsReport");
        }
        public ActionResult CostPoolTranDetailsReport()
        {
            PageModel model = (PageModel)TempData["CostPoolTranDetails"];
            return View(model);
        }



        public ActionResult CostPoolHead()
        {
            ViewData["HighLight_Menu_GL_Report"] = "heighlight";
            return View();
        }


        [HttpPost]
        public ActionResult CostPoolHead(PageModel model)
        {
            //var pdf = new PdfResult(aCnfJobModel, "GetJOBRegister_JobTypeReport");
            //return pdf;

            TempData["CostPoolHead"] = model;
            return RedirectToAction("CostPoolHeadReport");
        }
        public ActionResult CostPoolHeadReport()
        {
            PageModel model = (PageModel)TempData["CostPoolHead"];
            return View(model);
        }







        public JsonResult TagSearch(string term)
        {
            var compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());





            var tags = from p in db.GlCostPoolDbSet
                where p.COMPID == compid
                select new {p.COSTPNM, p.COSTPID};

            return this.Json(tags.Where(t => t.COSTPNM.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }

        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());


            String itemId = "";

            var rt = db.GlCostPoolDbSet.Where(n => n.COSTPNM.StartsWith(changedText) &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costname = n.COSTPNM

                                                         }).ToList();
            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n.costname);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }

                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }

                    }

                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }

                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }



            }
            else
            {
                itemId = changedText;
            }
            













            String itemId2 = "";

            var rt2 = db.GlCostPoolDbSet.Where(n => n.COSTPNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costpid = n.COSTPID,

                                                         });
            foreach (var n in rt2)
            {
                itemId2 = Convert.ToString(n.costpid);

            }

            var result = new {Costname=itemId, Costpid = itemId2 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        public JsonResult TagSearch2(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());





            var tags = from p in db.GLCostPMSTDbSet
                where p.COMPID == compid
                select new {p.COSTCNM, p.COSTCID};

            return this.Json(tags.Where(t => t.COSTCNM.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }

        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged2(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());


            String itemId = "";

            var rt = db.GLCostPMSTDbSet.Where(n => n.COSTCNM.StartsWith(changedText) &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costname = n.COSTCNM

                                                         }).ToList();
            int lenChangedtxt = changedText.Length;

            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if (changedText != "" && cont != 0)
            {
                while (status != "no")
                {
                    k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n.costname);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss = subss.ToUpper();
                        }
                        else
                        {
                            subss = "";
                        }


                        if (subss == changedText.ToUpper())
                        {
                            k++;
                        }
                        if (k == cont)
                        {
                            status = "yes";
                            lenChangedtxt++;
                            if (ss.Length > lenChangedtxt - 1)
                            {
                                changedText = changedText + ss[lenChangedtxt - 1];
                            }

                        }
                        else
                        {
                            status = "no";
                            //lenChangedtxt--;
                        }

                    }

                }
                if (lenChangedtxt == 1)
                {
                    itemId = changedText.Substring(0, lenChangedtxt);
                }

                else
                {
                    itemId = changedText.Substring(0, lenChangedtxt - 1);
                }



            }
            else
            {
                itemId = changedText;
            }
            




            String itemId2 = "";

            var rt2 = db.GLCostPMSTDbSet.Where(n => n.COSTCNM == changedText &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costcid = n.COSTCID,

                                                         });
            foreach (var n in rt2)
            {
                itemId2 = Convert.ToString(n.costcid);

            }

            var result = new {Costname=itemId, Costcid = itemId2 };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
