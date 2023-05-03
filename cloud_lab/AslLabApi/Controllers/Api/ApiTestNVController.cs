using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AslLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.Api
{
    public class ApiTestNVController : ApiController
    {
        
          IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiTestNVController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        AslLabApiContext db = new AslLabApiContext();

        [Route("api/RestnmList")]
        [HttpGet]
        public IEnumerable<TestNVDTO> GetRestnmList(string query, string query2,string query3)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                Int64 testid = Convert.ToInt64(query3);
                return String.IsNullOrEmpty(query) ? db.HMS_TESTNV.AsEnumerable().Select(b => new TestNVDTO{ }).ToList() :
                db.HMS_TESTNV.Where(p => p.RESTNM.StartsWith(query) && p.COMPID == compid && p.TESTID==testid).Distinct().Select(
                          x =>
                          new
                          {
                              Restname = x.RESTNM,
                              Restid = x.RESTID,
                              
                          })
                .AsEnumerable().Distinct().Select(a => new TestNVDTO
                {
                    RestName = a.Restname,
                    RestId = Convert.ToInt64(a.Restid)
                   
                }).ToList();
            }
        }
        [System.Web.Http.Route("api/DynamicRestname")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestNVDTO> Dynamicautocomplete(string changedText, string changedText2,string changedText3)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                Int64 testid = Convert.ToInt64(changedText3);
                string name = "";

                var rt = db.HMS_TESTNV.Where(n => n.RESTNM.StartsWith(changedText) && n.COMPID == compid && n.TESTID==testid).Distinct().Select(n => new
                {
                    headname = n.RESTNM

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



                var findid = (from n in db.HMS_TESTNV where n.RESTNM == name && n.COMPID == compid && n.TESTID==testid select n).Distinct().ToList();
                if (findid.Count != 0)
                {
                    return db.HMS_TESTNV.Where(p => p.RESTNM == name && p.COMPID == compid && p.TESTID==testid).Distinct().Select(
                        x =>
                            new
                            {
                                Restnm = x.RESTNM,
                                Restid = x.RESTID
                               

                            })
                        .AsEnumerable().Distinct().Select(a => new TestNVDTO
                        {

                            RestName = a.Restnm,
                            RestId = Convert.ToInt64(a.Restid)
                           
                        }).ToList();
                }
                else
                {
                    return db.HMS_TESTNV.AsEnumerable().Select(a => new TestNVDTO
                    {
                        RestName = Convert.ToString(name),
                        RestId = 0

                    }).ToList();
                }



            }

        }
        [System.Web.Http.Route("api/ApiTestNV/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestNVDTO> Get(string Compid, string testid)
        {

            Int64 compid = Convert.ToInt64(Compid);
            Int64 Testid = Convert.ToInt64(testid);
            //Int64 Restid = Convert.ToInt64(restid);

            var ache_kina_data = (from n in db.HMS_TESTNV where n.COMPID == compid && n.TESTID == Testid  select n).ToList();
            if (ache_kina_data.Count == 0)
            {
               


                yield return new TestNVDTO
                {

                    TestId = Testid,
                  //RestId = Restid,
                    RestGroupName = "",


                };

            }
            else
            {




                var testNVDto = (from t1 in db.HMS_TESTNV
                                //join t2 in db.HMS_TEST on t1.TVACID equals t2.TESTID
                                where t1.TESTID == Testid && t1.COMPID == compid 
                                select new
                                {
                                    Id = t1.ID,
                                    Testid = t1.TESTID,
                                    compID = t1.COMPID,
                                    Restid = t1.RESTID,
                                    Restname = t1.RESTNM,
                                    RestGname = t1.RESTGR,
                                    Restmu=t1.RESTMU,
                                    Showtype=t1.SHOWTP,
                                    Length=t1.LENGTH,
                                    Decimal=t1.DECIML,
                                    NValue=t1.NVALUE,
                                    DValue=t1.DVALUE,
                                    serial=t1.RESTSL,
                                    groupshow=t1.RESTGRV



                                });
                foreach (var item in testNVDto)
                {
                    yield return new TestNVDTO
                    {
                        ID = item.Id,
                        COMPID = item.compID,
                        TestId = item.Testid,
                        RestId = item.Restid,
                        RestName = item.Restname,
                        RestGroupName = item.RestGname,
                        RestMU = item.Restmu,
                        ShowType = item.Showtype,
                        Length = item.Length,
                        Decimal = item.Decimal,
                        NValue = item.NValue,
                        DValue = item.DValue,
                        SerialNo = item.serial,
                        GroupShow = item.groupshow
                        
                    };
                }




            }
        }


        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_TestNVLogData(TestNVDTO model)
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

            string testname = "";
            foreach (var test in testname_find)
            {
                testname = test.TESTNM;
            }


            aslLog.LOGDATA = Convert.ToString("Test Name: " + testname + ",\nResult Particulrs: " + model.RestName + ",\nResult Group: " + model.RestGroupName + ",\nM.Unit: " + model.RestMU
                +",\nShow Type: "+model.ShowType+",\nLength: "+model.Length+",\nDecimal: "+model.Decimal+",\nNormal Values: "+model.NValue+",\nSl No: "+model.SerialNo
                +",\nGroup Show: "+model.GroupShow);
            aslLog.TABLEID = "HMS_TESTNV";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }



        [Route("api/grid/TestNVChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(TestNVDTO model)
        {

            var check_data = (from n in db.HMS_TESTNV where n.COMPID == model.COMPID && n.TESTID == model.TestId &&  n.RESTID == model.RestId && n.RESTGR==model.RestGroupName select n).ToList();
            if (check_data.Count == 0)
            {
                if (model.RestId == null|| model.RestId==0)
                {
                    var max_data = Convert.ToInt64(
                   (from n in db.HMS_TESTNV where n.COMPID == model.COMPID && n.TESTID == model.TestId select n.RESTID)
                       .Max());
                    TestNV testData = new TestNV();
                    if (max_data == 0)
                {
                   

                    testData.COMPID = model.COMPID;
                    testData.TESTID = Convert.ToInt64(model.TestId);
                    testData.RESTID = Convert.ToInt64(Convert.ToString(model.TestId)+"01");
                    testData.RESTNM = model.RestName;
                    testData.RESTGR = model.RestGroupName;
                    testData.RESTMU = model.RestMU;
                    testData.LENGTH = model.Length;
                    testData.DECIML = model.Decimal;
                    testData.SHOWTP = model.ShowType;
                    testData.NVALUE = model.NValue;
                    testData.DVALUE = model.DValue;

                    testData.RESTSL = model.SerialNo;
                    testData.RESTGRV = model.GroupShow;

                    testData.USERPC = strHostName;
                    testData.INSIPNO = ipAddress.ToString();
                    testData.INSLTUDE = model.INSLTUDE;
                    testData.INSTIME = Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;




                    db.HMS_TESTNV.Add(testData);

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.RestId = testData.RESTID;
                    model.TestId = testData.TESTID;
                    model.RestName = testData.RESTNM;
                    model.RestGroupName = testData.RESTGR;
                    model.RestMU = testData.RESTMU;
                    model.ShowType = testData.SHOWTP;
                    model.Length = testData.LENGTH;
                    model.Decimal = testData.DECIML;
                    model.NValue = testData.NVALUE;
                    model.DValue = testData.DVALUE;
                    model.SerialNo = testData.RESTSL;
                    model.GroupShow = testData.RESTGRV;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSTIME = testData.INSTIME;

                    Insert_TestNVLogData(model);


                    db.SaveChanges();
                }
                    else
                    {
                        testData.COMPID = model.COMPID;
                        testData.TESTID = Convert.ToInt64(model.TestId);
                        testData.RESTID = max_data+1;
                        testData.RESTNM = model.RestName;
                        testData.RESTGR = model.RestGroupName;
                        testData.RESTMU = model.RestMU;
                        testData.LENGTH = model.Length;
                        testData.DECIML = model.Decimal;
                        testData.SHOWTP = model.ShowType;
                        testData.NVALUE = model.NValue;
                        testData.DVALUE = model.DValue;
                        testData.RESTSL = model.SerialNo;
                        testData.RESTGRV = model.GroupShow;

                        testData.USERPC = strHostName;
                        testData.INSIPNO = ipAddress.ToString();
                        testData.INSLTUDE = model.INSLTUDE;
                        testData.INSTIME = Convert.ToDateTime(td);
                        testData.INSUSERID = model.INSUSERID;

                        db.HMS_TESTNV.Add(testData);

                        model.ID = testData.ID;
                        model.COMPID = model.COMPID;
                        model.RestId = testData.RESTID;
                        model.TestId = testData.TESTID;
                        model.RestName = testData.RESTNM;
                        model.RestGroupName = testData.RESTGR;
                        model.RestMU = testData.RESTMU;
                        model.ShowType = testData.SHOWTP;
                        model.Length = testData.LENGTH;
                        model.Decimal = testData.DECIML;
                        model.NValue = testData.NVALUE;
                        model.DValue = testData.DVALUE;
                        model.SerialNo = testData.RESTSL;
                        model.GroupShow = testData.RESTGRV;
                        model.USERPC = testData.USERPC;
                        model.INSIPNO = testData.INSIPNO;
                        model.INSTIME = testData.INSTIME;
                        Insert_TestNVLogData(model);
                        db.SaveChanges();
                    }
                    

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                    return response;

                }
                
               
               
                
                else
                {
                    TestNV testData = new TestNV();

                    testData.COMPID = model.COMPID;
                    testData.TESTID = Convert.ToInt64(model.TestId);
                    testData.RESTID = model.RestId;
                    testData.RESTNM = model.RestName;
                    testData.RESTGR = model.RestGroupName;
                    testData.RESTMU = model.RestMU;
                    testData.LENGTH = model.Length;
                    testData.DECIML = model.Decimal;
                    testData.SHOWTP = model.ShowType;
                    testData.NVALUE = model.NValue;
                    testData.DVALUE = model.DValue;
                    testData.RESTSL = model.SerialNo;
                    testData.RESTGRV = model.GroupShow;

                    testData.USERPC = strHostName;
                    testData.INSIPNO = ipAddress.ToString();
                    testData.INSLTUDE = model.INSLTUDE;
                    testData.INSTIME = Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;

                    db.HMS_TESTNV.Add(testData);

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.RestId = testData.RESTID;
                    model.TestId = testData.TESTID;
                    model.RestName = testData.RESTNM;
                    model.RestGroupName = testData.RESTGR;
                    model.RestMU = testData.RESTMU;
                    model.ShowType = testData.SHOWTP;
                    model.Length = testData.LENGTH;
                    model.Decimal = testData.DECIML;
                    model.NValue = testData.NVALUE;
                    model.DValue = testData.DVALUE;
                    model.SerialNo = testData.RESTSL;
                    model.GroupShow = testData.RESTGRV;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSTIME = testData.INSTIME;
                    Insert_TestNVLogData(model);
                    db.SaveChanges();

                  

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                    return response;

                }
              

               
            }
            else
            {
                model.RestGroupName = "";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }


        [Route("api/ApiTestNV/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(TestNVDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_TESTNV where n.COMPID == model.COMPID && n.TESTID == model.TestId && n.RESTID == model.RestId && n.RESTGR == model.RestGroupName select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_TESTNV where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.TESTID = Convert.ToInt64(model.TestId);
                        item.RESTID = Convert.ToInt64(model.RestId);
                        item.RESTNM = model.RestName;
                        item.RESTGR = model.RestGroupName;
                        item.RESTMU = model.RestMU;
                        item.SHOWTP = model.ShowType;
                        item.LENGTH = model.Length;
                        item.DECIML = model.Decimal;
                        item.NVALUE = model.NValue;
                        item.DVALUE = model.DValue;
                        item.RESTSL = model.SerialNo;
                        item.RESTGRV = model.GroupShow;

                    }


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
                    model.RestGroupName = "";
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                TestNV testV = new TestNV();

                testV.ID = model.ID;
                testV.COMPID = model.COMPID;
                testV.TESTID = Convert.ToInt64(model.TestId);
                testV.RESTID = Convert.ToInt64(model.RestId);
                testV.RESTNM = model.RestName;
                testV.RESTGR = model.RestGroupName;
                testV.RESTMU = model.RestMU;
                testV.SHOWTP = model.ShowType;
                testV.LENGTH = model.Length;
                testV.DECIML = model.Decimal;
                testV.NVALUE = model.NValue;
                testV.DVALUE = model.DValue;
                testV.RESTSL = model.SerialNo;
                testV.RESTGRV = model.GroupShow;

               
                db.Entry(testV).State = EntityState.Modified;
                db.SaveChanges();
                try
                {
                    //
                    //db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }


        [Route("api/ApiTestNV/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(TestNVDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_TESTNV WHERE ID='{0}'", model.ID);
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
            Test testObj = new Test();






            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }



        [System.Web.Http.Route("apiTestNV/TestNameList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestNVDTO> GetTestNameList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                Int64 catid = Convert.ToInt64(Convert.ToString(compid) + "01");
               
                    return String.IsNullOrEmpty(query) ? db.HMS_TEST.AsEnumerable().Select(b => new TestNVDTO { }).ToList() :
               db.HMS_TEST.Where(p => p.TESTNM.StartsWith(query) && p.COMPID == compid && p.TCATID != catid).Select(
                         x =>
                         new
                         {
                             testname = x.TESTNM,
                             testid = x.TESTID
                            
                         })
               .AsEnumerable().Select(a => new TestNVDTO
               {
                   TestName = a.testname,
                   TestId = Convert.ToInt64(a.testid)
                 

               }).ToList();
              
              
            }
        }


        [System.Web.Http.Route("apiTestNV/DynamicTestName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestNVDTO> Dynamicautocomplete(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                Int64 catid = Convert.ToInt64(Convert.ToString(compid) + "01");

                String name = "";
                var rt = db.HMS_TEST.Where(n => n.TESTNM.StartsWith(changedText) && n.COMPID == compid && n.TCATID!=catid).Select(n => new
                {
                    headname = n.TESTNM

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

                var findid = (from n in db.HMS_TEST where n.TESTNM == name && n.COMPID == compid && n.TCATID != catid select n).ToList();
                

                if (findid.Count != 0)
                {
                   


                    return (from t1 in db.HMS_TEST
                          
                            where t1.COMPID == compid && t1.TESTNM == name && t1.TCATID!=catid
                            select new
                            {
                                testname = t1.TESTNM,
                                testid = t1.TESTID
                               


                            })
                   .AsEnumerable().Select(a => new TestNVDTO
                   {

                       TestName = a.testname,
                       TestId= Convert.ToInt64(a.testid),
                     

                   }).ToList();


                }
                else
                {
                    return db.HMS_TEST.AsEnumerable().Select(a => new TestNVDTO
                    {
                        TestName = name,
                        TestId = 0


                    }).ToList();
                }



            }

        }


    }
}
