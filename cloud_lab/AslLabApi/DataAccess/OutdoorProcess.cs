using AslLabApi.Models;
using AslLabApi.Models.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AslLabApi.DataAccess
{
    public class OutdoorProcess
    {

         public static string Outdoor(DateTime TransDate)
        {
            AslLabApiContext db = new AslLabApiContext();
            IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime td;



            //Get Ip ADDRESS,Time & user PC Name
            string strHostName;
            IPHostEntry ipHostInfo;
            IPAddress ipAddress;
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);

            var checkdata = (from n in db.HMS_OPDMST where n.TRANSDT == TransDate && n.COMPID == compid select n).OrderBy(x => x.TRANSNO).ToList();


            string converttoString = Convert.ToString(TransDate.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3);
            string Month = getMonth + "-" + getYear;

            if (checkdata.Count != 0)
            {
                var forremovedata = (from l in db.GlMasterDbSet
                                     where l.COMPID == compid && l.TRANSDT == TransDate && l.TABLEID == "HMS_OPDMST"
                                     select l).ToList();

                foreach (var VARIABLE in forremovedata)
                {

                    db.GlMasterDbSet.Remove(VARIABLE);

                }



                db.SaveChanges();
              


                foreach(var x in checkdata)
                {
                    if(x.TOTAMT>0)
                    {
                        Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                            where a.COMPID == compid && a.TRANSTP == "JOUR" && a.TABLEID == "HMS_OPDMST"
                                                            select a.TRANSSL).Max());

                        GL_MASTER model = new GL_MASTER();
                        Int64 ACCOUNTSRECEIVABLEOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "1030001");
                        Int64 INCOMEFROMOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "3010001");

                         if (maxSlCheck == 0)
                         {
                             model.TRANSSL = 71001;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = INCOMEFROMOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "OUTDOOR RECEIVABLE" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDMST";


                             model.DEBITAMT = x.TOTAMT;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = 61002;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = INCOMEFROMOUTDOOR;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "OUTDOOR RECEIVABLE" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDMST";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.TOTAMT;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }
                         else
                         {
                             model.TRANSSL = maxSlCheck+1;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = INCOMEFROMOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "OUTDOOR RECEIVABLE" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDMST";


                             model.DEBITAMT = x.TOTAMT;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = maxSlCheck+2;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = INCOMEFROMOUTDOOR;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "OUTDOOR RECEIVABLE" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDMST";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.TOTAMT;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }




                    }
                    if (x.DISCNET > 0)
                    {
                        Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                            where a.COMPID == compid && a.TRANSTP == "JOUR" && a.TABLEID == "HMS_OPDMST"
                                                            select a.TRANSSL).Max());

                        GL_MASTER model = new GL_MASTER();
                        Int64 ACCOUNTSRECEIVABLEOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "1030001");
                        Int64 DISCOUNT = Convert.ToInt64(Convert.ToString(compid) + "4010002");

                        if (maxSlCheck == 0)
                        {
                            model.TRANSSL = 72001;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "JOUR";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = DISCOUNT;
                            model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);
                            model.TRANSDRCR = "DEBIT";
                            model.TABLEID = "HMS_OPDMST";


                            model.DEBITAMT = x.DISCNET;
                            model.CREDITAMT = 0;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;


                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model.TRANSSL = 72002;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "JOUR";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CREDITCD = DISCOUNT;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);

                            model.TRANSDRCR = "CREDIT";
                            model.TABLEID = "HMS_OPDMST";

                            model.DEBITAMT = 0;
                            model.CREDITAMT = x.DISCNET;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;

                            db.GlMasterDbSet.Add(model);

                            //Insert_Process_LogData(model);
                            db.SaveChanges();
                        }
                        else
                        {
                            model.TRANSSL = maxSlCheck + 1;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "JOUR";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = DISCOUNT;
                            model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);
                            model.TRANSDRCR = "DEBIT";
                            model.TABLEID = "HMS_OPDMST";


                            model.DEBITAMT = x.DISCNET;
                            model.CREDITAMT = 0;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;


                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model.TRANSSL = maxSlCheck + 2;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "JOUR";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CREDITCD = DISCOUNT;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);

                            model.TRANSDRCR = "CREDIT";
                            model.TABLEID = "HMS_OPDMST";

                            model.DEBITAMT = 0;
                            model.CREDITAMT = x.DISCNET;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;

                            db.GlMasterDbSet.Add(model);

                            //Insert_Process_LogData(model);
                            db.SaveChanges();
                        }




                    }
                    if (x.RCVAMT > 0)
                    {
                        Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                            where a.COMPID == compid && a.TRANSTP == "MREC" && a.TABLEID == "HMS_OPDMST"
                                                            select a.TRANSSL).Max());

                        GL_MASTER model = new GL_MASTER();
                        Int64 ACCOUNTSRECEIVABLEOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "1030001");
                        Int64 CASHINHAND = Convert.ToInt64(Convert.ToString(compid) + "1010001");

                        if (maxSlCheck == 0)
                        {
                            model.TRANSSL = 72001;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "MREC";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = CASHINHAND;
                            model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED - OUTDOOR" + "-" + x.REMARKS);
                            model.TRANSDRCR = "DEBIT";
                            model.TABLEID = "HMS_OPDMST";


                            model.DEBITAMT = x.RCVAMT;
                            model.CREDITAMT = 0;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;


                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model.TRANSSL = 72002;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "MREC";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CREDITCD = CASHINHAND;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED - OUTDOOR" + "-" + x.REMARKS);

                            model.TRANSDRCR = "CREDIT";
                            model.TABLEID = "HMS_OPDMST";

                            model.DEBITAMT = 0;
                            model.CREDITAMT = x.RCVAMT;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;

                            db.GlMasterDbSet.Add(model);

                            //Insert_Process_LogData(model);
                            db.SaveChanges();
                        }
                        else
                        {
                            model.TRANSSL = maxSlCheck + 1;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "MREC";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = CASHINHAND;
                            model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED - OUTDOOR" + "-" + x.REMARKS);
                            model.TRANSDRCR = "DEBIT";
                            model.TABLEID = "HMS_OPDMST";


                            model.DEBITAMT = x.RCVAMT;
                            model.CREDITAMT = 0;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;


                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model.TRANSSL = maxSlCheck + 2;

                            model.TRANSDT = x.TRANSDT;
                            model.COMPID = x.COMPID;
                            model.TRANSTP = "MREC";
                            model.TRANSMY = Month;
                            model.TRANSNO = x.TRANSNO;
                            model.TRANSFOR = "";
                            model.TRANSMODE = "";
                            model.COSTPID = null;
                            model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                            model.CREDITCD = CASHINHAND;
                            model.CHEQUENO = null;
                            model.CHEQUEDT = null;
                            model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED - OUTDOOR" + "-" + x.REMARKS);

                            model.TRANSDRCR = "CREDIT";
                            model.TABLEID = "HMS_OPDMST";

                            model.DEBITAMT = 0;
                            model.CREDITAMT = x.RCVAMT;

                            model.USERPC = strHostName;
                            model.INSIPNO = ipAddress.ToString();
                            model.INSTIME = Convert.ToDateTime(td);

                            model.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.INSLTUDE = model.INSLTUDE;

                            db.GlMasterDbSet.Add(model);

                            //Insert_Process_LogData(model);
                            db.SaveChanges();
                        }




                    }
                }
            }
            else
            {
                var forremovedata = (from l in db.GlMasterDbSet
                                     where l.COMPID == compid && l.TRANSDT == TransDate && l.TABLEID == "HMS_OPDMST"
                                     select l).ToList();

                foreach (var VARIABLE in forremovedata)
                {

                    db.GlMasterDbSet.Remove(VARIABLE);

                }



                db.SaveChanges();
            }


            return "OK";
        }


         public static string OutdoorDue(DateTime TransDate)
         {
             AslLabApiContext db = new AslLabApiContext();
             IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
             TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
             DateTime td;



             //Get Ip ADDRESS,Time & user PC Name
             string strHostName;
             IPHostEntry ipHostInfo;
             IPAddress ipAddress;
             strHostName = System.Net.Dns.GetHostName();
             ipHostInfo = Dns.Resolve(Dns.GetHostName());
             ipAddress = ipHostInfo.AddressList[0];

             td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
             Int64 compid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
             var checkdata = (from n in db.HMS_OPDRCV where n.TRANSDT == TransDate && n.COMPID == compid select n).OrderBy(x => x.TRANSNO).ToList();


             string converttoString = Convert.ToString(TransDate.ToString("dd-MMM-yyyy"));
             string getYear = converttoString.Substring(9, 2);
             string getMonth = converttoString.Substring(3, 3);
             string Month = getMonth + "-" + getYear;

             if (checkdata.Count != 0)
             {

                 var forremovedata = (from l in db.GlMasterDbSet
                                      where l.COMPID == compid && l.TRANSDT == TransDate && l.TABLEID == "HMS_OPDRCV"
                                      select l).ToList();

                 foreach (var VARIABLE in forremovedata)
                 {

                     db.GlMasterDbSet.Remove(VARIABLE);

                 }



                 db.SaveChanges();


                 foreach (var x in checkdata)
                 {
                     if (x.DISCHOS > 0)
                     {
                         Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                             where a.COMPID == compid && a.TRANSTP == "JOUR" && a.TABLEID == "HMS_OPDRCV"
                                                             select a.TRANSSL).Max());

                         GL_MASTER model = new GL_MASTER();
                         Int64 ACCOUNTSRECEIVABLEOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "1030001");
                         Int64 DISCOUNT = Convert.ToInt64(Convert.ToString(compid) + "4010002");

                         if (maxSlCheck == 0)
                         {
                             model.TRANSSL = 74001;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = DISCOUNT;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDRCV";


                             model.DEBITAMT = x.DISCHOS;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = 74002;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = DISCOUNT;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDRCV";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.DISCHOS;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }
                         else
                         {
                             model.TRANSSL = maxSlCheck + 1;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = DISCOUNT;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDRCV";


                             model.DEBITAMT = x.DISCHOS;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = maxSlCheck + 2;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "JOUR";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = DISCOUNT;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "DISCOUNT" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDRCV";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.DISCHOS;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }




                     }
                     if (x.RCVAMT > 0)
                     {
                         Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                             where a.COMPID == compid && a.TRANSTP == "MREC" && a.TABLEID == "HMS_OPDRCV"
                                                             select a.TRANSSL).Max());

                         GL_MASTER model = new GL_MASTER();
                         Int64 ACCOUNTSRECEIVABLEOUTDOOR = Convert.ToInt64(Convert.ToString(compid) + "1030001");
                         Int64 CASHINHAND = Convert.ToInt64(Convert.ToString(compid) + "1010001");

                         if (maxSlCheck == 0)
                         {
                             model.TRANSSL = 75001;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "MREC";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = CASHINHAND;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED OUTDOOR - DUE" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDRCV";


                             model.DEBITAMT = x.RCVAMT;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = 75002;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "MREC";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = CASHINHAND;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED OUTDOOR - DUE" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDRCV";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.RCVAMT;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }
                         else
                         {
                             model.TRANSSL = maxSlCheck + 1;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "MREC";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = CASHINHAND;
                             model.CREDITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED OUTDOOR - DUE" + "-" + x.REMARKS);
                             model.TRANSDRCR = "DEBIT";
                             model.TABLEID = "HMS_OPDRCV";


                             model.DEBITAMT = x.RCVAMT;
                             model.CREDITAMT = 0;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;


                             db.GlMasterDbSet.Add(model);
                             db.SaveChanges();

                             model.TRANSSL = maxSlCheck + 2;

                             model.TRANSDT = x.TRANSDT;
                             model.COMPID = x.COMPID;
                             model.TRANSTP = "MREC";
                             model.TRANSMY = Month;
                             model.TRANSNO = x.TRANSNO;
                             model.TRANSFOR = "";
                             model.TRANSMODE = "";
                             model.COSTPID = null;
                             model.DEBITCD = ACCOUNTSRECEIVABLEOUTDOOR;
                             model.CREDITCD = CASHINHAND;
                             model.CHEQUENO = null;
                             model.CHEQUEDT = null;
                             model.REMARKS = Convert.ToString("OUTDOOR" + "-" + x.PATIENTID + "-" + "RECEIVED OUTDOOR - DUE" + "-" + x.REMARKS);

                             model.TRANSDRCR = "CREDIT";
                             model.TABLEID = "HMS_OPDRCV";

                             model.DEBITAMT = 0;
                             model.CREDITAMT = x.RCVAMT;

                             model.USERPC = strHostName;
                             model.INSIPNO = ipAddress.ToString();
                             model.INSTIME = Convert.ToDateTime(td);

                             model.INSUSERID =
                                 Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                             model.INSLTUDE = model.INSLTUDE;

                             db.GlMasterDbSet.Add(model);

                             //Insert_Process_LogData(model);
                             db.SaveChanges();
                         }




                     }
                 }
             }

             else
             {
                 var forremovedata = (from l in db.GlMasterDbSet
                                      where l.COMPID == compid && l.TRANSDT == TransDate && l.TABLEID == "HMS_OPDRCV"
                                      select l).ToList();

                 foreach (var VARIABLE in forremovedata)
                 {

                     db.GlMasterDbSet.Remove(VARIABLE);

                 }



                 db.SaveChanges();
             }



             return "OK";
         }


    }
}