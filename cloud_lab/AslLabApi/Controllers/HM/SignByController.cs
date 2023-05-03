using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.HM
{
    public class SignByController : AppController
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

        public SignByController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
        // GET: /SignBy/
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SignByDTO model)
        {

            //if (model.AuthorName != null && model.DepartmentName!=null)
            //{
               
                
            //        var findifexist = (from n in db.HMS_SIGNBY
            //            where
            //                n.DEPTNM == model.DepartmentName && n.AUTHNM == model.AuthorName && n.COMPID == model.COMPID
            //            select n).ToList();

            //        SignBy objSignBy = new SignBy();

            //        if (findifexist.Count == 0)
            //        {
            //            var maxdatafind = (from n in db.HMS_SIGNBY select n.SIGNID).Max();
            //            if (maxdatafind == null)
            //            {
            //                objSignBy.COMPID = model.COMPID;
            //                objSignBy.SIGNID = Convert.ToInt64(Convert.ToString(model.COMPID) + "01");
            //                objSignBy.DEPTNM = model.DepartmentName;
            //                objSignBy.AUTHNM = model.AuthorName;
            //                objSignBy.INSTITUTE = model.Institute;
            //                objSignBy.TITLE = model.Title;




            //                db.HMS_SIGNBY.Add(objSignBy);

            //                db.SaveChanges();



            //            }
            //            else
            //            {
            //                objSignBy.COMPID = model.COMPID;
            //                objSignBy.SIGNID = maxdatafind + 1;
            //                objSignBy.DEPTNM = model.DepartmentName;
            //                objSignBy.AUTHNM = model.AuthorName;
            //                objSignBy.INSTITUTE = model.Institute;
            //                objSignBy.TITLE = model.Title;

            //                db.HMS_SIGNBY.Add(objSignBy);
            //                db.SaveChanges();


            //            }
            //            TempData["message"] = "Author Info Created";
            //        }
            //        else
            //        {
            //            TempData["Message"] = "Duplicate Data Couldn't Be Created";
            //        }
                
                
            


            //}
            return RedirectToAction("Create");
        }


        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_SignByLogData(SignByDTO model)
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





            aslLog.LOGDATA = Convert.ToString("Author Name: " + model.AuthorName + ",\nTitle: " + model.Title + ",\nDepartment: " + model.DepartmentName + ",\nInstitute: " + model.Institute);
            aslLog.TABLEID = "HMS_SignBy";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }
        //this view table works partial
        public PartialViewResult SignByInfo(Int64 companyID, string AuthorName, string Title, string Departmentname, string Institute, string Insltude, string Insuserid)
        {
            SignByDTO model=new SignByDTO();

            if (AuthorName != "" && Departmentname != "")
            {
                var find_koita_ache = (from n in db.HMS_SIGNBY where n.COMPID == companyID select n).ToList();
    if (find_koita_ache.Count < 5)
            {
                var findifexist = (from n in db.HMS_SIGNBY where n.COMPID == companyID && n.DEPTNM == Departmentname && n.AUTHNM == AuthorName select n).ToList();

                SignBy objSignBy = new SignBy();

                if (findifexist.Count == 0)
                {
                    var maxdatafind = (from n in db.HMS_SIGNBY where n.COMPID == companyID select n.SIGNID).Max();
                    if (maxdatafind == null)
                    {
                        objSignBy.COMPID = companyID;
                        objSignBy.SIGNID = Convert.ToInt64(Convert.ToString(companyID) + "01");
                        objSignBy.DEPTNM = Departmentname;
                        objSignBy.AUTHNM = AuthorName;
                        objSignBy.INSTITUTE = Institute;
                        objSignBy.TITLE = Title;

                        objSignBy.USERPC = strHostName;
                        objSignBy.INSIPNO = ipAddress.ToString();
                        objSignBy.INSLTUDE = Insltude;
                        objSignBy.INSTIME = Convert.ToDateTime(td);
                        objSignBy.INSUSERID = Convert.ToInt64(Insuserid);


                        db.HMS_SIGNBY.Add(objSignBy);

                        model.COMPID = objSignBy.COMPID;
                        model.SignId = objSignBy.SIGNID;
                        model.DepartmentName = objSignBy.DEPTNM;
                        model.AuthorName = objSignBy.AUTHNM;
                        model.Institute = objSignBy.INSTITUTE;
                        model.Title = objSignBy.TITLE;

                        model.USERPC = objSignBy.USERPC;
                        model.INSIPNO = objSignBy.INSIPNO;
                        model.INSLTUDE = objSignBy.INSLTUDE;
                        model.INSTIME = objSignBy.INSTIME;
                        model.INSUSERID = objSignBy.INSUSERID;
                        Insert_SignByLogData(model);
                        db.SaveChanges();



                    }
                    else
                    {
                        objSignBy.COMPID = companyID;
                        objSignBy.SIGNID = maxdatafind + 1;
                        objSignBy.DEPTNM = Departmentname;
                        objSignBy.AUTHNM = AuthorName;
                        objSignBy.INSTITUTE = Institute;
                        objSignBy.TITLE = Title;

                        objSignBy.USERPC = strHostName;
                        objSignBy.INSIPNO = ipAddress.ToString();
                        objSignBy.INSLTUDE = Insltude;
                        objSignBy.INSTIME = Convert.ToDateTime(td);
                        objSignBy.INSUSERID = Convert.ToInt64(Insuserid);

                        db.HMS_SIGNBY.Add(objSignBy);

                        model.COMPID = objSignBy.COMPID;
                        model.SignId = objSignBy.SIGNID;
                        model.DepartmentName = objSignBy.DEPTNM;
                        model.AuthorName = objSignBy.AUTHNM;
                        model.Institute = objSignBy.INSTITUTE;
                        model.Title = objSignBy.TITLE;

                        model.USERPC = objSignBy.USERPC;
                        model.INSIPNO = objSignBy.INSIPNO;
                        model.INSLTUDE = objSignBy.INSLTUDE;
                        model.INSTIME = objSignBy.INSTIME;
                        model.INSUSERID = objSignBy.INSUSERID;

                        Insert_SignByLogData(model);
                        db.SaveChanges();


                    }
                    TempData["message"] = "Author Info Created";
                }
                else
                {
                    TempData["Message"] = "Duplicate Data Couldn't Be Created";
                }
            }
    else
    {
        TempData["Message"] = "You could not create more than 5 Entry";
    }

              


            }
           

            List<SignBy> listsignBy = new List<SignBy>();
          
          
                listsignBy = db.HMS_SIGNBY.Where(e => e.COMPID == companyID).ToList();
                return PartialView("~/Views/SignBy/_SignByInfo.cshtml", listsignBy);
            
            

        }
        public ActionResult Update()
        {
            return View();
        }


        public JsonResult AuthorListTag(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());




            var tags = from p in db.HMS_SIGNBY
                       where p.COMPID == compid
                       select p.AUTHNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);



        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DynamicautocompleteSignBy(string changedText)
        {
            using (var db = new AslLabApiContext())
            {
                var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());


                String name = "";

                var rt = db.HMS_SIGNBY.Where(n => n.AUTHNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                {
                    headname = n.AUTHNM

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



                String Signid = "";
                string Title="",deptName="",Institute="";

                var rt2 = db.HMS_SIGNBY.Where(n => n.AUTHNM == name && n.COMPID == compid).Select(n => new
                {
                    signid = n.SIGNID,
                   
                   title=n.TITLE,
                    deptname = n.DEPTNM,
                    institute=n.INSTITUTE
                   

                });
                foreach (var n in rt2)
                {
                    Signid = Convert.ToString(n.signid);
                    Title = n.title;
                    deptName = n.deptname;
                    Institute = n.institute;

                }



                var result = new { Authorname = name, SignId = Signid, Title = Title, Deptname = deptName, Institute = Institute };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
        }

        public PartialViewResult SignByInfoUpdate(Int64 companyID,Int64 Signid,string AuthorName, string Title, string Departmentname, string Institute)
        {
            if (AuthorName != "" && Departmentname != "")
            {
                var find_that_id = (from n in db.HMS_SIGNBY where n.COMPID==companyID && n.SIGNID == Signid select n).ToList();
                //var find_nameExist =(from n in db.KART_CATEGORY where n.CATNM == model.CategoryName select n).ToList().Count();

                if (find_that_id.Count != 0)
                {


                    foreach (var x in find_that_id)
                    {
                        x.AUTHNM = AuthorName;
                        x.TITLE = Title;
                        x.DEPTNM = Departmentname;
                        x.INSTITUTE = Institute;


                        db.SaveChanges();


                    }

                    TempData["message"] = "Author Info Updated";

                }


            }
            else
            {
                TempData["message"] = "Something went wrong";
            }


            List<SignBy> listsignBy = new List<SignBy>();


            listsignBy = db.HMS_SIGNBY.Where(e => e.COMPID == companyID).ToList();
            return PartialView("~/Views/SignBy/_SignByInfo.cshtml", listsignBy);



        }

	}
}