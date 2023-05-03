using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.HM
{
    public class DepartmentController : AppController
    {
        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;


        AslLabApiContext db = new AslLabApiContext();


        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public DepartmentController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_DepartmentLogData(DepartmentDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = Convert.ToInt64(model.INSUSERID);
            aslLog.LOGTYPE = "Insert";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.INSIPNO;
            aslLog.LOGLTUDE = model.INSLTUDE;


         


            aslLog.LOGDATA = Convert.ToString("Department Name: " + model.DepartmentName + ",\nRemarks: " + model.Remarks);
            aslLog.TABLEID = "HMS_DEPT";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        // GET: /Department/
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentDTO model)
        {
            
            if (model.DepartmentName != null)
            {
                var findifexist =
                (from n in db.HMS_DEPT where n.COMPID==model.COMPID && n.DEPTNM == model.DepartmentName select n).ToList();
                Department objDept = new Department();
                if (findifexist.Count == 0)
                {
                    var maxdatafind = (from n in db.HMS_DEPT where n.COMPID==model.COMPID select n.DEPTID).Max();
                    if (maxdatafind == null)
                    {
                        objDept.COMPID = model.COMPID;
                        objDept.DEPTID = Convert.ToInt64(Convert.ToString(model.COMPID)+"01");
                        objDept.DEPTNM = model.DepartmentName;
                        objDept.REMARKS = model.Remarks;
                        objDept.USERPC = strHostName;
                        objDept.INSIPNO = ipAddress.ToString();
                        objDept.INSTIME = Convert.ToDateTime(td);
                        objDept.INSLTUDE = model.INSLTUDE;
                        objDept.INSUSERID = model.INSUSERID;
                        


                        db.HMS_DEPT.Add(objDept);

                        model.ID = objDept.ID;
                        model.COMPID = objDept.COMPID;
                        model.DepartmentId = objDept.DEPTID;
                        model.DepartmentName = objDept.DEPTNM;
                        model.Remarks = objDept.REMARKS;
                        model.USERPC = objDept.USERPC;
                        model.INSIPNO = objDept.INSIPNO;
                        model.INSTIME = objDept.INSTIME;

                        Insert_DepartmentLogData(model);
                        db.SaveChanges();



                    }
                    else
                    {
                        objDept.COMPID = model.COMPID;
                        objDept.DEPTID = maxdatafind + 1;
                        objDept.DEPTNM = model.DepartmentName;
                        objDept.REMARKS = model.Remarks;
                        objDept.USERPC = strHostName;
                        objDept.INSIPNO = ipAddress.ToString();
                        objDept.INSTIME = Convert.ToDateTime(td);
                        objDept.INSLTUDE = model.INSLTUDE;
                        objDept.INSUSERID = model.INSUSERID;

                        db.HMS_DEPT.Add(objDept);

                        model.ID = objDept.ID;
                        model.COMPID = objDept.COMPID;
                        model.DepartmentId = objDept.DEPTID;
                        model.DepartmentName = objDept.DEPTNM;
                        model.Remarks = objDept.REMARKS;
                        model.USERPC = objDept.USERPC;
                        model.INSIPNO = objDept.INSIPNO;
                        model.INSTIME = objDept.INSTIME;

                        Insert_DepartmentLogData(model);
                        db.SaveChanges();


                    }
                }
                else
                {
                    TempData["ItemMessage"] = "Duplicate Data Couldn't Be Created";
                }
            

            }
            return RedirectToAction("Create");
        }

        public ActionResult Update()
        {
            return View();
        }


        public void Update_DepartmentLogData(DepartmentDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = Convert.ToInt64(model.UPDUSERID);
            aslLog.LOGTYPE = "Update";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;





            aslLog.LOGDATA = Convert.ToString("Department Name: " + model.DepartmentName + ",\nRemarks: " + model.Remarks);
            aslLog.TABLEID = "HMS_DEPT";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(DepartmentDTO model)
        {
            var find_that_id = (from n in db.HMS_DEPT where n.COMPID==model.COMPID && n.DEPTID == model.DepartmentId select n).ToList();
            //var find_nameExist =(from n in db.KART_CATEGORY where n.CATNM == model.CategoryName select n).ToList().Count();
            var LoggedUserId = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (find_that_id.Count != 0)
            {


                foreach (var x in find_that_id)
                {
                    x.DEPTNM = model.DepartmentName;
                    x.REMARKS = model.Remarks;
                    x.USERPC = x.USERPC;
                    x.INSIPNO = x.INSIPNO;
                    x.INSLTUDE = x.INSLTUDE;
                    x.INSUSERID = x.INSUSERID;
                    x.INSTIME = x.INSTIME;

                    x.UPDIPNO = ipAddress.ToString();
                    x.UPDLTUDE = model.UPDLTUDE;
                    x.UPDUSERID = LoggedUserId;
                    x.UPDTIME = Convert.ToDateTime(td);


                    model.COMPID = x.COMPID;
                    model.UPDUSERID = x.UPDUSERID;
                    model.UPDIPNO = x.UPDIPNO;
                    model.USERPC = x.USERPC;

                    Update_DepartmentLogData(model);
                    db.SaveChanges();


                }

                TempData["DeptID"] = "";


            }
            return RedirectToAction("Update");
        }


        //AutoComplete
        public JsonResult DepartmentListTag(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());




            var tags = from p in db.HMS_DEPT
                       where p.COMPID==compid
                       select p.DEPTNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DynamicautocompleteDept(string changedText)
        {
            using (var db = new AslLabApiContext())
            {
                var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                String name = "";

                var rt = db.HMS_DEPT.Where(n => n.DEPTNM.StartsWith(changedText) && n.COMPID==compid).Select(n => new
                {
                    headname = n.DEPTNM

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
                            string ss = Convert.ToString(n.headname);
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
                        name = changedText.Substring(0, lenChangedtxt);
                    }

                    else
                    {
                        name = changedText.Substring(0, lenChangedtxt - 1);
                    }



                }
                else
                {
                    name = changedText;
                }



                String DeptId = "";
                string Remarks = "";

                var rt2 = db.HMS_DEPT.Where(n => n.DEPTNM == name && n.COMPID==compid).Select(n => new
                {
                    deptid = n.DEPTID,
                    deptname = n.DEPTNM,
                    remarks = n.REMARKS

                });
                foreach (var n in rt2)
                {
                    DeptId = Convert.ToString(n.deptid);
                    Remarks = Convert.ToString(n.remarks);

                }



                var result = new { DepartmentName = name, DeptID = DeptId, REMARKS = Remarks };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
        }
	}
}