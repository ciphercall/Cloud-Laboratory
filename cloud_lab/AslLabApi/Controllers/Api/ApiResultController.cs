using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.Api
{
    public class ApiResultController : ApiController
    {

        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiResultController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        AslLabApiContext db = new AslLabApiContext();

        [System.Web.Http.Route("api/Result/PatientId")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ResultDTO> GetPatientId(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);

                var returndata = (from t1 in db.HMS_OPDMST

                                  where t1.COMPID == compid && t1.PATIENTID.StartsWith(query)
                                  select new
                                  {

                                      patientid = t1.PATIENTID,
                                      patientname = t1.PATIENTNM,
                                      transdt = t1.TRANSDT,
                                      mobileno = t1.MOBNO

                                  });
                foreach (var item in returndata)
                {
                    DateTime dd = Convert.ToDateTime(item.transdt);
                    string aa = dd.ToString("dd-MMM-yyyy");
                    yield return new ResultDTO
                    {

                        PatientId = item.patientid,
                        PatientName = item.patientname,

                        TransactionDate = aa,
                        MobileNo = item.mobileno
                    };
                }







            }
        }

        [System.Web.Http.Route("api/Result/DynamicPatientName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ResultDTO> autocompleteForResult(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                //DateTime TDate = Convert.ToDateTime(changedText3);

                String name = "";

                var rt = db.HMS_OPDMST.Where(n => n.PATIENTID.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                {
                    headname = n.PATIENTID

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



                var findid = (from n in db.HMS_OPDMST where n.PATIENTID == name && n.COMPID == compid select n).ToList();
                if (findid.Count != 0)
                {


                    var returndata = (from t1 in db.HMS_OPDMST
                                      //join t2 in db.HMS_OPD on t1.COMPID equals t2.COMPID
                                      where t1.COMPID == compid && t1.PATIENTID == name
                                      select new
                                      {

                                          patientid = t1.PATIENTID,
                                          patientname = t1.PATIENTNM,
                                          transdt = t1.TRANSDT,
                                          mobileno = t1.MOBNO


                                      });
                    foreach (var item in returndata)
                    {
                        DateTime dd = Convert.ToDateTime(item.transdt);
                        string aa = dd.ToString("dd-MMM-yyyy");
                        yield return new ResultDTO
                        {

                            PatientId = item.patientid,
                            PatientName = item.patientname,

                            TransactionDate = aa,
                            MobileNo = item.mobileno
                        };
                    }
                    //return db.HMS_OPDMST.Where(p => p.PATIENTID == name && p.COMPID == compid).Select(
                    //    x =>
                    //        new
                    //        {
                    //            patientname = x.PATIENTNM,
                    //            patientid = x.PATIENTID



                    //        })
                    //    .AsEnumerable().Select(a => new ResultDTO
                    //    {

                    //        PatientName = a.patientname,
                    //        PatientId = a.patientid




                    //    }).ToList();
                }
                else
                {
                    //return db.HMS_OPDMST.AsEnumerable().Select(a => new ResultDTO
                    //{
                    //    PatientId = name,
                    //    PatientName = ""



                    //}).ToList();
                    yield return new ResultDTO
                    {

                        PatientId = name,
                        PatientName = ""


                    };
                }



            }

        }


        [System.Web.Http.Route("api/ApiResult/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ResultDTO> Get(string Compid, string Transdt, string Patientid, string Testid, string Insuserid, string Insltude)
        {

            Int64 compid = Convert.ToInt64(Compid);

            DateTime TransDate = DateTime.Parse(Transdt);
            Int64 TestID = Convert.ToInt64(Testid);



            var ache_kina_data = (from n in db.HMS_RESULT where n.COMPID == compid && n.PATIENTID == Patientid && n.TRANSDT == TransDate && n.TESTID == TestID select n).ToList();
            if (ache_kina_data.Count == 0)
            {


                var find_restid_fromTestnv =
                    (from n in db.HMS_TESTNV where n.COMPID == compid && n.TESTID == TestID select n).ToList();

                Result obj = new Result();

                foreach (var testNv in find_restid_fromTestnv)
                {


                    obj.COMPID = compid;
                    obj.TRANSDT = TransDate;
                    obj.PATIENTID = Patientid;
                    obj.TESTID = TestID;
                    obj.RESTID = testNv.RESTID;
                    obj.RESULT = testNv.DVALUE;
                    obj.USERPC = strHostName;
                    obj.INSIPNO = ipAddress.ToString();
                    obj.INSTIME = Convert.ToDateTime(td);

                    obj.INSUSERID = Convert.ToInt64(Insuserid);

                    obj.INSLTUDE = Insltude;


                    db.HMS_RESULT.Add(obj);
                    db.SaveChanges();
                }


                var restdto = (from t1 in db.HMS_RESULT
                               join t2 in db.HMS_TESTNV on t1.RESTID equals t2.RESTID
                               where t1.COMPID == compid && t1.PATIENTID == Patientid && t1.TESTID == TestID && t1.TRANSDT == TransDate
                               select new
                               {
                                   Id = t1.ID,
                                   companyid = t1.COMPID,
                                   patientid = t1.PATIENTID,
                                   testid = t1.TESTID,
                                   restid = t1.RESTID,
                                   restname = t2.RESTNM,
                                   result = t1.RESULT


                               });
                foreach (var item in restdto)
                {
                    yield return new ResultDTO
                    {
                        ID = item.Id,
                        COMPID = item.companyid,
                        PatientId = item.patientid,
                        TestId = item.testid,
                        RestId = item.restid,
                        Restname = item.restname,
                        Result = item.result
                    };
                }






            }
            else
            {

                var restdto = (from t1 in db.HMS_RESULT
                               join t2 in db.HMS_TESTNV on t1.RESTID equals t2.RESTID
                               where t1.COMPID == compid && t1.PATIENTID == Patientid && t1.TESTID == TestID && t1.TRANSDT == TransDate
                               select new
                               {
                                   Id = t1.ID,
                                   companyid = t1.COMPID,
                                   patientid = t1.PATIENTID,
                                   testid = t1.TESTID,
                                   restid = t1.RESTID,
                                   restname = t2.RESTNM,
                                   result = t1.RESULT


                               });
                foreach (var item in restdto)
                {
                    yield return new ResultDTO
                    {
                        ID = item.Id,
                        COMPID = item.companyid,
                        PatientId = item.patientid,
                        TestId = item.testid,
                        RestId = item.restid,
                        Restname = item.restname,
                        Result = item.result
                    };
                }




            }

        }

        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_ResultLogData(ResultDTO model)
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

            var testname_find =
                (from n in db.HMS_TEST where n.COMPID == model.COMPID && n.TESTID == model.TestId select n).ToList();

            string testname = "",testParticular="";
            foreach (var testMaster in testname_find)
            {
                testname = testMaster.TESTNM;
            }

            var testParticular_find =
                (from n in db.HMS_TESTNV where n.COMPID == model.COMPID && n.RESTID == model.RestId select n).ToList();
            foreach (var test in testParticular_find)
            {
                testParticular = test.RESTNM;
            }
            aslLog.LOGDATA = Convert.ToString("Transaction Date: " + model.TransactionDate +",\nPatient Id: " + model.PatientId + ",\nTest Name : "
                + testname + ",\nTest Particulars: " + testParticular +
                ",\nResult: " + model.Result);
            aslLog.TABLEID = "HMS_RESULT";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }

        [System.Web.Http.Route("api/ApiResult/Child")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage TestChild(ResultDTO model)       //This is not used in Result Form........
        {
            DateTime date = Convert.ToDateTime(model.TransactionDate);
            var check_data = (from n in db.HMS_RESULT where n.COMPID == model.COMPID && n.TESTID == model.TestId && n.TRANSDT == date && n.PATIENTID == model.PatientId && n.RESTID == model.RestId select n).ToList();
            if (check_data.Count == 0)
            {





                Result testData = new Result();

                testData.COMPID = model.COMPID;
                testData.TESTID = model.TestId;
                testData.PATIENTID = model.PatientId;
                testData.TRANSDT = date;
                testData.RESTID = model.RestId;

                testData.RESULT = model.Result;

                testData.USERPC = strHostName;
                testData.INSIPNO = ipAddress.ToString();
                testData.INSTIME = Convert.ToDateTime(td);

                testData.INSUSERID = model.INSUSERID;

                testData.INSLTUDE = model.INSLTUDE;


                db.HMS_RESULT.Add(testData);

               

                model.ID = testData.ID;
                model.COMPID = model.COMPID;
                model.TestId = model.TestId;
                model.PatientId = model.PatientId;
                model.TransactionDate = model.TransactionDate;
                model.RestId = model.RestId;
                model.Result = model.Result;

                model.USERPC = testData.USERPC;
                model.INSIPNO = testData.INSIPNO;
                model.INSTIME = testData.INSTIME;

                Insert_ResultLogData(model);
                db.SaveChanges();



                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.TestId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }

        [System.Web.Http.Route("api/ApiResult/DeleteData")]
        [System.Web.Http.HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(ResultDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_RESULT WHERE ID='{0}'", model.ID);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            conn.Close();
            Result testObj = new Result();






            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }


        public void Update_ResultLogData(ResultDTO model)
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
            aslLog.LOGDATA = Convert.ToString("Transaction Date: " + model.TransactionDate +
               ",\nPatient Id: " + model.PatientId + ",\nTest ID: " + model.TestId + ",\nRest ID: " + model.RestId +
                ",\nResult: " + model.Result);
            aslLog.TABLEID = "HMS_RESULT";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }



        [System.Web.Http.Route("api/ApiResult/SaveData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveData(ResultDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_RESULT where n.COMPID == model.COMPID && n.TESTID == model.TestId && n.PATIENTID == model.PatientId && n.RESTID == model.RestId && n.RESULT == model.Result select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_RESULT where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.PATIENTID = model.PatientId;
                        item.TRANSDT = Convert.ToDateTime(model.TransactionDate);
                        item.TESTID = model.TestId;
                        item.RESTID = model.RestId;
                        item.RESULT = model.Result;
                        item.USERPC = strHostName;
                        item.INSIPNO = item.INSIPNO;
                        item.INSLTUDE = item.INSLTUDE;
                        item.INSTIME = item.INSTIME;
                        item.INSUSERID = item.INSUSERID;

                        item.UPDIPNO = ipAddress.ToString();
                        item.UPDLTUDE = model.INSLTUDE;
                        item.UPDTIME = Convert.ToDateTime(td);
                        item.UPDUSERID = model.INSUSERID;

                        model.USERPC = item.USERPC;
                        model.UPDIPNO = item.UPDIPNO;
                        model.UPDLTUDE = item.UPDLTUDE;
                        model.UPDTIME = item.UPDTIME;
                        model.UPDUSERID = item.UPDUSERID;

                    }

                    Update_ResultLogData(model);

                    //db.Entry(cartFilter).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                    }

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
                else
                {
                    model.Result = "";
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                Result item = new Result();

                item.ID = model.ID;
                item.COMPID = model.COMPID;
                item.PATIENTID = model.PatientId;
                item.TRANSDT = Convert.ToDateTime(model.TransactionDate);
                item.TESTID = model.TestId;
                item.RESTID = model.RestId;
                item.RESULT = model.Result;

                item.USERPC = strHostName;
                item.INSIPNO = item.INSIPNO;
                item.INSLTUDE = item.INSLTUDE;
                item.INSTIME = item.INSTIME;
                item.INSUSERID = item.INSUSERID;

                item.UPDIPNO = ipAddress.ToString();
                item.UPDLTUDE = model.INSLTUDE;
                item.UPDTIME = Convert.ToDateTime(td);
                item.UPDUSERID = model.INSUSERID;

                db.Entry(item).State = EntityState.Modified;

                model.USERPC = item.USERPC;
                model.UPDIPNO = item.UPDIPNO;
                model.UPDLTUDE = item.UPDLTUDE;
                model.UPDTIME = item.UPDTIME;
                model.UPDUSERID = item.UPDUSERID;

                Update_ResultLogData(model);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }

    }
}
