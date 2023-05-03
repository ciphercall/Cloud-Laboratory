using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;
using AslLabApi.Models.ASL;

namespace AslLabApi.Controllers.HM
{
    public class OpdController : AppController
    {
        AslLabApiContext db = new AslLabApiContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public OpdController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_OpdMasterLogData(OpdDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.CompanyID && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.CompanyID);
            aslLog.USERID = Convert.ToInt64(model.INSUSERID);
            aslLog.LOGTYPE = "Insert";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.INSIPNO;
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.PatientID + ",\nPatient Name: " + model.PatientName + ",\nTransaction Date: " + model.TransactionDate + ",\nGender: " + model.Gender +
                ",\nAge: " + model.Age + ",\nMobile No: " + model.MObileNo + ",\nAddress: " + model.Address + ",\nRF(%): " + model.RfPercentage +
                ",\nDelivery Date: " + model.DvDate + ",\nDelivery Time: " + model.Dvtm + ",\nTotal AMount: " + model.TotalAmount +
                ",\nDoctor Name: " + model.DoctorName + ",\nRefer Name: " + model.Refername +
                ",\nDiscount Refer: " + model.DiscountRefer + ",\nDiscount Lab: " + model.DiscountHos + ",\nDiscount Net: " + model.Discountnet + ",\nNet Amount :" + model.NetAmount +
                ",\nPaid: " + model.ReceiveAmount + ",\nDue Amount: " + model.DueAmount);
            aslLog.TABLEID = "HMS_OPDMST";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }



        // GET: /Opd/
        public ActionResult Index()
        {
            return View();
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(OpdDTO model, string command)
        {
            if (command != "Complete")
            {
                TempData["GetDepartmentWiseReport"] = model;

                return RedirectToAction("DepartmentWiseReport");
            }

            else
            {
                var selectdata =
                    (from n in db.HMS_OPDMST
                     where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID
                     select n).ToList();
              

                foreach (var opdMaster in selectdata)
                {
                    opdMaster.COMPID = opdMaster.COMPID;
                    opdMaster.PATIENTID = opdMaster.PATIENTID;
                    opdMaster.PATIENTNM = opdMaster.PATIENTNM;
                    opdMaster.TRANSDT = opdMaster.TRANSDT;
                    opdMaster.TRANSMY = opdMaster.TRANSMY;
                    opdMaster.TRANSNO = opdMaster.TRANSNO;
                    opdMaster.RFPCNT = opdMaster.RFPCNT;
                    opdMaster.USERPC = opdMaster.USERPC;
                    opdMaster.INSUSERID = opdMaster.INSUSERID;
                    opdMaster.INSTIME = opdMaster.INSTIME;
                    opdMaster.INSIPNO = opdMaster.INSIPNO;
                    opdMaster.INSLTUDE = opdMaster.INSLTUDE;

                    opdMaster.GENDER = model.Gender;
                    opdMaster.AGE = model.Age;
                    opdMaster.MOBNO = model.MObileNo;
                    opdMaster.ADDRESS = model.Address;
                    opdMaster.DOCTORID = model.DoctorID;
                    opdMaster.REFERID = model.ReferID;
                    opdMaster.DVDT = model.DvDate;
                    opdMaster.DVTM = model.Dvtm;

                    opdMaster.TOTAMT = model.TotalAmount;




                    opdMaster.DISCREF = model.DiscountRefer;


                    opdMaster.DISCNET = model.Discountnet;



                    opdMaster.NETAMT = model.NetAmount;



                    opdMaster.DUEAMT = model.DueAmount;
                    opdMaster.RCVAMT = model.ReceiveAmount;

                    model.USERPC = opdMaster.USERPC;
                    model.INSIPNO = opdMaster.INSIPNO;

                    Insert_OpdMasterLogData(model);

                    ASL_PSMS obj = new ASL_PSMS();
                    obj.COMPID = opdMaster.COMPID;
                    obj.TRANSYY = Convert.ToInt64(Convert.ToString(opdMaster.TRANSNO).Substring(0,3));
                    obj.TRANSNO = Convert.ToInt64(opdMaster.TRANSNO);
                    obj.TRANSDT = opdMaster.TRANSDT;
                    obj.MOBNO = opdMaster.MOBNO;
                    obj.LANGUAGE = "ENGLISH";
                    obj.MSGTP = "WELCOME";
                    obj.MESSAGE = "WELCOME";
                    obj.STATUS = "PENDING";
                    obj.SENTTM = null;
                    obj.USERPC = opdMaster.USERPC;
                    obj.INSIPNO = opdMaster.INSIPNO;
                    obj.INSLTUDE = opdMaster.INSLTUDE;
                    obj.INSTIME = opdMaster.INSTIME;
                    obj.INSUSERID = Convert.ToInt64(opdMaster.INSUSERID);

                    db.SendLogSMSDbSet.Add(obj);




                    db.SaveChanges();

                }
                var selectdata_from_opd =
                    (from n in db.HMS_OPD where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n)
                        .ToList();
                foreach (var opd in selectdata_from_opd)
                {
                    opd.COMPID = opd.COMPID;
                    opd.PATIENTID = opd.PATIENTID;

                    opd.TRANSDT = opd.TRANSDT;

                    opd.USERPC = opd.USERPC;
                    opd.INSUSERID = opd.INSUSERID;
                    opd.INSTIME = opd.INSTIME;
                    opd.INSIPNO = opd.INSIPNO;
                    opd.INSLTUDE = opd.INSLTUDE;


                    opd.REFERID = model.ReferID;
                    opd.TESTSL = opd.TESTSL;
                    opd.TCATID = opd.TCATID;
                    opd.TESTID = opd.TESTID;
                    opd.AMOUNT = opd.AMOUNT;
                    opd.PCNTR = opd.PCNTR;
                    opd.PCNTD = opd.PCNTD;
                    opd.DISCR = opd.DISCR;
                    opd.REMARKS = opd.REMARKS;

                    // Insert_OpdLogData(model);

                    db.SaveChanges();

                }

                return RedirectToAction("Index");
            }

            //return RedirectToAction("Index");
        }
        public ActionResult GetOpdBillReport()
        {
            OpdDTO model = (OpdDTO)TempData["GetOpdBillReport"];
            return View(model);
        }


        public ActionResult ForDepartment(string Compid, string TransactionDate, string PatienId, string patientname, string Gender,
           string Age, string Address, string MObileNo, string ReferID, string refername, string doctorid, string doctorname, decimal discount, decimal netamount, decimal paid, decimal due, Int64 forprintorPreview)
        {
            OpdDTO model = new OpdDTO();
            model.CompanyID = Convert.ToInt64(Compid);
            model.TransactionDate = Convert.ToDateTime(TransactionDate);
            model.PatientID = PatienId;
            model.PatientName = patientname;
            model.Gender = Gender;
            model.Age = Age;
            model.Address = Address;
            model.MObileNo = MObileNo;
            if (ReferID == "")
            {
                ReferID = "0";
            }
            model.ReferID = Convert.ToInt64(ReferID);
            model.DoctorName = doctorname;
            model.Refername = refername;
            model.Discountnet = discount;
            model.NetAmount = netamount;
            model.ReceiveAmount = paid;
            model.DueAmount = due;
         
            model.ForPrintOrPreview = forprintorPreview;

            var search_deliveryInfo = (from n in db.HMS_OPDMST where n.COMPID == model.CompanyID && n.PATIENTID == PatienId select n).ToList();
            
            foreach(var x in search_deliveryInfo)
            {
                model.DvDate = x.DVDT;
                model.Dvtm = x.DVTM;
            }


            TempData["GetOpdBillReport"] = model;
            return RedirectToAction("GetOpdBillReport");
        }

        public ActionResult DepartmentWiseReport()
        {
            OpdDTO model = (OpdDTO)TempData["GetDepartmentWiseReport"];
            return View(model);
        }




        //-----------------Update Part-------------------------//
        public ActionResult Update()
        {
            return View();
        }


        public void Update_OpdMasterLogData(OpdDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.CompanyID && n.USERID == model.UPDUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.CompanyID);
            aslLog.USERID = Convert.ToInt64(model.UPDUSERID);
            aslLog.LOGTYPE = "Update";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = model.UPDIPNO;
            aslLog.LOGLTUDE = model.UPDLTUDE;
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.PatientID + ",\nPatient Name: " + model.PatientName + ",\nTransaction Date: " + model.TransactionDate + ",\nGender: " + model.Gender +
                ",\nAge: " + model.Age + ",\nMobile No: " + model.MObileNo + ",\nAddress: " + model.Address + ",\nRF(%): " + model.RfPercentage +
                ",\nDelivery Date: " + model.DvDate + ",\nDelivery Time: " + model.Dvtm + ",\nTotal AMount: " + model.TotalAmount +
                ",\nDoctor Name: " + model.DoctorName + ",\nRefer Name: " + model.Refername +
                ",\nDiscount Refer: " + model.DiscountRefer + ",\nDiscount Lab: " + model.DiscountHos + ",\nDiscount Net: " + model.Discountnet + ",\nNet Amount :" + model.NetAmount +
                ",\nPaid: " + model.ReceiveAmount + ",\nDue Amount: " + model.DueAmount);
            aslLog.TABLEID = "HMS_OPDMST";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [AcceptVerbs("POST")]
        [ActionName("Update")]
        public ActionResult UpdatePost(OpdDTO model, string command)
        {
            if (command != "Update")
            {
                TempData["GetDepartmentWiseReport"] = model;

                return RedirectToAction("DepartmentWiseReport");
            }

            else
            {
                var selectdata =
                    (from n in db.HMS_OPDMST
                     where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID
                     select n).ToList();


                foreach (var opdMaster in selectdata)
                {
                    opdMaster.COMPID = opdMaster.COMPID;
                    opdMaster.PATIENTID = opdMaster.PATIENTID;
                    opdMaster.PATIENTNM = opdMaster.PATIENTNM;
                    opdMaster.TRANSDT = opdMaster.TRANSDT;
                    opdMaster.TRANSMY = opdMaster.TRANSMY;
                    opdMaster.TRANSNO = opdMaster.TRANSNO;
                    opdMaster.RFPCNT = model.RfPercentage;
                    opdMaster.USERPC = opdMaster.USERPC;
                    opdMaster.INSUSERID = opdMaster.INSUSERID;
                    opdMaster.INSTIME = opdMaster.INSTIME;
                    opdMaster.INSIPNO = opdMaster.INSIPNO;
                    opdMaster.INSLTUDE = opdMaster.INSLTUDE;

                    opdMaster.UPDIPNO = ipAddress.ToString();
                    opdMaster.UPDLTUDE = model.INSLTUDE;
                    opdMaster.UPDTIME = Convert.ToDateTime(td);
                    opdMaster.UPDUSERID = model.UPDUSERID;

                    opdMaster.GENDER = model.Gender;
                    opdMaster.AGE = model.Age;
                    opdMaster.MOBNO = model.MObileNo;
                    opdMaster.ADDRESS = model.Address;
                    opdMaster.DOCTORID = model.DoctorID;
                    opdMaster.REFERID = model.ReferID;
                    opdMaster.DVDT = model.DvDate;
                    opdMaster.DVTM = model.Dvtm;
                    opdMaster.TOTAMT = model.TotalAmount;
                    opdMaster.DISCREF = model.DiscountRefer;
                    opdMaster.DISCNET = model.Discountnet;
                    opdMaster.NETAMT = model.NetAmount;
                    opdMaster.DUEAMT = model.DueAmount;
                    opdMaster.RCVAMT = model.ReceiveAmount;

                    model.USERPC = opdMaster.USERPC;
                    model.UPDIPNO = ipAddress.ToString();
                    model.UPDLTUDE = model.INSLTUDE;
                    model.UPDTIME = Convert.ToDateTime(td);

                    Update_OpdMasterLogData(model);
                    db.SaveChanges();

                }
                var selectdata_from_opd =
                    (from n in db.HMS_OPD where n.COMPID == model.CompanyID && n.PATIENTID == model.PatientID select n)
                        .ToList();
                foreach (var opd in selectdata_from_opd)
                {

                    opd.COMPID = opd.COMPID;
                    opd.PATIENTID = opd.PATIENTID;

                    opd.TRANSDT = opd.TRANSDT;


                    opd.USERPC = opd.USERPC;
                    opd.INSUSERID = opd.INSUSERID;
                    opd.INSTIME = opd.INSTIME;
                    opd.INSIPNO = opd.INSIPNO;
                    opd.INSLTUDE = opd.INSLTUDE;

                    opd.UPDIPNO = ipAddress.ToString();
                    opd.UPDLTUDE = model.INSLTUDE;
                    opd.UPDTIME = Convert.ToDateTime(td);
                    opd.UPDUSERID = model.UPDUSERID;

                    opd.REFERID = model.ReferID;
                    opd.TESTSL = opd.TESTSL;
                    opd.TCATID = opd.TCATID;
                    opd.TESTID = opd.TESTID;

                    opd.AMOUNT = opd.AMOUNT;
                    
                   
                    opd.PCNTR = model.RfPercentage;

                    opd.PCNTD = opd.PCNTD;

                    if (model.RfPercentage < opd.PCNTD)
                    {
                        opd.DISCR = opd.AMOUNT * (model.RfPercentage * Convert.ToDecimal(.01));
                    }
                    else
                    {
                        opd.DISCR = opd.AMOUNT * (opd.PCNTD * Convert.ToDecimal(.01));
                    }
                    opd.REMARKS = opd.REMARKS;

                    // Insert_OpdLogData(model);

                    db.SaveChanges();

                }

                return RedirectToAction("Update");
            }

            //return RedirectToAction("Index");
        }

    }
}