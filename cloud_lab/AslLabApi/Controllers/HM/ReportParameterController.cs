using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.ASL;

namespace AslLabApi.Controllers.HM
{
    public class ReportParameterController : AppController
    {
        //
        AslLabApiContext db = new AslLabApiContext();
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        // GET: /ReportParameter/
        public ActionResult Index()
        {
            return View();
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(OpdDTO model,string command)
        {
            if (command == "Top - Only")
            {
                TempData["TestPad"] = model;
                return RedirectToAction("TestPad");
            }

            else if (command == "Top - USG/X-ray")
            {
                TempData["Test_Pad_USG_XRAY"] = model;
                return RedirectToAction("TestPadUsgXray");
            }
            else if (command == "Money Receipt")
            {
                TempData["LabOrder"] = model;
                return RedirectToAction("LabOrder");
            }
            else if (command == "Submit")
            {
                TempData["Pathological_report"] = model;
                return RedirectToAction("PathologicalReport");
            }
            else if (command == "Money Receipt")
            {
                var data_fetch = (from n in db.HMS_OPDMST where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n).ToList();
                foreach (var x in data_fetch)
                {
                    var search_doctor = (from n in db.HMS_REFER where n.COMPID == model.CompanyID && n.REFERID == x.DOCTORID select n).ToList();
                    var searh_refer = (from n in db.HMS_REFER where n.COMPID == model.CompanyID && n.REFERID == x.REFERID select n).ToList();
                    string doctorname = "", refername = "";
                    foreach (var d in search_doctor)
                    {
                        doctorname = d.REFERNM;
                    }
                    foreach (var d in searh_refer)
                    {
                        refername = d.REFERNM;
                    }
                    model.ID = x.ID;
                    model.CompanyID = x.COMPID;
                    model.PatientID = x.PATIENTID;
                    model.PatientName = x.PATIENTNM;
                    model.TransactionDate = x.TRANSDT;
                    model.Gender = x.GENDER;
                    model.Age = x.AGE;
                    model.MObileNo = x.MOBNO;
                    model.Address = x.ADDRESS;
                    model.DoctorID = x.DOCTORID;
                    model.ReferID = x.REFERID;
                    model.Discountnet = x.DISCNET;
                    model.NetAmount = x.NETAMT;
                    model.ReceiveAmount = x.RCVAMT;
                    model.DueAmount = Convert.ToDecimal(x.DUEAMT);
                    model.DoctorName = doctorname;
                    model.Refername = refername;
                    model.DvDate = x.DVDT;
                    model.Dvtm = x.DVTM;

                }

                TempData["GetOpdBillReport"] = model;

                return RedirectToAction("GetOpdBillReport");
            }
            else if (command == "Money Receipt - Due")
            {
                OpdDTO obj2 = new OpdDTO();
                obj2.CompanyID = model.CompanyID;
                obj2.PatientID = model.PatientID;

                obj2.TransactionDate = model.TransactionDate;
                obj2.PatientName = model.PatientName;
                var search_info = (from n in db.HMS_OPDMST where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n).ToList();
                foreach (var x in search_info)
                {
                    obj2.MObileNo = x.MOBNO;
                    obj2.Age = x.AGE;
                    obj2.Gender = x.GENDER;
                    var doctor = (from n in db.HMS_REFER where n.COMPID == model.CompanyID && n.REFERID == x.DOCTORID select n).ToList();
                    foreach (var xx in doctor)
                    {
                        obj2.DoctorName = xx.REFERNM;
                    }

                    var refer = (from n in db.HMS_REFER where n.COMPID == model.CompanyID && n.REFERID == x.REFERID select n).ToList();
                    foreach (var xx in refer)
                    {
                        obj2.Refername = xx.REFERNM;
                    }

                    obj2.Discountnet = x.DISCNET;
                    obj2.NetAmount = x.NETAMT;
                    obj2.ReceiveAmount = x.RCVAMT;
                    obj2.DueAmount = Convert.ToDecimal(x.DUEAMT);
                    obj2.DvDate = x.DVDT;
                    obj2.Dvtm = x.DVTM;
                    obj2.TransactionDate = x.TRANSDT;
                }

                var receivedue = (from n in db.HMS_OPDRCV where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n).ToList();
                decimal sumdicount = 0, sumreceive = 0;
                foreach (var x in receivedue)
                {
                    sumdicount += Convert.ToDecimal(x.DISCHOS);
                    sumreceive += Convert.ToDecimal(x.RCVAMT);

                }
                obj2.DueDiscountHos = Convert.ToDecimal(sumdicount);

                obj2.DueReceiveAmount = Convert.ToDecimal(sumreceive);
                obj2.DueDueAmount = obj2.DueAmount - Convert.ToDecimal(sumdicount) - Convert.ToDecimal(sumreceive);

                TempData["GetDueBillReport"] = obj2;

                return RedirectToAction("GetDueBillReport");
            }
            else if (command == "SMS - Report")
            {
                ASL_PSMS obj = new ASL_PSMS();
                obj.COMPID = model.CompanyID;
                var search_patientinfo = (from n in db.HMS_OPDMST where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n).ToList();
                foreach(var x in search_patientinfo)
                {
                    obj.TRANSYY = Convert.ToInt64(Convert.ToString(x.TRANSNO).Substring(0, 3));
                    obj.TRANSNO = Convert.ToInt64(x.TRANSNO);
                    obj.TRANSDT = DateTime.Now;
                    obj.MOBNO = x.MOBNO;
                    obj.LANGUAGE = "ENGLISH";
                    obj.MSGTP = "REPORT";
                    obj.MESSAGE = "REPORT READY";
                    obj.STATUS = "PENDING";
                    obj.SENTTM = null;
                    obj.USERPC = x.USERPC;
                    obj.INSIPNO = x.INSIPNO;
                    obj.INSLTUDE = x.INSLTUDE;
                    obj.INSTIME = x.INSTIME;
                    obj.INSUSERID = Convert.ToInt64(x.INSUSERID);
                }
                

                db.SendLogSMSDbSet.Add(obj);




                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return null;
            }
           
        }

        public ActionResult GetOpdBillReport()
        {
            OpdDTO model = (OpdDTO)TempData["GetOpdBillReport"];
            return View(model);
        }

        public ActionResult GetDueBillReport()
        {
            OpdDTO model = (OpdDTO)TempData["GetDueBillReport"];
            return View(model);
        }
        public ActionResult TestPad()
        {

            OpdDTO model = (OpdDTO)TempData["TestPad"];
            return View(model);
        }

        public ActionResult TestPadUsgXray()
        {
            OpdDTO model = (OpdDTO)TempData["Test_Pad_USG_XRAY"];
            return View(model);
        }
        public ActionResult LabOrder()
        {
            OpdDTO model = (OpdDTO)TempData["LabOrder"];
            return View(model);
        }

        public ActionResult PathologicalReport()
        {
            OpdDTO model = (OpdDTO)TempData["Pathological_report"];
            return View(model);
        }
        
        public JsonResult testorCat(string Type,string PatientdID)
        {
            Int64 comid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
         
            List<SelectListItem> testorcatList = new List<SelectListItem>();
            var data_from_opd =
                (from n in db.HMS_OPD where n.COMPID == comid && n.PATIENTID == PatientdID select n).ToList();



            if (Type == "Category")
            {

                var ans1 = from n in db.HMS_TESTMST where n.COMPID == comid  select new { n.TCATID, n.TCATNM };
                foreach (var opd in data_from_opd)
                {
                    foreach (var x in ans1)
                    {
                        if (x.TCATID == opd.TCATID)
                        {
                            testorcatList.Add(new SelectListItem { Text = x.TCATNM, Value = Convert.ToString(x.TCATID) });
                        }

                       
                    }
                }
                
            }

            else if (Type == "Test")
            {
                var ans1 = from n in db.HMS_TEST where n.COMPID == comid select new { n.TESTID, n.TESTNM };
                foreach (var opd in data_from_opd)
                {
                   
                    foreach (var x in ans1)
                    {
                        if (x.TESTID == opd.TESTID)
                        {
                            testorcatList.Add(new SelectListItem { Text = x.TESTNM, Value = Convert.ToString(x.TESTID) });
                        }
                       
                    }
                }
                
            }









            return Json(new SelectList(testorcatList, "Value", "Text"));

        }
	}
}