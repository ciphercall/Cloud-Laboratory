using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;

namespace AslLabApi.Controllers.Api
{
    public class ApiTestMasterController : ApiController
    {

          IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiTestMasterController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
        AslLabApiContext db=new AslLabApiContext();

        [Route("api/TestCatList")]
        [HttpGet]
        public IEnumerable<TestMasterDTO> GetCatList(string query,string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                return String.IsNullOrEmpty(query) ? db.HMS_TESTMST.AsEnumerable().Select(b => new TestMasterDTO { }).ToList() :
                db.HMS_TESTMST.Where(p => p.TCATNM.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              TCatName = x.TCATNM,
                              TCatId = x.TCATID,
                              departmentid=x.DEPTID,
                              topyn=x.TOPYN
                          })
                .AsEnumerable().Select(a => new TestMasterDTO
                {
                    TestCatName = a.TCatName,
                    TestCatId = Convert.ToInt64(a.TCatId),
                    DepartmentId = a.departmentid,
                    TopYesNo = a.topyn
                }).ToList();
            }
        }


        [System.Web.Http.Route("api/DynamicTestMasterCat")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMasterDTO> Dynamicautocomplete(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                String name = "";

                var rt = db.HMS_TESTMST.Where(n => n.TCATNM.StartsWith(changedText) && n.COMPID ==compid).Select(n => new
                {
                    headname = n.TCATNM

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



                var findid = (from n in db.HMS_TESTMST where n.TCATNM == name && n.COMPID==compid select n).ToList();
                if (findid.Count != 0)
                {
                    return db.HMS_TESTMST.Where(p => p.TCATNM == name && p.COMPID==compid).Select(
                        x =>
                            new
                            {
                                TCatname = x.TCATNM,
                                TCatId = x.TCATID,
                                departmentId=x.DEPTID,
                                topyn=x.TOPYN

                            })
                        .AsEnumerable().Select(a => new TestMasterDTO
                        {
                            
                            TestCatName = a.TCatname,
                            TestCatId = Convert.ToInt64(a.TCatId),
                            DepartmentId = a.departmentId,
                            TopYesNo = a.topyn

                        }).ToList();
                }
                else
                {
                    return db.HMS_TESTMST.AsEnumerable().Select(a => new TestMasterDTO
                    {
                        TestCatName = name,
                        TestCatId = 0

                    }).ToList();
                }



            }

        }

        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_TestMasterLogData(TestMasterDTO model)
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

            var deptname_find =
                (from n in db.HMS_DEPT where n.COMPID == model.COMPID && n.DEPTID == model.DepartmentId select n).ToList();

            string dptname = "";
            foreach (var testMaster in deptname_find)
            {
                dptname = testMaster.DEPTNM;
            }

          
            aslLog.LOGDATA = Convert.ToString("Category Name: " + model.TestCatName + ",\nDepartment Name: " + dptname+",\nTop Yes/No: "+model.TopYesNo);
            aslLog.TABLEID = "HMS_TESTMST";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [System.Web.Http.Route("api/ApiTestMaster/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMasterDTO> Get(string Compid, string TCatName, string TCatID, string Department,string Topyn, string Insltude, string Inuserid)
        {
            TestMasterDTO model=new TestMasterDTO();

            Int64 compid = Convert.ToInt64(Compid);
            Int64 TCategoryID = Convert.ToInt64(TCatID);
            Int64 departmentID = Convert.ToInt64(Department);
            Int64 INSuserid = Convert.ToInt64(Inuserid);

            var ache_kina_data = (from n in db.HMS_TESTMST where n.COMPID == compid && n.TCATNM == TCatName && n.TCATID == TCategoryID && n.DEPTID == departmentID select n).ToList();
            if (ache_kina_data.Count == 0)
            {
                var name_ache_kina = (from n in db.HMS_TESTMST where n.COMPID == compid && n.TCATNM == TCatName select n).ToList();

                if (name_ache_kina.Count == 0)
                {

                    Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.HMS_TESTMST where n.COMPID==compid select n.TCATID).Max());

                    if (max_idta_berkorbo == 0)
                    {

                        TestMaster obj = new TestMaster
                        {
                            COMPID = compid,
                            TCATID = Convert.ToInt64(Convert.ToString(compid)+"01"),
                            TCATNM = TCatName,
                            DEPTID = Convert.ToInt64(Department),
                            TOPYN = Topyn


                        };
                        obj.USERPC = strHostName;
                        obj.INSIPNO = ipAddress.ToString();
                        obj.INSLTUDE = Insltude;
                        obj.INSTIME = Convert.ToDateTime(td);
                        obj.INSUSERID = INSuserid;

                      

                        model.COMPID = obj.COMPID;
                        model.TestCatId = obj.TCATID;
                        model.TestCatName = obj.TCATNM;
                        model.DepartmentId = obj.DEPTID;
                        model.TopYesNo = Topyn;

                        model.USERPC = obj.USERPC;
                        model.INSIPNO = obj.INSIPNO;
                        model.INSLTUDE = obj.INSLTUDE;
                        model.INSTIME = obj.INSTIME;
                        model.INSUSERID = obj.INSUSERID;

                        Insert_TestMasterLogData(model);
                        db.HMS_TESTMST.Add(obj);
                        db.SaveChanges();
                        yield return new TestMasterDTO
                        {

                            TestCatId = Convert.ToInt64(Convert.ToString(compid) + "01"),
                           TestId = 0,
                            TestCatName = TCatName

                        };


                    }
                    else
                    {
                        TestMaster obj = new TestMaster
                        {
                            COMPID = compid,
                            TCATID =max_idta_berkorbo+1,
                            TCATNM = TCatName,
                            DEPTID = Convert.ToInt64(Department),
                            TOPYN = Topyn

                        };
                        obj.USERPC = strHostName;
                        obj.INSIPNO = ipAddress.ToString();
                        obj.INSLTUDE = Insltude;
                        obj.INSTIME = Convert.ToDateTime(td);
                        obj.INSUSERID = INSuserid;


                        model.COMPID = obj.COMPID;
                        model.TestCatId = obj.TCATID;
                        model.TestCatName = obj.TCATNM;
                        model.DepartmentId = obj.DEPTID;
                        model.TopYesNo = Topyn;

                        model.USERPC = obj.USERPC;
                        model.INSIPNO = obj.INSIPNO;
                        model.INSLTUDE = obj.INSLTUDE;
                        model.INSTIME = obj.INSTIME;
                        model.INSUSERID = obj.INSUSERID;

                        Insert_TestMasterLogData(model);
                        db.HMS_TESTMST.Add(obj);
                        db.SaveChanges();
                        yield return new TestMasterDTO
                        {

                            TestCatId = max_idta_berkorbo+1,
                            TestId = 0,
                            TestCatName = TCatName,
                            

                        };



                    }
                }


            }
            else
            {
                var ff = (from n in db.HMS_TEST where n.COMPID==compid && n.TCATID == TCategoryID && n.DEPTID==departmentID select n).ToList();

                if (ff.Count == 0)
                {

                    yield return new TestMasterDTO
                    {

                        TestCatId = TCategoryID,
                        TestId = 0,
                        TestCatName = TCatName


                    };

                }
                else
                {



                    var testmstDto = (from t1 in db.HMS_TEST
                                        //join t2 in db.KART_CATEGORY on t1.LEVELGID equals t2.CATID
                                        where t1.TCATID == TCategoryID
                                        select new
                                        {
                                            Id = t1.ID,
                                            TestCatid = t1.TCATID,
                                            DepartmentId = t1.DEPTID,
                                            TestID = t1.TESTID,
                                            Testname = t1.TESTNM,
                                            rate = t1.RATE,
                                            pcntd=t1.PCNTD


                                        });
                    foreach (var item in testmstDto)
                    {
                        yield return new TestMasterDTO
                        {
                            ID = item.Id,
                            TestCatId = Convert.ToInt64(item.TestCatid),
                            TestCatName = TCatName,
                            DepartmentId = Convert.ToInt64(Department),
                            TestId = item.TestID,
                            TestName =item.Testname,
                            Rate = item.rate,
                            PcnTD = item.pcntd
                        };
                    }



                }
            }

        }

        public void Insert_TestLogData(TestMasterDTO model)
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

            var deptname_find =
                (from n in db.HMS_DEPT where n.COMPID == model.COMPID && n.DEPTID == model.DepartmentId select n).ToList();

            string dptname = "";
            foreach (var testMaster in deptname_find)
            {
                dptname = testMaster.DEPTNM;
            }
            var catname_find =
                (from n in db.HMS_TESTMST where n.COMPID == model.COMPID && n.TCATID == model.TestCatId select n).ToList();

            string catname = "";
            foreach (var testMaster in catname_find)
            {
                catname = testMaster.TCATNM;
            }

            aslLog.LOGDATA = Convert.ToString("Category Name: " + catname + ",\nDepartment Name: " + dptname + ",\nTest Name: " + model.TestName +
                ",\nRate: "+model.Rate+",\nDiscount(%): "+model.PcnTD);
            aslLog.TABLEID = "HMS_TEST";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [Route("api/ApiTestMaster/TestChild")]
        [HttpPost]
        public HttpResponseMessage TestChild(TestMasterDTO model)
        {

            var check_data = (from n in db.HMS_TEST where n.COMPID==model.COMPID && n.TCATID == model.TestCatId && n.DEPTID == model.DepartmentId && n.TESTNM==model.TestName select n).ToList();
            if (check_data.Count == 0)
            {
                var ache_kina_data = (from n in db.HMS_TEST where n.COMPID==model.COMPID && n.TCATID == model.TestCatId && n.DEPTID==model.DepartmentId select n).ToList();

                if (ache_kina_data.Count == 0)
                {


                    Test testData = new Test();
                    testData.COMPID = model.COMPID;
                    testData.TCATID = model.TestCatId;
                    testData.TCATID = model.TestCatId;
                    testData.DEPTID = model.DepartmentId;
                    testData.TESTNM = model.TestName;
                    testData.TESTID = Convert.ToInt64(Convert.ToString(model.TestCatId) + "001");
                    testData.RATE = model.Rate;
                    testData.PCNTD = model.PcnTD;

                    testData.USERPC = strHostName;
                    testData.INSIPNO =ipAddress.ToString();
                    testData.INSLTUDE = model.INSLTUDE;
                    testData.INSTIME =Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;



                    db.HMS_TEST.Add(testData);

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.TestCatId = testData.TCATID;
                    model.DepartmentId = Convert.ToInt64(testData.DEPTID);
                    model.TestId = Convert.ToInt64(testData.TESTID);
                    model.TestName = testData.TESTNM;
                    model.Rate = testData.RATE;
                    model.PcnTD = testData.PCNTD;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSLTUDE = testData.INSLTUDE;
                    model.INSTIME = testData.INSTIME;
                    model.INSUSERID = testData.INSUSERID;

                    Insert_TestLogData(model);

                    db.SaveChanges();

                   
                }
                else
                {
                    Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.HMS_TEST where n.COMPID==model.COMPID && n.TCATID == model.TestCatId && n.DEPTID==model.DepartmentId select n.TESTID).Max());
                    Test testData = new Test();

                    testData.COMPID = model.COMPID;
                    testData.TCATID = model.TestCatId;
                    testData.TCATID = model.TestCatId;
                    testData.DEPTID = model.DepartmentId;
                    testData.TESTNM = model.TestName;
                    testData.TESTID = max_idta_berkorbo + 1;
                    testData.RATE = model.Rate;
                    testData.PCNTD = model.PcnTD;

                    testData.USERPC = strHostName;
                    testData.INSIPNO = ipAddress.ToString();
                    testData.INSLTUDE = model.INSLTUDE;
                    testData.INSTIME = Convert.ToDateTime(td);
                    testData.INSUSERID = model.INSUSERID;

                    db.HMS_TEST.Add(testData);

                    model.ID = testData.ID;
                    model.COMPID = model.COMPID;
                    model.TestCatId = testData.TCATID;
                    model.DepartmentId = Convert.ToInt64(testData.DEPTID);
                    model.TestId = Convert.ToInt64(testData.TESTID);
                    model.TestName = testData.TESTNM;
                    model.Rate = testData.RATE;
                    model.PcnTD = testData.PCNTD;
                    model.USERPC = testData.USERPC;
                    model.INSIPNO = testData.INSIPNO;
                    model.INSLTUDE = testData.INSLTUDE;
                    model.INSTIME = testData.INSTIME;
                    model.INSUSERID = testData.INSUSERID;


                    Insert_TestLogData(model);
                    db.SaveChanges();

                   
                }

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

        [Route("api/ApiTestMaster/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(TestMasterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_TEST where n.COMPID==model.COMPID && n.TCATID == model.TestCatId && n.DEPTID == model.DepartmentId && n.TESTNM==model.TestName select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {
                    
                    var data_find = (from n in db.HMS_TEST where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.TCATID = model.TestCatId;
                        item.DEPTID = model.DepartmentId;
                        item.TESTID = model.TestId;
                        item.TESTNM = model.TestName;
                        item.RATE = model.Rate;
                        item.PCNTD = model.PcnTD;

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
                    model.TestId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                Test item = new Test();

                item.ID = model.ID;
                item.COMPID = model.COMPID;
                item.TCATID = model.TestCatId;
                item.DEPTID = model.DepartmentId;
                item.TESTID = model.TestId;
                item.TESTNM = model.TestName;
                item.RATE = model.Rate;
                item.PCNTD = model.PcnTD;

                db.Entry(item).State = EntityState.Modified;

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

        [Route("api/ApiTestMaster/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(TestMasterDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_TEST WHERE ID='{0}'", model.ID);
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
            Test testObj  = new Test();






            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }

        [System.Web.Http.Route("api/ApiTestMaster/HeadData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<TestMasterDTO> GetHead(string Compid)
        {
            //Int64 compid = Convert.ToInt64(Compid);
            //return db.HMS_TESTMST.ToList().Where(n => n.COMPID == compid);

            Int64 compid = Convert.ToInt64(Compid);


            var masterdata = (from t1 in db.HMS_TESTMST
                             join t2 in db.HMS_DEPT on t1.DEPTID equals t2.DEPTID into xx
                             from subpet in xx.DefaultIfEmpty()
                             where  t1.COMPID == compid 
                             select new
                             {
                                 Id = t1.ID,
                                
                                 compID = t1.COMPID,
                                tcatid=t1.TCATID,
                                tcatname=t1.TCATNM,
                                 Departmentid = (subpet == null ? 0 : subpet.DEPTID),
                                 Departmentname = (subpet == null ? String.Empty : subpet.DEPTNM),
                                 topyn=t1.TOPYN


                             });


            foreach (var item in masterdata)
            {
                yield return new TestMasterDTO
                {
                    ID = item.Id,
                    COMPID = item.compID,
                    TestCatId = item.tcatid,
                    TestCatName = item.tcatname,
                    DepartmentId = Convert.ToInt64(item.Departmentid),
                    DepartmentName=item.Departmentname,
                    TopYesNo = item.topyn

                };
            }
            //return db.HMS_TESTMST.Where(p =>  p.COMPID == compid).Select(
            //          x =>
            //          new
            //          {
            //              TCatName = x.TCATNM,
            //              TCatId = x.TCATID,
            //              departmentid = x.DEPTID
            //          })
            //.AsEnumerable().Select(a => new TestMasterDTO
            //{
            //    TestCatName = a.TCatName,
            //    TestCatId = Convert.ToInt64(a.TCatId),
            //    DepartmentId = a.departmentid
            //}).ToList();
        }
        [Route("api/ApiTestMasterHead/SaveData2")]
        [HttpPost]
        public HttpResponseMessage SaveData2(TestMasterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_TESTMST where n.COMPID == model.COMPID && n.TCATID == model.TestCatId && n.DEPTID == model.DepartmentId select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_TESTMST where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.TCATID = model.TestCatId;
                        item.TCATNM = model.TestCatName;
                        item.DEPTID = model.DepartmentId;
                        item.TOPYN = model.TopYesNo;

                        item.USERPC = item.USERPC;
                        item.INSIPNO = item.INSIPNO;
                        item.INSLTUDE = item.INSLTUDE;
                        item.INSTIME = item.INSTIME;
                        item.INSUSERID = item.INSUSERID;


                        item.UPDIPNO = ipAddress.ToString();
                        item.UPDLTUDE = model.UPDLTUDE;
                        item.UPDUSERID = model.UPDUSERID;
                        item.UPDTIME = Convert.ToDateTime(td);

                    }

                    var test_data =
                        (from n in db.HMS_TEST where n.COMPID == model.COMPID && n.TCATID == model.TestCatId select n);
              
                    foreach (var testitem in test_data)
                    {
                        testitem.ID = testitem.ID;
                        testitem.COMPID = testitem.COMPID;
                        testitem.TCATID = testitem.TCATID;
                        testitem.TESTID = testitem.TESTID;
                        testitem.TESTNM = testitem.TESTNM;
                        testitem.DEPTID = model.DepartmentId;
                        testitem.RATE = testitem.RATE;
                        testitem.PCNTD = testitem.PCNTD;

                        testitem.USERPC = testitem.USERPC;
                        testitem.INSIPNO = testitem.INSIPNO;
                        testitem.INSLTUDE = testitem.INSLTUDE;
                        testitem.INSTIME = testitem.INSTIME;
                        testitem.INSUSERID = testitem.INSUSERID;


                        testitem.UPDIPNO = ipAddress.ToString();
                        testitem.UPDLTUDE = model.UPDLTUDE;
                        testitem.UPDUSERID = model.UPDUSERID;
                        testitem.UPDTIME = Convert.ToDateTime(td);

                    }

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
                    model.TestId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                TestMaster item = new TestMaster();

                item.ID = model.ID;
                item.COMPID = model.COMPID;
                item.TCATID = model.TestCatId;
                item.TCATNM = model.TestCatName;
                item.DEPTID = model.DepartmentId;
                item.TOPYN = model.TopYesNo;

                item.USERPC = item.USERPC;
                item.INSIPNO = item.INSIPNO;
                item.INSLTUDE = item.INSLTUDE;
                item.INSTIME = item.INSTIME;
                item.INSUSERID = item.INSUSERID;


                item.UPDIPNO = ipAddress.ToString();
                item.UPDLTUDE = model.UPDLTUDE;
                item.UPDUSERID = model.UPDUSERID;
                item.UPDTIME = Convert.ToDateTime(td);

                db.Entry(item).State = EntityState.Modified;

                var test_data =
                        (from n in db.HMS_TEST where n.COMPID == model.COMPID && n.TCATID == model.TestCatId select n);
                //db.Entry(cartFilter).State = EntityState.Modified;
                foreach (var testitem in test_data)
                {
                    testitem.ID = testitem.ID;
                    testitem.COMPID = testitem.COMPID;
                    testitem.TCATID = testitem.TCATID;
                    testitem.TESTID = testitem.TESTID;
                    testitem.TESTNM = testitem.TESTNM;
                    testitem.DEPTID = model.DepartmentId;
                    testitem.RATE = testitem.RATE;
                    testitem.PCNTD = testitem.PCNTD;

                    testitem.USERPC = testitem.USERPC;
                    testitem.INSIPNO = testitem.INSIPNO;
                    testitem.INSLTUDE = testitem.INSLTUDE;
                    testitem.INSTIME = testitem.INSTIME;
                    testitem.INSUSERID = testitem.INSUSERID;


                    testitem.UPDIPNO = ipAddress.ToString();
                    testitem.UPDLTUDE = model.UPDLTUDE;
                    testitem.UPDUSERID = model.UPDUSERID;
                    testitem.UPDTIME = Convert.ToDateTime(td);



                }
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
