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
    public class ApiMgtController : ApiController
    {


        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiMgtController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
        AslLabApiContext db = new AslLabApiContext();

        [Route("api/MgtList")]
        [HttpGet]
        public IEnumerable<ManagementDTO> GetMgtList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                return String.IsNullOrEmpty(query) ? db.HMS_MGT.AsEnumerable().Select(b => new ManagementDTO { }).ToList() :
                db.HMS_MGT.Where(p => p.MGTNM.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              mgtname = x.MGTNM,
                              mgtid = x.MGTID,

                              designation = x.DESIG,
                              address = x.ADDRESS,
                              mobileno1 = x.MOBNO1,
                              mobileno2 = x.MOBNO2,
                              emailid = x.EMAILID
                          })
                .AsEnumerable().Select(a => new ManagementDTO
                {
                    ManagementName = a.mgtname,
                    ManagementId = Convert.ToInt64(a.mgtid),

                    Designation = a.designation,
                    Address = a.address,
                    MobileNo1 = a.mobileno1,
                    MobileNo2 = a.mobileno2,
                    EmailId = a.emailid
                }).ToList();
            }
        }

        [System.Web.Http.Route("api/DynamicMgt")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ManagementDTO> Dynamicautocomplete(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);

                String name = "";

                var rt = db.HMS_MGT.Where(n => n.MGTNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                {
                    headname = n.MGTNM

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



                var findid = (from n in db.HMS_MGT where n.MGTNM == name && n.COMPID == compid select n).ToList();
                if (findid.Count != 0)
                {
                    return db.HMS_MGT.Where(p => p.MGTNM == name && p.COMPID == compid).Select(
                        x =>
                            new
                            {
                                mgtname = x.MGTNM,
                                mgtid = x.MGTID,

                                designation = x.DESIG,
                                address = x.ADDRESS,
                                mobileno1 = x.MOBNO1,
                                mobileno2 = x.MOBNO2,
                                emailid = x.EMAILID

                            })
                        .AsEnumerable().Select(a => new ManagementDTO
                        {

                            ManagementName = a.mgtname,
                            ManagementId = Convert.ToInt64(a.mgtid),

                            Designation = a.designation,
                            Address = a.address,
                            MobileNo1 = a.mobileno1,
                            MobileNo2 = a.mobileno2,
                            EmailId = a.emailid

                        }).ToList();
                }
                else
                {
                    return db.HMS_MGT.AsEnumerable().Select(a => new ManagementDTO
                    {
                        ManagementName = name,
                        ManagementId = 0

                    }).ToList();
                }



            }

        }
        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_MgtLogData(ManagementDTO model)
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





            aslLog.LOGDATA = Convert.ToString("Management Name: " + model.ManagementName + ",\nDesignation: " + model.Designation + ",\nAddress: " + model.Address + ",\nMobile No 1: " + model.MobileNo1
                + ",\nMobile No 2: " + model.MobileNo2 + ",\nEmail Id: " + model.EmailId);
            aslLog.TABLEID = "HMS_MGT";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        [System.Web.Http.Route("api/ApiMgt/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ManagementDTO> Get(string Compid, string ManagementId, string ManagementName, string Designation, string Address, string MobileNo1, string MobileNo2, string EmailId, string Insltude, string Insuserid)
        {

            Int64 compid = Convert.ToInt64(Compid);

            Int64 mgtid = Convert.ToInt64(ManagementId);

            ManagementDTO model = new ManagementDTO();

            var ache_kina_data = (from n in db.HMS_MGT where n.COMPID == compid && n.MGTID == mgtid && n.MGTNM == ManagementName select n).ToList();
            if (ache_kina_data.Count == 0)
            {


                Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.HMS_MGT where n.COMPID == compid select n.MGTID).Max());

                if (max_idta_berkorbo == 0)
                {

                    Management obj = new Management
                    {
                        COMPID = compid,

                        MGTID = Convert.ToInt64(Convert.ToString(compid) + "101"),
                        MGTNM = ManagementName,
                        DESIG = Designation,
                        ADDRESS = Address,
                        MOBNO1 = MobileNo1,
                        MOBNO2 = MobileNo2,
                        EMAILID = EmailId,
                        USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),
                        INSLTUDE = Insltude,
                        INSTIME = Convert.ToDateTime(td),
                        INSUSERID = Convert.ToInt64(Insuserid)

                    };

                    db.HMS_MGT.Add(obj);

                    model.ID = obj.ID;
                    model.COMPID = obj.COMPID;
                    model.ManagementId = obj.MGTID;
                    model.ManagementName = obj.MGTNM;
                    model.Designation = obj.DESIG;
                    model.Address = obj.ADDRESS;
                  
                    model.MobileNo1 = obj.MOBNO1;
                    model.MobileNo2 = obj.MOBNO2;
                    model.EmailId = obj.EMAILID;

                    model.USERPC = obj.USERPC;
                    model.INSIPNO = obj.INSIPNO;
                    model.INSLTUDE = obj.INSLTUDE;
                    model.INSTIME = obj.INSTIME;
                    model.INSUSERID = obj.INSUSERID;
                    Insert_MgtLogData(model);

                    db.SaveChanges();

                    yield return new ManagementDTO
                    {

                        COMPID = compid,
                        ManagementName = ManagementName,

                        ManagementId = Convert.ToInt64(Convert.ToString(compid) + "101"),
                        Designation = Designation,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        ReferId = 0

                    };


                }
                else
                {
                    Management obj = new Management
                    {
                        COMPID = compid,

                        MGTID = max_idta_berkorbo + 1,
                        MGTNM = ManagementName,
                        DESIG = Designation,
                        ADDRESS = Address,
                        MOBNO1 = MobileNo1,
                        MOBNO2 = MobileNo2,
                        EMAILID = EmailId,
                        USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),
                        INSLTUDE = Insltude,
                        INSTIME = Convert.ToDateTime(td),
                        INSUSERID = Convert.ToInt64(Insuserid)

                    };

                    db.HMS_MGT.Add(obj);

                    model.ID = obj.ID;
                    model.COMPID = obj.COMPID;
                    model.ManagementId = obj.MGTID;
                    model.ManagementName = obj.MGTNM;
                    model.Designation = obj.DESIG;
                    model.Address = obj.ADDRESS;

                    model.MobileNo1 = obj.MOBNO1;
                    model.MobileNo2 = obj.MOBNO2;
                    model.EmailId = obj.EMAILID;

                    model.USERPC = obj.USERPC;
                    model.INSIPNO = obj.INSIPNO;
                    model.INSLTUDE = obj.INSLTUDE;
                    model.INSTIME = obj.INSTIME;
                    model.INSUSERID = obj.INSUSERID;
                     Insert_MgtLogData(model);
                    db.SaveChanges();

                    yield return new ManagementDTO
                    {
                        COMPID = compid,
                        ManagementName = ManagementName,

                        ManagementId = max_idta_berkorbo + 1,
                        Designation = Designation,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        ReferId = 0


                    };



                }



            }
            else
            {
                var ff = (from n in db.HMS_MGTRF where n.COMPID == compid && n.MGTID == mgtid select n).ToList();

                if (ff.Count == 0)
                {

                    yield return new ManagementDTO
                    {

                        COMPID = compid,
                        ManagementName = ManagementName,

                        ManagementId = mgtid,
                        Designation = Designation,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        ReferId = 0


                    };

                }
                else
                {



                    var referDto = (from t1 in db.HMS_MGTRF
                                    join t2 in db.HMS_REFER on t1.COMPID equals t2.COMPID
                                    where t1.REFERID == t2.REFERID && t1.MGTID == mgtid
                                    select new
                                    {
                                        Id = t1.ID,

                                        Referid = t1.REFERID,
                                        refername = t2.REFERNM,

                                        Refpcnt = t1.RFPCNT,
                                        Remarks = t1.REMARKS


                                    });
                    foreach (var item in referDto)
                    {
                        yield return new ManagementDTO
                        {
                            ID = item.Id,
                            COMPID = compid,
                            ReferId = item.Referid,
                            ManagementId = mgtid,
                            Refername = item.refername,
                            ReferPCNT = item.Refpcnt,
                            Remarks = item.Remarks
                        };
                    }



                }
            }

        }
        public void Insert_MGTRFLogData(ManagementDTO model)
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


            var refername_find =
                (from n in db.HMS_REFER where n.COMPID == model.COMPID && n.REFERID == model.ReferId select n).ToList();

            string refername = "";
            foreach (var refer in refername_find)
            {
                refername = refer.REFERNM;
            }


            var managementname_find =
                (from n in db.HMS_MGT where n.COMPID == model.COMPID && n.MGTID == model.ManagementId select n).ToList();

            string mgtname = "";
            foreach (var mgt in managementname_find)
            {
                mgtname = mgt.MGTNM;
            }
            aslLog.LOGDATA = Convert.ToString("Management Name: " + mgtname + ",\nRefer Name: " + refername + ",\nRF(%): " + model.ReferPCNT + ",\nRemarks: " + model.Remarks);
            aslLog.TABLEID = "HMS_MGTRF";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }

        [Route("api/grid/MgtChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(ManagementDTO model)
        {

            var check_data = (from n in db.HMS_MGTRF where n.COMPID == model.COMPID && n.REFERID == model.ReferId && n.MGTID == model.ManagementId select n).ToList();
            if (check_data.Count == 0)
            {



                ManagementRF childData = new ManagementRF();
                childData.COMPID = model.COMPID;
                childData.MGTID = model.ManagementId;

                childData.REFERID = model.ReferId;
                childData.RFPCNT = model.ReferPCNT;
                childData.REMARKS = model.Remarks;

                childData.USERPC = strHostName;
                childData.INSIPNO = ipAddress.ToString();
                childData.INSLTUDE = model.INSLTUDE;
                childData.INSTIME = Convert.ToDateTime(td);
                childData.INSUSERID = model.INSUSERID;

                db.HMS_MGTRF.Add(childData);

                model.ID = childData.ID;
                model.COMPID = model.COMPID;
                model.ReferId = childData.REFERID;
                model.ManagementId = childData.MGTID;

                model.ReferPCNT = childData.RFPCNT;
                model.Remarks = childData.REMARKS;
                model.USERPC = strHostName;
                model.INSIPNO = ipAddress.ToString();
                model.INSLTUDE = model.INSLTUDE;
                model.INSTIME = Convert.ToDateTime(td);
                model.INSUSERID = model.INSUSERID;

                Insert_MGTRFLogData(model);

                db.SaveChanges();

               







                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.ReferId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }

        [Route("api/ApiMgt/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(ManagementDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_MGTRF where n.COMPID == model.COMPID && n.MGTID == model.ManagementId && n.REFERID == model.ReferId select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_MGTRF where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.REFERID = model.ReferId;
                        item.MGTID = Convert.ToInt64(model.ManagementId);
                        item.RFPCNT = model.ReferPCNT;
                        item.REMARKS = model.Remarks;


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
                    model.ReferId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                ManagementRF managementRf = new ManagementRF();

                managementRf.ID = model.ID;
                managementRf.COMPID = model.COMPID;
                managementRf.REFERID = model.ReferId;
                managementRf.MGTID = Convert.ToInt64(model.ManagementId);
                managementRf.RFPCNT = model.ReferPCNT;
                managementRf.REMARKS = model.Remarks;



                db.Entry(managementRf).State = EntityState.Modified;

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

        [Route("api/ApiMgt/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(ManagementDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_MGTRF WHERE ID='{0}'", model.ID);
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
    }
}
