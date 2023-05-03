using System.Data;
using System.Data.SqlClient;
using AslLabApi.Controllers;
using AslLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models.GL;

namespace AslLabApi.Controllers
{
    public class CostPoolController : AppController
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


        public CostPoolController()
        {
            ViewData["HighLight_Menu_GL_Form"] = "heighlight";
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        //create asl_log object
        public ASL_LOG aslLog = new ASL_LOG();

        // SAVE ALL INFORMATION from CNF_ExpenseHeadModel TO Asl_lOG Database Table.
        public void Insert_COSTPOOLMST_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.AGlCostPMST.COMPID && n.USERID == model.AGlCostPMST.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.AGlCostPMST.COMPID);
            aslLog.USERID = model.AGlCostPMST.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.AGlCostPMST.INSIPNO;
            aslLog.LOGLTUDE = model.AGlCostPMST.INSLTUDE;
            aslLog.TABLEID = "GL_COSTPMST";
            aslLog.LOGDATA = Convert.ToString("Group Cost Name: " + model.AGlCostPMST.COSTCNM + ".");
            aslLog.USERPC = model.AGlCostPMST.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }


        public void Insert_GL_CostPool_LogData(PageModel model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.AGlCostPool.COMPID && n.USERID == model.AGlCostPool.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.AGlCostPool.COMPID);
            aslLog.USERID = model.AGlCostPool.INSUSERID;
            aslLog.LOGTYPE = "INSERT";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.AGlCostPool.INSIPNO;
            aslLog.LOGLTUDE = model.AGlCostPool.INSLTUDE;
            aslLog.TABLEID = "Cost Pool";
            string costCName = "";
            var getCostCname = (from m in db.GLCostPMSTDbSet
                                where m.COMPID == aslLog.COMPID && m.COSTCID == model.AGlCostPool.COSTCID
                                select new { m.COSTCNM }).Distinct().ToList();
            foreach (var VARIABLE in getCostCname)
            {
                costCName = VARIABLE.COSTCNM;
            }

            aslLog.LOGDATA = Convert.ToString("Group Cost Name: " + costCName + ",\nCost Head Name: " + model.AGlCostPool.COSTPNM + ",\nCost SID: " + model.AGlCostPool.COSTPSID + ",\nRemarks: " + model.AGlCostPool.REMARKS + ".");
            aslLog.USERPC = model.AGlCostPool.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }



        public void Update_GL_CostPool_LogData(ASL_LOG aslLOGRef)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslLOGRef.COMPID && n.USERID == aslLOGRef.USERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }


            aslLog.COMPID = Convert.ToInt64(aslLOGRef.COMPID);
            aslLog.USERID = aslLOGRef.USERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslLOGRef.LOGIPNO;
            aslLog.LOGLTUDE = aslLOGRef.LOGLTUDE;
            aslLog.TABLEID = "Cost Pool";
            aslLog.LOGDATA = aslLOGRef.LOGDATA;
            aslLog.USERPC = strHostName;
            db.AslLogDbSet.Add(aslLog);
        }




        public void Delete_GL_CostPool_LogData(GL_COSTPOOL model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
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
            aslLog.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;
            aslLog.TABLEID = "COST POOL";

            string costCName = "";
            var getCostCname = (from m in db.GLCostPMSTDbSet
                                where m.COMPID == aslLog.COMPID && m.COSTCID == model.COSTCID
                                select new { m.COSTCNM }).Distinct().ToList();
            foreach (var VARIABLE in getCostCname)
            {
                costCName = VARIABLE.COSTCNM;
            }

            aslLog.LOGDATA = Convert.ToString("Group Cost Name: " + costCName + ",\nCost Head Name: " + model.COSTPNM + ",\nCost SID: " + model.COSTPSID + ",\nRemarks: " + model.REMARKS + ".");
            aslLog.USERPC = model.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }

        public ASL_DELETE AslDelete = new ASL_DELETE();
        public void Delete_ASL_Delete_GL_CostPool_LogData(GL_COSTPOOL model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("d"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));


            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == model.COMPID && n.USERID == model.UPDUSERID select n.DELSLNO).Max());

            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(model.COMPID);
            AslDelete.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = model.UPDIPNO;
            AslDelete.DELLTUDE = model.UPDLTUDE;
            AslDelete.TABLEID = "Cost Pool";

            string costCName = "";
            var getCostCname = (from m in db.GLCostPMSTDbSet
                                where m.COMPID == aslLog.COMPID && m.COSTCID == model.COSTCID
                                select new { m.COSTCNM }).Distinct().ToList();
            foreach (var VARIABLE in getCostCname)
            {
                costCName = VARIABLE.COSTCNM;
            }

            AslDelete.DELDATA = Convert.ToString("Group Cost Name: " + costCName + ",\nCost Head Name: " + model.COSTPNM + ",\nCost SID: " + model.COSTPSID + ",\nRemarks: " + model.REMARKS + ".");
            AslDelete.USERPC = model.USERPC;
            db.AslDeleteDbSet.Add(AslDelete);


        }





        // GET: /CostPool/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            var dt = (PageModel)TempData["costpool"];
            return View(dt);

        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model, string command)
        {
            if (command == "Submit")
            {
                if (model.AGlCostPMST.COSTCNM != null)
                {
                    //Get Ip ADDRESS,Time & user PC Name
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    model.AGlCostPMST.USERPC = strHostName;
                    model.AGlCostPMST.INSIPNO = ipAddress.ToString();
                    model.AGlCostPMST.INSTIME = Convert.ToDateTime(td);
                    //Insert User ID save POS_ITEMMST table attribute (INSUSERID) field
                    model.AGlCostPMST.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                    Int64 companyID = Convert.ToInt64(model.AGlCostPMST.COMPID);

                    Int64 minCategoryId = Convert.ToInt64((from n in db.GLCostPMSTDbSet where n.COMPID == companyID select n.COSTCID).Min());

                    var result = db.GLCostPMSTDbSet.Count(d => d.COSTCNM == model.AGlCostPMST.COSTCNM
                                                                   && d.COMPID == companyID);


                    if (result == 0)
                    {

                        //.........................................................Create Permission Check
                        var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
                        var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

                        var createStatus = "";

                        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
                        string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='CostPool' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable ds = new DataTable();
                        da.Fill(ds);

                        foreach (DataRow row in ds.Rows)
                        {
                            createStatus = row["INSERTR"].ToString();
                        }

                        conn.Close();

                        if (createStatus == 'I'.ToString())
                        {
                            //TempData["cnfExpenseHead"] = model;
                            TempData["costpool"] = model;
                            //TempData["ExpCID"] = model.AGlCostPMST.COSTCID;
                            TempData["CostCID"] = model.AGlCostPMST.COSTCID;
                            TempData["ShowAddButton"] = "Show Add Button";
                            TempData["message"] = "Permission not granted!";
                            return RedirectToAction("Index");
                        }
                        //...............................................................................................


                        AslUserco aslUserco = db.AslUsercoDbSet.FirstOrDefault(r => (r.COMPID == companyID));
                        if (aslUserco == null)
                        {
                            TempData["message"] = " User ID not found ";
                            TempData["ShowAddButton"] = "Show Add Button";
                        }
                        else
                        {
                            Int64 maxData =
                                Convert.ToInt64(
                                    (from n in db.GLCostPMSTDbSet
                                     where n.COMPID == model.AGlCostPMST.COMPID
                                     select n.COSTCID).Max());




                            Int64 R = Convert.ToInt64(model.AGlCostPMST.COMPID + "999");
                            Int64 comId = Convert.ToInt64(model.AGlCostPMST.COMPID);

                            if (maxData == 0)
                            {


                                model.AGlCostPMST.COSTCID = Convert.ToInt64(comId + "001");


                                Insert_COSTPOOLMST_LogData(model);

                                db.GLCostPMSTDbSet.Add(model.AGlCostPMST);
                                db.SaveChanges();

                                TempData["message"] = "\"" + model.AGlCostPMST.COSTCNM +
                                                      "\" successfully saved.\n Please Create the Cost Pool List.";
                                TempData["ShowAddButton"] = "Show Add Button";
                                //TempData["cnfExpenseHead"] = aCnfExpenseHeadModel;
                                TempData["costpool"] = model;
                                TempData["CostCID"] = model.AGlCostPMST.COSTCID;


                                return RedirectToAction("Index");
                            }


                            else if (maxData <= R)
                            {

                                model.AGlCostPMST.COSTCID = maxData + 1;



                                Insert_COSTPOOLMST_LogData(model);

                                db.GLCostPMSTDbSet.Add(model.AGlCostPMST);
                                db.SaveChanges();

                                TempData["message"] = "\"" + model.AGlCostPMST.COSTCNM +
                                                      "\" successfully saved.\n Please Create the Cost Pool List.";
                                TempData["ShowAddButton"] = "Show Add Button";
                                //TempData["cnfExpenseHead"] = aCnfExpenseHeadModel;
                                TempData["costpool"] = model;
                                TempData["CostCID"] = model.AGlCostPMST.COSTCID;

                                return RedirectToAction("Index");
                            }



                            else
                            {
                                TempData["ShowAddButton"] = "Show Add Button";
                                TempData["message"] = "Not possible entry ";
                                return RedirectToAction("Index");
                            }
                        }
                    }

                    else if (result > 0)
                    {
                        var ans = from n in db.GLCostPMSTDbSet
                                  where n.COMPID == model.AGlCostPMST.COMPID
                                      && n.COSTCNM == model.AGlCostPMST.COSTCNM
                                  select new { n.COSTCID };
                        foreach (var a in ans)
                        {
                            model.AGlCostPMST.COSTCID = a.COSTCID;
                        }


                        TempData["ShowAddButton"] = "Show Add Button";
                        //TempData["cnfExpenseHead"] = aCnfExpenseHeadModel;
                        TempData["costpool"] = model;
                        TempData["CostCID"] = model.AGlCostPMST.COSTCID;

                        TempData["latitute_deleteAccount"] = model.AGlCostPMST.INSLTUDE;
                        return RedirectToAction("Index");
                    }


                }
            }
            if (command == "Add")
            {
                //.........................................................Create Permission Check
                var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
                var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

                var createStatus = "";

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
                string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='CostPool' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    createStatus = row["INSERTR"].ToString();
                }

                conn.Close();

                if (createStatus == 'I'.ToString())
                {
                    TempData["ShowAddButton"] = "Show Add Button";
                    TempData["costpool"] = model;
                    TempData["CostCID"] = model.AGlCostPMST.COSTCID;

                    TempData["message"] = "Permission not granted!";
                    return RedirectToAction("Index");
                }
                //...............................................................................................

                model.AGlCostPool.COMPID = model.AGlCostPMST.COMPID;
                model.AGlCostPool.COSTCID = model.AGlCostPMST.COSTCID;

                if (model.AGlCostPMST.COSTCID == null)
                {
                    TempData["message"] = "Enter COSTCID First";
                    return View("Index");
                }
                model.AGlCostPool.USERPC = strHostName;
                model.AGlCostPool.INSIPNO = ipAddress.ToString();
                model.AGlCostPool.INSTIME = td;
                model.AGlCostPool.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                model.AGlCostPool.INSLTUDE = model.AGlCostPMST.INSLTUDE;


                try
                {


                    GL_COSTPMST mst_CompID = db.GLCostPMSTDbSet.FirstOrDefault(r => (r.COMPID == model.AGlCostPMST.COMPID));

                    Int64 CostCID = Convert.ToInt64(model.AGlCostPMST.COSTCID);

                    GL_COSTPMST mst_CostCID = db.GLCostPMSTDbSet.FirstOrDefault(r => (r.COSTCID == CostCID));

                    if (mst_CompID == null && mst_CostCID == null)
                    {
                        TempData["ShowAddButton"] = "Show Add Button";
                        TempData["message"] = "COSTCID not found ";
                        return View("Index");
                    }
                    else
                    {
                        Int64 maxData = Convert.ToInt64((from n in db.GlCostPoolDbSet where n.COMPID == model.AGlCostPMST.COMPID && n.COSTCID == model.AGlCostPMST.COSTCID select n.COSTPID).Max());

                        Int64 R = Convert.ToInt64(CostCID + "9999");

                        if (maxData == 0)
                        {
                            model.AGlCostPool.COSTPID = Convert.ToInt64(CostCID + "0001");

                            Insert_GL_CostPool_LogData(model);

                            db.GlCostPoolDbSet.Add(model.AGlCostPool);
                            if (db.SaveChanges() > 0)
                            {
                                TempData["message"] = "Cost Pool List Successfully Saved";
                                model.AGlCostPool.COSTPNM = "";
                                model.AGlCostPool.COSTPSID = "";
                                model.AGlCostPool.REMARKS = "";




                            }

                        }
                        else if (maxData <= R)
                        {

                            model.AGlCostPool.COSTPID = maxData + 1;

                            Insert_GL_CostPool_LogData(model);

                            db.GlCostPoolDbSet.Add(model.AGlCostPool);
                            db.SaveChanges();
                            TempData["message"] = "Cost Pool Successfully Saved";
                            model.AGlCostPool.COSTPNM = "";
                            model.AGlCostPool.COSTPSID = "";
                            model.AGlCostPool.REMARKS = "";



                        }
                        else
                        {
                            TempData["message"] = "Cost Pool entry not possible";
                            TempData["ShowAddButton"] = "Show Add Button";

                        }
                    }


                }
                catch (Exception ex)
                {

                }
                TempData["ShowAddButton"] = "Show Add Button";
                TempData["costpool"] = model;
                TempData["CostCID"] = model.AGlCostPMST.COSTCID;

                return RedirectToAction("Index");
            }




            if (command == "Update")
            {
                var query =
                    from a in db.GlCostPoolDbSet
                    where (a.COSTPID == model.AGlCostPool.COSTPID && a.COMPID == model.AGlCostPool.COMPID && a.COSTCID == model.AGlCostPMST.COSTCID)
                    select a;
                model.AGlCostPool.COMPID = model.AGlCostPMST.COMPID;
                model.AGlCostPool.COSTCID = model.AGlCostPMST.COSTCID;


                foreach (GL_COSTPOOL a in query)
                {
                    // Insert any additional changes to column values.
                    a.COSTPNM = model.AGlCostPool.COSTPNM;
                    a.COSTPSID = model.AGlCostPool.COSTPSID;
                    a.REMARKS = model.AGlCostPool.REMARKS;


                    a.UPDIPNO = ipAddress.ToString();
                    a.UPDTIME = td;
                    a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    a.UPDLTUDE = model.AGlCostPMST.INSLTUDE;
                    TempData["AccountLogData"] = Convert.ToString("Head ID: " + a.COSTCID + ",\nCost P ID: " + a.COSTPID + ",\nCost P Name: " + a.COSTPNM + ".");

                }

                ASL_LOG aslLogref = new ASL_LOG();

                aslLogref.LOGIPNO = ipAddress.ToString();
                aslLogref.COMPID = model.AGlCostPool.COMPID;
                model.AGlCostPool.INSLTUDE = model.AGlCostPMST.INSLTUDE;
                aslLogref.LOGLTUDE = model.AGlCostPool.INSLTUDE;

                //Update User ID save ASL_ROLE table attribute UPDUSERID
                aslLogref.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                aslLogref.LOGDATA = TempData["AccountLogData"].ToString();

                Update_GL_CostPool_LogData(aslLogref);

                db.SaveChanges();

                TempData["costpool"] = model;
                TempData["CostCID"] = model.AGlCostPMST.COSTCID;

                TempData["ShowAddButton"] = "Show Add Button";
                model.AGlCostPool.COSTPNM = "";
                model.AGlCostPool.COSTPSID = "";

                model.AGlCostPool.REMARKS = "";



                return RedirectToAction("Index");

            }


            return RedirectToAction("Index");
        }




        public ActionResult EditAccountUpdate(Int64 id, Int64 cid, Int64 Costcid, Int64 costPId, string costPName, PageModel model)
        {
            //.........................................................Create Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var updateStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query1 = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='CostPool' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query1, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                updateStatus = row["UPDATER"].ToString();
            }
            conn.Close();

            //add the data from database to model
            var expcid = from m in db.GLCostPMSTDbSet where m.COSTCID == Costcid && m.COMPID == cid select m;
            foreach (var categoryResult in expcid)
            {
                model.AGlCostPMST.COMPID = cid;
                model.AGlCostPMST.COSTCID = Costcid;
                model.AGlCostPMST.COSTCNM = categoryResult.COSTCNM;


            }


            TempData["costpool"] = model;
            TempData["CostCID"] = Costcid;

            TempData["ShowAddButton"] = null;
            if (updateStatus == 'I'.ToString())
            {
                TempData["message"] = "Permission not granted!";
                return RedirectToAction("Index");
            }
            //...............................................................................................

            model.AGlCostPool = db.GlCostPoolDbSet.Find(id);

            var item = from r in db.GlCostPoolDbSet where r.CostPLID == id select r.COSTPNM;
            foreach (var it in item)
            {
                model.AGlCostPool.COSTPNM = it.ToString();
            }

            return RedirectToAction("Index");

        }






        public ActionResult AccountDelete(Int64 id, Int64 cid, Int64 Costcid, Int64 costPId, string costPName, PageModel model)
        {
            //.........................................................Create Permission Check
            var LoggedCompId = System.Web.HttpContext.Current.Session["loggedCompID"].ToString();
            var loggedUserID = System.Web.HttpContext.Current.Session["loggedUserID"].ToString();

            var deleteStatus = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("SELECT STATUS, INSERTR, UPDATER, DELETER FROM ASL_ROLE WHERE  CONTROLLERNAME='CostPool' AND COMPID='{0}' AND USERID = '{1}'", LoggedCompId, loggedUserID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            foreach (DataRow row in ds.Rows)
            {
                deleteStatus = row["DELETER"].ToString();
            }
            conn.Close();
            //add the data from database to model
            var costcid = from m in db.GLCostPMSTDbSet where m.COSTCID == Costcid && m.COMPID == cid select m;
            foreach (var categoryResult in costcid)
            {
                model.AGlCostPMST.COMPID = cid;
                model.AGlCostPMST.COSTCID = Costcid;
                model.AGlCostPMST.COSTCNM = categoryResult.COSTCNM;

            }


            TempData["costpool"] = model;
            TempData["CostCID"] = Costcid;

            TempData["ShowAddButton"] = "Show Add Button";
            if (deleteStatus == 'I'.ToString())
            {
                TempData["message"] = "Permission not granted!";
                return RedirectToAction("Index");

            }
            //...............................................................................................

            GL_COSTPOOL Accountitem = db.GlCostPoolDbSet.Find(id);
            //Get Ip ADDRESS,Time & user PC Name
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Accountitem.USERPC = strHostName;
            Accountitem.UPDIPNO = ipAddress.ToString();
            Accountitem.UPDTIME = Convert.ToDateTime(td);
            //Delete User ID save POS_ITEMMST table attribute (UPDUSERID) field
            Accountitem.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            if (TempData["latitute_deleteAccount"] != null)
            {
                //Get current LOGLTUDE data 
                Accountitem.UPDLTUDE = TempData["latitute_deleteAccount"].ToString();
            }

            Delete_GL_CostPool_LogData(Accountitem);
            Delete_ASL_Delete_GL_CostPool_LogData(Accountitem);
            db.GlCostPoolDbSet.Remove(Accountitem);
            db.SaveChanges();

            return RedirectToAction("Index");
        }




        ////AutoComplete 
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult ItemNameChanged(string changedText)
        //{
        //    var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
        //    String itemId = "";

        //    var rt = db.GLCostPMSTDbSet.Where(n => n.COSTCNM == changedText &&
        //                                                 n.COMPID == compid).Select(n => new
        //                                                 {
        //                                                     costcid = n.COSTCID,

        //                                                 });
        //    foreach (var n in rt)
        //    {
        //        itemId = Convert.ToString(n.costcid);

        //    }

        //    var result = new { costcid = itemId };
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}





        //AutoComplete
        public JsonResult TagSearch(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());





            var tags = from p in db.GLCostPMSTDbSet
                       where p.COMPID == compid
                       select p.COSTCNM;


            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);



        }
        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult keyword(string changedText)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            String itemId = "";

            var rt = db.GLCostPMSTDbSet.Where(n => n.COSTCNM.StartsWith(changedText) &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costcnm = n.COSTCNM

                                                         }).ToList();
            int lenChangedtxt = changedText.Length;
           
            Int64 cont = rt.Count();
            Int64 k = 0;
            string status = "";
            if(changedText!="" && cont!=0)
            {
                while (status != "no")
                {
                     k = 0;
                    foreach (var n in rt)
                    {
                        string ss = Convert.ToString(n.costcnm);
                        string subss = "";
                        if (ss.Length >= lenChangedtxt)
                        {
                            subss = ss.Substring(0, lenChangedtxt);
                            subss=subss.ToUpper();
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
               if(lenChangedtxt==1)
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
            



            //for item changed.......
            String itemId2 = "";

            var rt2 = db.GLCostPMSTDbSet.Where(n => n.COSTCNM == itemId &&
                                                         n.COMPID == compid).Select(n => new
                                                         {
                                                             costcid = n.COSTCID,

                                                         });
            foreach (var n in rt2)
            {
                itemId2 = Convert.ToString(n.costcid);

            }

           // var result = new { costcid = itemId };

            var result = new { Costcnm = itemId, costcid = itemId2};
            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}
