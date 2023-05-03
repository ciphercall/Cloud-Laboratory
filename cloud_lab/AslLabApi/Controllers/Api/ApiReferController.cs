using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslLabApi.Models;
using AslLabApi.Models.DTO;
using AslLabApi.Models.HMS;
using AslLabApi.Models.GL;
using System.Data.Entity;

namespace AslLabApi.Controllers.Api
{
    public class ApiReferController : ApiController
    {


        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiReferController()
        {

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];

            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
        AslLabApiContext db = new AslLabApiContext();

        [Route("api/ReferList")]
        [HttpGet]
        public IEnumerable<ReferDTO> GetReferList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                return String.IsNullOrEmpty(query) ? db.HMS_REFER.AsEnumerable().Select(b => new ReferDTO { }).ToList() :
                db.HMS_REFER.Where(p => p.REFERNM.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              refername = x.REFERNM,
                              referid = x.REFERID,
                              refergid = x.REFERGID,
                              title = x.TITLE,
                              address = x.ADDRESS,
                              mobileno1 = x.MOBNO1,
                              mobileno2 = x.MOBNO2,
                              emailid = x.EMAILID
                          })
                .AsEnumerable().Select(a => new ReferDTO
                {
                    ReferName = a.refername,
                    ReferId = Convert.ToInt64(a.referid),
                    ReferGroupId = a.refergid,
                    Title = a.title,
                    Address = a.address,
                    MobileNo1 = a.mobileno1,
                    MobileNo2 = a.mobileno2,
                    EmailId = a.emailid
                }).ToList();
            }
        }

        [System.Web.Http.Route("api/DynamicRefer")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ReferDTO> Dynamicautocomplete(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);

                String name = "";

                var rt = db.HMS_REFER.Where(n => n.REFERNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                {
                    headname = n.REFERNM

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



                var findid = (from n in db.HMS_REFER where n.REFERNM == name && n.COMPID == compid select n).ToList();
                if (findid.Count != 0)
                {
                    return db.HMS_REFER.Where(p => p.REFERNM == name && p.COMPID == compid).Select(
                        x =>
                            new
                            {
                                refername = x.REFERNM,
                                referid = x.REFERID,
                                refergid = x.REFERGID,
                                title = x.TITLE,
                                address = x.ADDRESS,
                                mobileno1 = x.MOBNO1,
                                mobileno2 = x.MOBNO2,
                                emailid = x.EMAILID

                            })
                        .AsEnumerable().Select(a => new ReferDTO
                        {

                            ReferName = a.refername,
                            ReferId = Convert.ToInt64(a.referid),
                            ReferGroupId = a.refergid,
                            Title = a.title,
                            Address = a.address,
                            MobileNo1 = a.mobileno1,
                            MobileNo2 = a.mobileno2,
                            EmailId = a.emailid

                        }).ToList();
                }
                else
                {
                    return db.HMS_REFER.AsEnumerable().Select(a => new ReferDTO
                    {
                        ReferName = name,
                        ReferId = 0,
                        ReferGroupId = 0

                    }).ToList();
                }



            }

        }



        [System.Web.Http.Route("api/ApiRefer/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ReferDTO> Get(string Compid, string ReferGroupId, string ReferId, string ReferName, string Title, string Address, string MobileNo1, string MobileNo2, string EmailId)
        {

            Int64 compid = Convert.ToInt64(Compid);
            Int64 referGroupid = Convert.ToInt64(ReferGroupId);
            Int64 referId = Convert.ToInt64(ReferId);

            var ache_kina_data = (from n in db.HMS_REFER where n.COMPID == compid && n.REFERGID == referGroupid && n.REFERID == referId && n.REFERNM == ReferName select n).ToList();
            if (ache_kina_data.Count == 0)
            {


                Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.HMS_REFER where n.COMPID == compid select n.REFERID).Max());
                Int64 headtp = 2, hlevel = 5;
                if (max_idta_berkorbo == 0)
                {

                    Refer obj = new Refer
                    {
                        COMPID = compid,
                        REFERGID = Convert.ToInt64(Convert.ToString(compid) + "203"),
                        REFERID = Convert.ToInt64(Convert.ToString(compid) + "2030001"),
                        REFERNM = ReferName,
                        TITLE = Title,
                        ADDRESS = Address,
                        MOBNO1 = MobileNo1,
                        MOBNO2 = MobileNo2,
                        EMAILID = EmailId,


                    };
                    //Int64 seacrh_maxaccid = 0;
                    //Int64 controlcode = Convert.ToInt64(Convert.ToString(compid) + "202010200000");
                    //try
                    //{
                    //    seacrh_maxaccid = Convert.ToInt64((from n in db.GlAcchartDbSet where n.COMPID == compid && n.HEADTP == headtp && n.HLEVELCD == hlevel && n.CONTROLCD == controlcode select n.ACCOUNTCD).Max());
                    //}
                    //catch
                    //{
                    //    seacrh_maxaccid = 0;
                    //}

                    GL_ACCHART account = new GL_ACCHART();


                    account.COMPID = compid;
                    account.HEADTP = 2;
                    account.HEADCD = Convert.ToInt64(Convert.ToString(compid) + "203");
                    account.ACCOUNTCD = Convert.ToInt64(Convert.ToString(compid) + "2030001");
                    account.ACCOUNTNM = ReferName;



                    account.USERPC = strHostName;
                    account.INSIPNO = ipAddress.ToString();
                    account.INSTIME = Convert.ToDateTime(td);






                    bool value = false;
                    try
                    {
                        db.HMS_REFER.Add(obj);
                        db.GlAcchartDbSet.Add(account);
                        db.SaveChanges();
                        value = true;
                    }
                    catch (Exception)
                    {
                        value = false;
                    }

                    yield return new ReferDTO
                    {

                        COMPID = compid,
                        ReferName = ReferName,
                        ReferGroupId = Convert.ToInt64(Convert.ToString(compid) + "203"),
                        ReferId = Convert.ToInt64(Convert.ToString(compid) + "2030001"),
                        Title = Title,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        TCatId = 0,
                        errorStatus = value

                    };


                }
                else
                {
                    Refer obj = new Refer
                    {
                        COMPID = compid,
                        REFERGID = Convert.ToInt64(Convert.ToString(compid) + "203"),
                        REFERID = max_idta_berkorbo + 1,
                        REFERNM = ReferName,
                        TITLE = Title,
                        ADDRESS = Address,
                        MOBNO1 = MobileNo1,
                        MOBNO2 = MobileNo2,
                        EMAILID = EmailId

                    };
                
                    GL_ACCHART account = new GL_ACCHART
                    {
                        COMPID = compid,
                        HEADTP = 2,
                        HEADCD = Convert.ToInt64(Convert.ToString(compid) + "203"),
                        ACCOUNTCD = max_idta_berkorbo + 1,
                      
                        ACCOUNTNM = ReferName,
                       
                      
                        USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),
                        INSTIME = Convert.ToDateTime(td),
                    };
                    bool value = false;

                    try
                    {
                        db.HMS_REFER.Add(obj);
                        db.GlAcchartDbSet.Add(account);
                        db.SaveChanges();
                        value = true;
                    }
                    catch (Exception)
                    {
                        value = false;
                    }


                    yield return new ReferDTO
                    {
                        COMPID = compid,
                        ReferName = ReferName,
                        ReferGroupId = Convert.ToInt64(Convert.ToString(compid) + "203"),
                        ReferId = max_idta_berkorbo + 1,
                        Title = Title,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        TCatId = 0,
                        errorStatus = value


                    };



                }



            }
            else
            {
                var ff = (from n in db.HMS_RFPCNT where n.COMPID == compid && n.REFERID == referId select n).ToList();

                if (ff.Count == 0)
                {

                    yield return new ReferDTO
                    {

                        COMPID = compid,
                        ReferName = ReferName,
                        ReferGroupId = referGroupid,
                        ReferId = referId,
                        Title = Title,
                        Address = Address,
                        MobileNo1 = MobileNo1,
                        MobileNo2 = MobileNo2,
                        EmailId = EmailId,
                        TCatId = 0,
                        errorStatus = true


                    };

                }
                else
                {



                    var referDto = (from t1 in db.HMS_RFPCNT
                                    join t2 in db.HMS_TESTMST on t1.TCATID equals t2.TCATID
                                    where t1.COMPID == t2.COMPID && t1.REFERID == referId
                                    select new
                                    {
                                        Id = t1.ID,

                                        Referid = t1.REFERID,
                                        TCatId = t1.TCATID,
                                        Tcatname = t2.TCATNM,
                                        Refpcnt = t1.RFPCNT,
                                        Remarks = t1.REMARKS


                                    });
                    foreach (var item in referDto)
                    {
                        yield return new ReferDTO
                        {
                            ID = item.Id,
                            COMPID = compid,
                            ReferId = item.Referid,
                            ReferGroupId = referGroupid,
                            TCatId = item.TCatId,
                            TCatname = item.Tcatname,
                            ReferPCNT = item.Refpcnt,
                            Remarks = item.Remarks,
                            errorStatus = true
                        };
                    }



                }
            }

        }


        [Route("api/grid/ReferChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(ReferDTO model)
        {

            var check_data = (from n in db.HMS_RFPCNT where n.COMPID == model.COMPID && n.REFERID == model.ReferId && n.TCATID == model.TCatId select n).ToList();
            if (check_data.Count == 0)
            {



                RefPersonContact childData = new RefPersonContact();
                childData.COMPID = model.COMPID;
                childData.TCATID = model.TCatId;

                childData.REFERID = model.ReferId;
                childData.RFPCNT = model.ReferPCNT;
                childData.REMARKS = model.Remarks;

                childData.USERPC = strHostName;
                childData.INSIPNO = ipAddress.ToString();
                childData.INSLTUDE = model.INSLTUDE;
                childData.INSUSERID = model.INSUSERID;
                childData.INSTIME = Convert.ToDateTime(td);


                db.HMS_RFPCNT.Add(childData);

                db.SaveChanges();

                model.ID = childData.ID;
                model.COMPID = model.COMPID;
                model.ReferId = childData.REFERID;
                model.TCatId = childData.TCATID;

                model.ReferPCNT = childData.RFPCNT;
                model.Remarks = childData.REMARKS;







                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.TCatId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }

        [Route("api/ApiRefer/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(ReferDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_RFPCNT where n.COMPID == model.COMPID && n.TCATID == model.TCatId && n.REFERID == model.ReferId select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_RFPCNT where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.REFERID = model.ReferId;
                        item.TCATID = Convert.ToInt64(model.TCatId);
                        item.RFPCNT = model.ReferPCNT;
                        item.REMARKS = model.Remarks;

                        item.USERPC = item.USERPC;
                        item.INSIPNO = item.INSIPNO;
                        item.INSLTUDE = item.INSLTUDE;
                        item.INSUSERID = item.INSUSERID;
                        item.INSTIME = item.INSTIME;

                        item.UPDIPNO = ipAddress.ToString();
                        item.UPDLTUDE = model.INSLTUDE;
                        item.UPDUSERID = model.INSUSERID;
                        item.UPDTIME = Convert.ToDateTime(td);

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
                    model.TCatId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                RefPersonContact referp = new RefPersonContact();

                referp.ID = model.ID;
                referp.COMPID = model.COMPID;
                referp.REFERID = model.ReferId;
                referp.TCATID = Convert.ToInt64(model.TCatId);
                referp.RFPCNT = model.ReferPCNT;
                referp.REMARKS = model.Remarks;

                referp.USERPC = strHostName;


                referp.UPDIPNO = ipAddress.ToString();
                referp.UPDLTUDE = model.INSLTUDE;
                referp.UPDUSERID = model.INSUSERID;
                referp.UPDTIME = Convert.ToDateTime(td);

                db.Entry(referp).State = EntityState.Modified;

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

        [Route("api/ApiRefer/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(ReferDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_REFER WHERE ID='{0}'", model.ID);
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



        [System.Web.Http.Route("api/ApiRefer/GetData2")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ReferDTO> Get2(string Compid)
        {

            Int64 compid = Convert.ToInt64(Compid);


            var ache_kina_data = (from n in db.HMS_REFER where n.COMPID == compid select n).ToList();
            if (ache_kina_data.Count == 0)
            {

                yield return new ReferDTO
                {

                    COMPID = compid,
                    ReferId = 0


                };




            }
            else
            {

                foreach (var item in ache_kina_data)
                {
                    yield return new ReferDTO
                    {
                        ID = item.ID,
                        COMPID = compid,
                        ReferId = item.REFERID,
                        ReferGroupId = item.REFERGID,
                        ReferName = item.REFERNM,
                        Title = item.TITLE,
                        Address = item.ADDRESS,
                        MobileNo1 = item.MOBNO1,
                        MobileNo2 = item.MOBNO2,
                        EmailId = item.EMAILID

                    };
                }



            }
        }



        public ASL_LOG aslLog = new ASL_LOG();
        public void Update_ReferLogData(ReferDTO model)
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




            aslLog.LOGDATA = Convert.ToString("Refer Name: " + model.ReferName + ",\nTitle: " + model.Title + "\nAddress: " + model.Address + "\nMobile No1: " + model.MobileNo1 + "\nMobile No2: " + model.MobileNo2 + "\nEmailID: " +
                model.EmailId);
            aslLog.TABLEID = "HMS_REFER";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
        }


        //Update Part
        [Route("api/ApiRefer/SaveData2")]
        [HttpPost]
        public HttpResponseMessage SaveData2(ReferDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.HMS_REFER where n.COMPID == model.COMPID && n.REFERNM == model.ReferName select n).ToList();

            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.ID)
                {

                    var data_find = (from n in db.HMS_REFER where n.ID == model.ID select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.REFERID = item.REFERID;
                        item.REFERGID = item.REFERGID;
                        item.REFERNM = model.ReferName;
                        item.TITLE = model.Title;
                        item.ADDRESS = model.Address;
                        item.MOBNO1 = model.MobileNo1;
                        item.MOBNO2 = model.MobileNo2;
                        item.EMAILID = model.EmailId;

                        item.USERPC = item.USERPC;
                        item.INSIPNO = item.INSIPNO;
                        item.INSLTUDE = item.INSLTUDE;
                        item.INSTIME = item.INSTIME;
                        item.INSUSERID = item.INSUSERID;

                        item.UPDIPNO = ipAddress.ToString();
                        item.UPDLTUDE = model.UPDLTUDE;
                        item.UPDTIME = Convert.ToDateTime(td);
                        item.UPDUSERID = model.UPDUSERID;

                        model.UPDIPNO = item.UPDIPNO;
                        model.USERPC = item.USERPC;
                        model.UPDTIME = item.UPDTIME;
                        model.UPDUSERID = item.UPDUSERID;

                        Update_ReferLogData(model);


                    }


                    var data_find_account =
                (from n in db.GlAcchartDbSet
                 where n.COMPID == model.COMPID && n.ACCOUNTCD == model.ReferId
                 select n).ToList();


                    foreach (var item2 in data_find_account)
                    {
                        item2.ACCHARTId = item2.ACCHARTId;
                        item2.COMPID = model.COMPID;
                        item2.HEADTP = item2.HEADTP;
                        item2.HEADCD = item2.HEADCD;
                        item2.ACCOUNTCD = item2.ACCOUNTCD;
                     
                        item2.ACCOUNTNM = model.ReferName;
                       

                        item2.USERPC = strHostName;

                        item2.INSIPNO = item2.INSIPNO;
                        item2.INSLTUDE = item2.INSLTUDE;
                        item2.INSTIME = item2.INSTIME;
                        item2.INSUSERID = item2.INSUSERID;


                        item2.UPDIPNO = ipAddress.ToString();
                        item2.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                        item2.UPDTIME = Convert.ToDateTime(td);
                        item2.UPDUSERID = Convert.ToInt64(model.INSUSERID);

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
                    model.TCatId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

            }
            else
            {
                var find_row = (from n in db.HMS_REFER where n.COMPID == model.COMPID && n.ID == model.ID select n).ToList();

                foreach (var item in find_row)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.REFERID = item.REFERID;
                    item.REFERGID = item.REFERGID;
                    item.REFERNM = model.ReferName;
                    item.TITLE = model.Title;
                    item.ADDRESS = model.Address;
                    item.MOBNO1 = model.MobileNo1;
                    item.MOBNO2 = model.MobileNo2;
                    item.EMAILID = model.EmailId;

                    item.USERPC = item.USERPC;
                    item.INSIPNO = item.INSIPNO;
                    item.INSLTUDE = item.INSLTUDE;
                    item.INSTIME = item.INSTIME;
                    item.INSUSERID = item.INSUSERID;

                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = model.UPDLTUDE;
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = model.UPDUSERID;

                    model.UPDIPNO = item.UPDIPNO;
                    model.USERPC = item.USERPC;
                    model.UPDTIME = item.UPDTIME;
                    model.UPDUSERID = item.UPDUSERID;

                    Update_ReferLogData(model);
                    db.Entry(item).State = EntityState.Modified;
                }

                var data_find_account =
              (from n in db.GlAcchartDbSet
               where n.COMPID == model.COMPID && n.ACCOUNTCD == model.ReferId
               select n).ToList();


                foreach (var item2 in data_find_account)
                {
                    item2.ACCHARTId = item2.ACCHARTId;
                    item2.COMPID = model.COMPID;
                    item2.HEADTP = item2.HEADTP;
                    item2.HEADCD = item2.HEADCD;
                    item2.ACCOUNTCD = item2.ACCOUNTCD;
                   
                    item2.ACCOUNTNM = model.ReferName;
                    

                    item2.USERPC = strHostName;

                    item2.INSIPNO = item2.INSIPNO;
                    item2.INSLTUDE = item2.INSLTUDE;
                    item2.INSTIME = item2.INSTIME;
                    item2.INSUSERID = item2.INSUSERID;


                    item2.UPDIPNO = ipAddress.ToString();
                    item2.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item2.UPDTIME = Convert.ToDateTime(td);
                    item2.UPDUSERID = Convert.ToInt64(model.INSUSERID);

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

