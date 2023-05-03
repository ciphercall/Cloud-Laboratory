using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.Api
{
    public class ApiTestVController : ApiController
    {
        
          IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiTestVController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        AslLabApiContext db = new AslLabApiContext();

        [System.Web.Http.Route("api/ApiTestV/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestVDTO> Get(string Compid, string TCatID, string TestID)
        {

            Int64 compid = Convert.ToInt64(Compid);
            Int64 TCategoryID = Convert.ToInt64(TCatID);
            Int64 testID = Convert.ToInt64(TestID);

            var ache_kina_data = (from n in db.HMS_TESTV where n.COMPID == compid && n.TCATID == TCategoryID && n.TESTID == testID select n).ToList();
            if (ache_kina_data.Count == 0)
            {
                //Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.HMS_TESTMST where n.COMPID == compid select n.TCATID).Max());


                yield return new TestVDTO
                {

                    TestCatId = TCategoryID,
                    TestId = testID,
                    TestVACID = 0,


                };

            }
            else
            {




                var testVDto = (from t1 in db.HMS_TESTV
                                join t2 in db.HMS_TEST on t1.TVACID equals t2.TESTID
                                where t1.TCATID == TCategoryID && t1.COMPID == compid && t1.TESTID == testID
                                select new
                                {
                                    Id = t1.ID,
                                    TestCatid = t1.TCATID,
                                    compID = t1.COMPID,
                                    TestID = t1.TESTID,
                                    testVname=t2.TESTNM,
                                    testVACID = t1.TVACID,



                                });
                foreach (var item in testVDto)
                {
                    yield return new TestVDTO
                    {
                        ID = item.Id,
                        COMPID = item.compID,
                        TestCatId = item.TestCatid,
                        TestId = item.TestID,
                        TestVACID = item.testVACID,
                        TestVName = item.testVname
                    };
                }




            }
        }

        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_TestVaccumLogData(TestVDTO model)
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

            var catname_find =
                (from n in db.HMS_TESTMST where n.COMPID == model.COMPID && n.TCATID == model.TestCatId select n).ToList();

            string catname = "";
            foreach (var testMaster in catname_find)
            {
                catname = testMaster.TCATNM;
            }

            var testname_find =
                (from n in db.HMS_TEST where n.COMPID == model.COMPID && n.TESTID == model.TestId select n).ToList();

            string testname = "";
            foreach (var test in testname_find)
            {
                testname = test.TESTNM;
            }

            var vaccumname_find =
               (from n in db.HMS_TEST where n.COMPID == model.COMPID && n.TESTID == model.TestVACID select n).ToList();

            string vaccumname = "";
            foreach (var test in vaccumname_find)
            {
                vaccumname = test.TESTNM;
            }

            aslLog.LOGDATA = Convert.ToString("Category Name: " + catname + ",\nTest Name: " + testname+",\nVaccum Name: "+vaccumname);
            aslLog.TABLEID = "HMS_TESTMST";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [System.Web.Http.Route("api/grid/TestVChild")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddChildData(TestVDTO model)
        {

            var check_data = (from n in db.HMS_TESTV where n.COMPID == model.COMPID && n.TCATID == model.TestCatId && n.TESTID == model.TestId && n.TVACID == model.TestVACID select n).ToList();
            if (check_data.Count == 0)
            {


                TestV testData = new TestV();

                testData.COMPID = model.COMPID;
                testData.TCATID = Convert.ToInt64(model.TestCatId);
                testData.TESTID = Convert.ToInt64(model.TestId);
                testData.TVACID = Convert.ToInt64(model.TestVACID);

                testData.USERPC = strHostName;
                testData.INSIPNO = ipAddress.ToString();
                testData.INSLTUDE = model.INSLTUDE;
                testData.INSTIME = Convert.ToDateTime(td);
                testData.INSUSERID = model.INSUSERID;



                db.HMS_TESTV.Add(testData);

                model.ID = testData.ID;
                model.COMPID = model.COMPID;
                model.TestCatId = testData.TCATID;
                model.TestId = testData.TESTID;
                model.TestVACID = testData.TVACID;

                model.USERPC = testData.USERPC;
                model.INSIPNO = testData.INSIPNO;
                model.INSTIME = testData.INSTIME;

                Insert_TestVaccumLogData(model);
                db.SaveChanges();

            

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.TestVACID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }

        [System.Web.Http.Route("api/ApiTestV/SaveData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveData(TestVDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_TESTV where n.COMPID == model.COMPID && n.TCATID == model.TestCatId && n.TESTID == model.TestId && n.TVACID == model.TestVACID select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_TESTV where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.TCATID = Convert.ToInt64(model.TestCatId);
                        item.TESTID = Convert.ToInt64(model.TestId);
                        item.TVACID = Convert.ToInt64(model.TestVACID);

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
                    model.TestVACID = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                TestV testV = new TestV();

                testV.ID = model.ID;
                testV.COMPID = model.COMPID;
                testV.TCATID = Convert.ToInt64(model.TestCatId);
                testV.TESTID = Convert.ToInt64(model.TestId);
                testV.TVACID = Convert.ToInt64(model.TestVACID);

                //var data_find = (from n in db.HMS_TESTV where n.ID == model.ID select n).ToList();
                //foreach (var item in data_find)
                //{
                //    item.ID = model.ID;
                //    item.COMPID = model.COMPID;
                //    item.TCATID = model.TestCatId;
                //    item.TESTID = model.TestId;
                //    item.TVACID = model.TestVACID;

                //}
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

        [System.Web.Http.Route("api/ApiTestV/DeleteData")]
        [System.Web.Http.HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(TestVDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_TESTV WHERE ID='{0}'", model.ID);
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

        [System.Web.Http.Route("api/ApiTestV/TestNameLoad")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage TestNameLoad(string Catid, string Compid)
        {
            Int64 Categoryid = Convert.ToInt64(Catid);
            Int64 CompID= Convert.ToInt64(Compid);
            var testname_search =
                (from n in db.HMS_TEST where n.TCATID == Categoryid && n.COMPID == CompID select n).ToList();
            List<SelectListItem> xyz=new List<SelectListItem>();
            foreach (var item in testname_search)
            {
               xyz.Add(new SelectListItem{Value = Convert.ToString(item.TESTID),Text = item.TESTNM});
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, xyz);
            return response;
          
        }

    }
}
