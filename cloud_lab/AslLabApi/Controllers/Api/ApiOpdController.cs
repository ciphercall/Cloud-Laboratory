using System;
using System.Collections.Generic;
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
    public class ApiOpdController : ApiController
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        AslLabApiContext db = new AslLabApiContext();

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiOpdController()
        {
           
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }

        [System.Web.Http.Route("api/TestNameList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> GetTestNameList(string query, string query2, string query3)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                Int64 catid = Convert.ToInt64(query3);
                if (catid == 0)
                {
                    return String.IsNullOrEmpty(query) ? db.HMS_TEST.AsEnumerable().Select(b => new OpdDTO { }).ToList() :
               db.HMS_TEST.Where(p => p.TESTNM.StartsWith(query) && p.COMPID == compid).Select(
                         x =>
                         new
                         {
                             testname = x.TESTNM,
                             testid = x.TESTID,
                             rate = x.RATE,
                             pcntd = x.PCNTD,
                             tCatid = x.TCATID
                         })
               .AsEnumerable().Select(a => new OpdDTO
               {
                   TestName = a.testname,
                   TestID = Convert.ToInt64(a.testid),
                   TestCategoryId = a.tCatid,
                   TestCategoryName = "",
                   Amount = a.rate,
                   Pcntd = a.pcntd

               }).ToList();
                }
                else
                {


                    return (from t1 in db.HMS_TEST
                            join t2 in db.HMS_TESTMST on t1.TCATID equals t2.TCATID
                            where t1.COMPID == compid && t2.COMPID == compid && t1.TCATID == catid && t1.TESTNM.StartsWith(query)
                            select new
                            {
                                testname = t1.TESTNM,
                                testid = t1.TESTID,
                                categoryname = t2.TCATNM,
                                rate = t1.RATE,
                                pcntd = t1.PCNTD


                            })
                           .AsEnumerable().Select(a => new OpdDTO
                           {

                               TestName = a.testname,
                               TestID = Convert.ToInt64(a.testid),
                               TestCategoryName = a.categoryname,
                               Amount = a.rate,
                               Pcntd = a.pcntd


                           }).ToList();
                }
            }
        }

        [System.Web.Http.Route("api/DynamicTestName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> Dynamicautocomplete(string changedText, string changedText2, string changedText3)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                Int64 catid = Convert.ToInt64(changedText3);

                String name = "";
                var rt = db.HMS_TEST.Where(n => n.TESTNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                {
                    headname = ""

                }).ToList();//This for using initialization, I had no way at that time

                if (catid == 0)
                {
                    rt = db.HMS_TEST.Where(n => n.TESTNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
                   {
                       headname = n.TESTNM

                   }).ToList();
                }
                else
                {
                    rt = db.HMS_TEST.Where(n => n.TESTNM.StartsWith(changedText) && n.COMPID == compid && n.TCATID == catid).Select(n => new
                   {
                       headname = n.TESTNM

                   }).ToList();
                }



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

                var findid = (from n in db.HMS_TEST where n.TESTNM == name && n.COMPID == compid && n.TCATID == catid select n).ToList();
                if (catid == 0)
                {
                    findid = (from n in db.HMS_TEST where n.TESTNM == name && n.COMPID == compid select n).ToList();
                }
                else
                {
                    findid = (from n in db.HMS_TEST where n.TESTNM == name && n.COMPID == compid && n.TCATID == catid select n).ToList();
                }

                if (findid.Count != 0)
                {
                    Int64 TCatid = Convert.ToInt64(findid[0].TCATID);


                    return (from t1 in db.HMS_TEST
                            join t2 in db.HMS_TESTMST on t1.TCATID equals t2.TCATID
                            where t1.COMPID == compid && t2.COMPID == compid && t1.TCATID == TCatid && t1.TESTNM == name
                            select new
                            {
                                testname = t1.TESTNM,
                                testid = t1.TESTID,
                                categoryname = t2.TCATNM,
                                rate = t1.RATE,
                                pcntd = t1.PCNTD


                            })
                   .AsEnumerable().Select(a => new OpdDTO
                   {

                       TestName = a.testname,
                       TestID = Convert.ToInt64(a.testid),
                       TestCategoryName = a.categoryname,

                       TestCategoryId = TCatid,
                       Amount = a.rate,
                       Pcntd = a.pcntd


                   }).ToList();


                }
                else
                {
                    return db.HMS_TEST.AsEnumerable().Select(a => new OpdDTO
                    {
                        TestName = name,
                        TestID = 0


                    }).ToList();
                }



            }

        }

        [System.Web.Http.Route("api/ApiOpd/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> Get(string Compid, string TransactionDate, string PatientName, string RfPercentage, string PatienId, string InUserID, string InsLtude)
        {

            //Get Ip ADDRESS,Time & user PC Name
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Int64 InsUserID = Convert.ToInt64(InUserID);
            Int64 compid = Convert.ToInt64(Compid);

            DateTime fdate = DateTime.Parse(TransactionDate);
            string ffdt = fdate.ToString("ddMMyy");

            decimal Rfpercentage = Convert.ToDecimal(RfPercentage);


            var ache_kina_data = (from n in db.HMS_OPDMST where n.COMPID == compid && n.TRANSDT == fdate && n.PATIENTID == PatienId select n).ToList();
            if (ache_kina_data.Count == 0)
            {
                var max_patientID =
                  (from n in db.HMS_OPDMST where n.COMPID == compid && n.TRANSDT == fdate select n.PATIENTID).Max();
                if (max_patientID == null)
                {
                    string converttoString = Convert.ToString(fdate.ToString("dd-MMM-yyyy"));
                    string getYear = converttoString.Substring(9, 2);
                    string getMonth = converttoString.Substring(3, 3);
                    string Month = getMonth + "-" + getYear;


                    string converttoString1 = Convert.ToString(fdate.ToString("dd-MM-yyyy"));
                    string getyear = converttoString1.Substring(6, 4);
                    string getmonth = converttoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    var max = Convert.ToInt64((from n in db.HMS_OPDMST where n.COMPID == compid select n.TRANSNO).Max());
                    if (max == 0)
                    {
                        max = Convert.ToInt64(halftransno + "0001");
                    }
                    else
                    {
                        max = max + 1;
                    }
                    OpdMaster obj = new OpdMaster
                    {
                        COMPID = compid,
                        TRANSDT = fdate,
                        PATIENTID = ffdt + "-" + "001",
                        PATIENTNM = PatientName,
                        RFPCNT = Rfpercentage,

                        USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),

                        INSTIME = Convert.ToDateTime(td),

                        INSUSERID = InsUserID,
                        INSLTUDE = InsLtude,
                        TRANSMY = Month,
                        TRANSNO = max

                    };

                    db.HMS_OPDMST.Add(obj);
                    db.SaveChanges();
                    yield return new OpdDTO
                    {

                        CompanyID = compid,
                        TransactionDate = fdate,
                        PatientID = ffdt + "-" + "001",
                        PatientName = PatientName,
                        RfPercentage = Rfpercentage,
                        TestSerial = 0

                    };
                }
                else
                {
                    Int64 abc = Convert.ToInt64(max_patientID.Substring(7, 3)) + 1;
                    string xxx = Convert.ToString(abc);
                    string patienid = "";
                    if (xxx.Length == 1)
                    {
                        patienid = max_patientID.Substring(0, 7) + "00" + xxx;
                    }
                    else if (xxx.Length == 2)
                    {
                        patienid = max_patientID.Substring(0, 7) + "0" + xxx;
                    }
                    else if (xxx.Length == 3)
                    {
                        patienid = max_patientID.Substring(0, 7) + xxx;
                    }

                    string converttoString = Convert.ToString(fdate.ToString("dd-MMM-yyyy"));
                    string getYear = converttoString.Substring(9, 2);
                    string getMonth = converttoString.Substring(3, 3);
                    string Month = getMonth + "-" + getYear;


                    string converttoString1 = Convert.ToString(fdate.ToString("dd-MM-yyyy"));
                    string getyear = converttoString1.Substring(6, 4);
                    string getmonth = converttoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    var max = Convert.ToInt64((from n in db.HMS_OPDMST where n.COMPID == compid select n.TRANSNO).Max());
                    if (max == 0)
                    {
                        max = Convert.ToInt64(halftransno + "0001");
                    }
                    else
                    {
                        max = max + 1;
                    }

                    OpdMaster obj = new OpdMaster
                    {
                        COMPID = compid,
                        TRANSDT = fdate,
                        PATIENTID = patienid,
                        PATIENTNM = PatientName,
                        RFPCNT = Rfpercentage,

                         USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),
                        INSTIME = Convert.ToDateTime(td),

                        INSUSERID = InsUserID,
                        INSLTUDE = InsLtude,
                        TRANSMY = Month,
                        TRANSNO = max

                    };

                    db.HMS_OPDMST.Add(obj);
                    db.SaveChanges();

                    yield return new OpdDTO
                    {

                        CompanyID = compid,
                        TransactionDate = fdate,
                        PatientID = patienid,
                        PatientName = PatientName,
                        RfPercentage = Rfpercentage,
                        TestSerial = 0

                    };
                }












            }
            else
            {
                if (PatienId == null)
                {
                    var max_patientID =
                   (from n in db.HMS_OPDMST where n.COMPID == compid && n.TRANSDT == fdate select n.PATIENTID).Max();
                    Int64 abc = Convert.ToInt64(max_patientID.Substring(7, 3)) + 1;
                    string xxx = Convert.ToString(abc);
                    string patienid = "";
                    if (xxx.Length == 1)
                    {
                        patienid = max_patientID.Substring(0, 7) + "00" + xxx;
                    }
                    else if (xxx.Length == 2)
                    {
                        patienid = max_patientID.Substring(0, 7) + "0" + xxx;
                    }
                    else if (xxx.Length == 3)
                    {
                        patienid = max_patientID.Substring(0, 7) + xxx;
                    }

                    string converttoString = Convert.ToString(fdate.ToString("dd-MMM-yyyy"));
                    string getYear = converttoString.Substring(9, 2);
                    string getMonth = converttoString.Substring(3, 3);
                    string Month = getMonth + "-" + getYear;


                    string converttoString1 = Convert.ToString(fdate.ToString("dd-MM-yyyy"));
                    string getyear = converttoString1.Substring(6, 4);
                    string getmonth = converttoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    var max = Convert.ToInt64((from n in db.HMS_OPDMST where n.COMPID == compid select n.TRANSNO).Max());
                    if (max == 0)
                    {
                        max = Convert.ToInt64(halftransno + "0001");
                    }
                    else
                    {
                        max = max + 1;
                    }


                    OpdMaster obj = new OpdMaster
                    {
                        COMPID = compid,
                        TRANSDT = fdate,
                        PATIENTID = patienid,
                        PATIENTNM = PatientName,
                        RFPCNT = Rfpercentage,


                         USERPC = strHostName,
                        INSIPNO = ipAddress.ToString(),
                        INSTIME = Convert.ToDateTime(td),

                        INSUSERID = InsUserID,
                        INSLTUDE = InsLtude,
                        TRANSMY = Month,
                        TRANSNO = max
                    };

                    db.HMS_OPDMST.Add(obj);
                    db.SaveChanges();

                    yield return new OpdDTO
                    {

                        CompanyID = compid,
                        TransactionDate = fdate,
                        PatientID = patienid,
                        PatientName = PatientName,
                        RfPercentage = Rfpercentage,
                        TestSerial = 0

                    };
                }
                else
                {
                    var ff = (from n in db.HMS_OPD where n.COMPID == compid && n.TRANSDT == fdate && n.PATIENTID == PatienId select n).ToList();
                    if (ff.Count == 0)
                    {

                        yield return new OpdDTO
                        {

                            CompanyID = compid,
                            TransactionDate = fdate,
                            PatientID = PatienId,
                            PatientName = PatientName,
                            RfPercentage = Rfpercentage,
                            TestSerial = 0


                        };

                    }
                    else
                    {

                        var opd_data = (from t1 in db.HMS_OPD
                                        join t2 in db.HMS_OPDMST on t1.PATIENTID equals t2.PATIENTID
                                        join t3 in db.HMS_TEST on t1.TESTID equals t3.TESTID
                                        join t4 in db.HMS_TESTMST on t1.TCATID equals t4.TCATID
                                        where t1.COMPID == compid && t1.COMPID == t2.COMPID && t1.TRANSDT == fdate && t1.PATIENTID == PatienId
                                        select new
                                        {
                                            Id = t1.ID,

                                            compid = t1.COMPID,
                                            Transdate = t1.TRANSDT,
                                            Patientid = t1.PATIENTID,
                                            referid = t1.REFERID,
                                            testsl = t1.TESTSL,
                                            tcatid = t1.TCATID,
                                            tCatname = t4.TCATNM,
                                            testid = t1.TESTID,
                                            testname = t3.TESTNM,
                                            amount = t1.AMOUNT,
                                            pcntr = t1.PCNTR,
                                            pcntd = t1.PCNTD,
                                            discr = t1.DISCR,
                                            remarks = t1.REMARKS


                                        });

                        foreach (var item in opd_data)
                        {

                            yield return new OpdDTO
                            {
                                ID = item.Id,
                                CompanyID = item.compid,
                                PatientName = PatientName,
                                TransactionDate = item.Transdate,
                                PatientID = item.Patientid,
                                ReferID = item.referid,
                                TestSerial = item.testsl,
                                TestCategoryId = item.tcatid,
                                TestCategoryName = item.tCatname,
                                TestID = item.testid,
                                TestName = item.testname,
                                Amount = item.amount,
                                Pcntd = item.pcntd,
                                Pcntr = item.pcntr,
                                Discr = item.discr,
                                Remarks = item.remarks

                            };
                        }
                    }
                }




            }


        }


        [System.Web.Http.Route("api/PatientNameList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> GetPatientNameList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
                //DateTime Tdate = Convert.ToDateTime(query3);

                return String.IsNullOrEmpty(query) ? db.HMS_OPDMST.AsEnumerable().Select(b => new OpdDTO { }).ToList() :
                db.HMS_OPDMST.Where(p => p.PATIENTID.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              patientname = x.PATIENTNM,
                              patientid = x.PATIENTID

                              //dueamount = x.DUEAMT

                          })
                .AsEnumerable().Select(a => new OpdDTO
                {
                    PatientName = a.patientname,
                    PatientID = a.patientid

                    //DueAmount = Convert.ToDecimal(a.dueamount)

                }).ToList();
            }
        }

        [System.Web.Http.Route("api/DynamicPatientName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> DynamicAutocomplete(string changedText, string changedText2)
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
                    decimal discount = 0, paid = 0;
                    var due_find = (from n in db.HMS_OPDRCV where n.COMPID == compid && n.PATIENTID == name select n);

                    if (due_find.Count() != 0)
                    {
                        foreach (var item in due_find)
                        {
                            discount = Convert.ToDecimal(discount + item.DISCHOS);
                            paid = Convert.ToDecimal(paid + item.RCVAMT);
                        }
                    }
                    return db.HMS_OPDMST.Where(p => p.PATIENTID == name && p.COMPID == compid).Select(
                        x =>
                            new
                            {
                                patientname = x.PATIENTNM,
                                patientid = x.PATIENTID,
                                dueamount = x.DUEAMT - discount - paid,


                            })
                        .AsEnumerable().Select(a => new OpdDTO
                        {

                            PatientName = a.patientname,
                            PatientID = a.patientid,
                            DueAmount = Convert.ToDecimal(a.dueamount)



                        }).ToList();
                }
                else
                {
                    return db.HMS_OPDMST.AsEnumerable().Select(a => new OpdDTO
                    {
                        PatientID = name,
                        PatientName = "",
                        DueAmount = 0


                    }).ToList();
                }



            }

        }


        [System.Web.Http.Route("api/CatNameList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> GetCatNameList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);


                return String.IsNullOrEmpty(query) ? db.HMS_TESTMST.AsEnumerable().Select(b => new OpdDTO { }).ToList() :
                db.HMS_TESTMST.Where(p => p.TCATNM.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              catname = x.TCATNM,
                              catid = x.TCATID,



                          })
                .AsEnumerable().Select(a => new OpdDTO
                {
                    TestCategoryId = a.catid,
                    TestCategoryName = a.catname,



                }).ToList();
            }
        }


        [System.Web.Http.Route("api/DynamicCatName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> DynamicCatcomplete(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);

                String name = "";

                var rt = db.HMS_TESTMST.Where(n => n.TCATNM.StartsWith(changedText) && n.COMPID == compid).Select(n => new
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



                var findid = (from n in db.HMS_TESTMST where n.TCATNM == name && n.COMPID == compid select n).ToList();
                if (findid.Count != 0)
                {
                    return db.HMS_TESTMST.Where(p => p.TCATNM == name && p.COMPID == compid).Select(
                        x =>
                            new
                            {
                                catname = x.TCATNM,
                                catid = x.TCATID,



                            })
                        .AsEnumerable().Select(a => new OpdDTO
                        {

                            TestCategoryName = a.catname,
                            TestCategoryId = a.catid,




                        }).ToList();
                }
                else
                {
                    return db.HMS_TESTMST.AsEnumerable().Select(a => new OpdDTO
                    {
                        TestCategoryName = name,
                        TestCategoryId = 0


                    }).ToList();
                }



            }

        }

        public ASL_LOG aslLog = new ASL_LOG();
        public void Insert_OpdLogData(OpdDTO model)
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
            aslLog.LOGDATA = Convert.ToString("Patient ID: " + model.PatientID + ",\nPatient Name: " + model.PatientName + ",\nTransaction Date: " + model.TransactionDate +
               ",\nRefer Name: " + model.Refername +
                ",\nTest Serial: " + model.TestSerial + ",\nCategory Name: " + model.TestCategoryName + ",\nTest Name: " + model.TestName + ",\nAmount :" + model.Amount +
                ",\nPcntd: " + model.Pcntd + ",\nPcntr: " + model.Pcntr + ",\nDiscr: " + model.Discr + ",\nRemarks: " + model.Remarks);
            aslLog.TABLEID = "HMS_OPD";
            aslLog.USERPC = model.USERPC;

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }




        [System.Web.Http.Route("api/grid/OpdChild")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddChildData(OpdDTO model)
        {
            DateTime Tdate = Convert.ToDateTime(model.TransactionDate);

            var check_data = (from n in db.HMS_OPD
                              where n.COMPID == model.CompanyID && n.TRANSDT == Tdate
                                  && n.PATIENTID == model.PatientID && n.TCATID == model.TestCategoryId && n.TESTID == model.TestID
                              select n).ToList();
            if (check_data.Count == 0)
            {
                Int64 max_testSl = Convert.ToInt64((from n in db.HMS_OPD
                                                    where n.COMPID == model.CompanyID && n.TRANSDT == Tdate
                                                          && n.PATIENTID == model.PatientID
                                                    select n.TESTSL).Max());


                Opd childData = new Opd();
                childData.COMPID = model.CompanyID;
                childData.TRANSDT = Tdate;

                childData.PATIENTID = model.PatientID;
                childData.TCATID = model.TestCategoryId;
                childData.TESTID = model.TestID;
                childData.AMOUNT = model.Amount;
                childData.PCNTD = model.Pcntd;
                childData.PCNTR = model.Pcntr;
                childData.DISCR = model.Discr;
                childData.TESTSL = max_testSl + 1;
                  
                       
                       
                childData.USERPC = strHostName;
                childData.INSIPNO = ipAddress.ToString();
                childData.INSTIME = Convert.ToDateTime(td);
                childData.INSUSERID = model.INSUSERID;
                childData.INSLTUDE = model.INSLTUDE;
                if(childData.TCATID!=null && childData.TESTID!=0)
                {
                    db.HMS_OPD.Add(childData);

                    db.SaveChanges();
                }
               

                model.ID = childData.ID;
                model.CompanyID = model.CompanyID;
                model.TransactionDate = model.TransactionDate;
                model.PatientID = model.PatientID;
              
                model.TestSerial = childData.TESTSL;
              
                model.TestCategoryId = model.TestCategoryId;
                model.TestID = model.TestID;

                model.Amount = model.Amount;


                model.Pcntd = model.Pcntd;
                model.Pcntr = model.Pcntr;
                model.Discr = model.Discr;

                if (childData.TCATID != null && childData.TESTID != 0)
                {
                    Insert_OpdLogData(model);
                }
               






                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.TestCategoryId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }



        [System.Web.Http.Route("api/ApiOpd/DeleteData")]
        [System.Web.Http.HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(OpdDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AslLabApiContext"].ToString());
            string query = string.Format("DELETE FROM HMS_OPD WHERE ID='{0}'", model.ID);
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
            Opd testObj = new Opd();






            return Request.CreateResponse(HttpStatusCode.OK, testObj);
        }


        [System.Web.Http.Route("api/OpdReferNameList")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> GetReferNameList(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);


                return String.IsNullOrEmpty(query) ? db.HMS_REFER.AsEnumerable().Select(b => new OpdDTO { }).ToList() :
                db.HMS_REFER.Where(p => p.REFERNM.StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                              refername = x.REFERNM,
                              referid = x.REFERID,



                          })
                .AsEnumerable().Select(a => new OpdDTO
                {
                    ReferID = a.referid,
                    Refername = a.refername,



                }).ToList();
            }
        }
        [System.Web.Http.Route("api/DynamicRefName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> DynamicRefcomplete(string changedText, string changedText2)
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



                            })
                        .AsEnumerable().Select(a => new OpdDTO
                        {

                            Refername = a.refername,
                            ReferID = a.referid,




                        }).ToList();
                }
                else
                {
                    return db.HMS_REFER.AsEnumerable().Select(a => new OpdDTO
                    {
                        Refername = name,
                        ReferID = 0


                    }).ToList();
                }



            }

        }


        [System.Web.Http.Route("api/ApiOpd/Save2ndPart")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> Get(string Compid, string TransactionDate, string PatienId, string Gender,
           string Age, string Address, string MObileNo,string DoctorID, string ReferID, string DvDate, string Dvtm, string TotalAmount, string DiscountRefer,
            string DiscountHos, string Discountnet, string NetAmount, string ReceiveAmount, string DueAmount)
        {

            Int64 compid = Convert.ToInt64(Compid);

            DateTime fdate = DateTime.Parse(TransactionDate);
          
            if (Age == null || Age=="")
            {
                Age = "0";
            }
            

            var ache_kina_data = (from n in db.HMS_OPDMST where n.COMPID == compid && n.TRANSDT == fdate && n.PATIENTID == PatienId select n).ToList();
            if (ache_kina_data.Count == 0)
            {
                //yield return new OpdDTO
                //{

                //    CompanyID = compid,
                //    TransactionDate = fdate,

                //    TestSerial = 0

                //};


            }
            else
            {
                foreach (var item in ache_kina_data)
                {
                    item.ID = item.ID;
                    item.COMPID = item.COMPID;
                    item.TRANSDT = item.TRANSDT;
                    item.PATIENTID = item.PATIENTID;
                    item.TRANSMY = item.TRANSMY;
                    item.TRANSNO = item.TRANSNO;

                    item.USERPC = item.USERPC;
                    item.INSUSERID = item.INSUSERID;
                    item.INSIPNO = item.INSIPNO;
                    item.INSLTUDE = item.INSLTUDE;
                    item.INSTIME = item.INSTIME;

                    item.GENDER = Gender;
                    item.AGE = Age;
                    item.ADDRESS = Address;
                    item.MOBNO = MObileNo;
                    item.DOCTORID = Convert.ToInt64(DoctorID);
                    item.REFERID = Convert.ToInt64(ReferID);
                    if (DvDate == null)
                    {
                        item.DVDT = null;
                    }
                    else
                    {
                        item.DVDT = Convert.ToDateTime(DvDate);
                    }

                    item.DVTM = Dvtm;
                    item.TOTAMT = Convert.ToDecimal(TotalAmount);
                    item.DISCREF = Convert.ToDecimal(DiscountRefer);
                    item.DISCHOS = Convert.ToDecimal(DiscountHos);
                    item.DISCNET = Convert.ToDecimal(Discountnet);
                    item.NETAMT = Convert.ToDecimal(NetAmount);
                    item.RCVAMT = Convert.ToDecimal(ReceiveAmount);
                    item.DUEAMT = Convert.ToDecimal(DueAmount);
                    db.SaveChanges();
                }



            }


            return null;





        }


        //..............Update...................

        [System.Web.Http.Route("api/OpdUpdate/PatientId")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> GetPatientId(string query, string query2)
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
                                      rf = t1.RFPCNT
                                     // gender=t1.GENDER,
                                     // age=t1.AGE,
                                     // address=t1.ADDRESS,
                                     // mobile=t1.MOBNO,
                                     //doctorid=t1.DOCTORID,
                                     //referid=t1.REFERID,
                                     //deliverydate=t1.DVDT

                                  });
               
                foreach (var item in returndata)
                {
                    DateTime dd = Convert.ToDateTime(item.transdt);
                    string aa = dd.ToString("dd-MMM-yyyy");
                    string doctorname = "", refername = "";
                    //var refername_find =
                    //    (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID==item.referid select n).ToList();
                    //foreach (var refer in refername_find)
                    //{
                    //    refername = refer.REFERNM;
                    //}
                    //var doctorname_find =
                    // (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID == item.doctorid select n).ToList();
                    //foreach (var refer in doctorname_find)
                    //{
                    //    doctorname = refer.REFERNM;
                    //}
                    yield return new OpdDTO
                    {

                        PatientID = item.patientid,
                        PatientName = item.patientname,
                        RfPercentage = item.rf,
                        TDateBrother = aa
                        //Gender = item.gender,
                        //Age = item.age,
                        //Address = item.address,
                        //MObileNo = item.mobile,
                        //DoctorName = doctorname,
                        //DoctorID = item.doctorid,
                        //Refername = refername,
                        //ReferID = item.referid,
                        //DvDate = item.deliverydate

                    };
                }







            }
        }

        [System.Web.Http.Route("api/Opd/DynamicPatientName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> autocompleteForOpd(string changedText, string changedText2)
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
                                          rf = t1.RFPCNT


                                      });
                    foreach (var item in returndata)
                    {
                        DateTime dd = Convert.ToDateTime(item.transdt);
                        string aa = dd.ToString("dd-MMM-yyyy");
                        yield return new OpdDTO
                        {

                            PatientID = item.patientid,
                            PatientName = item.patientname,

                            TDateBrother = aa,
                            RfPercentage = item.rf
                        };
                    }

                }
                else
                {

                    yield return new OpdDTO
                    {

                        PatientID = name,
                        PatientName = ""


                    };
                }



            }

        }


        [System.Web.Http.Route("api/ApiOpd/GetData2")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> Get2(string Compid, string TransactionDate, string PatientName, string RfPercentage, string PatienId, string InUserID, string InsLtude)
        {

            //Get Ip ADDRESS,Time & user PC Name
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Int64 InsUserID = Convert.ToInt64(InUserID);
            Int64 compid = Convert.ToInt64(Compid);

            DateTime fdate = DateTime.Parse(TransactionDate);
            string ffdt = fdate.ToString("ddMMyy");

            decimal Rfpercentage = Convert.ToDecimal(RfPercentage);

            
               
              
                    var ff = (from n in db.HMS_OPD where n.COMPID == compid && n.PATIENTID == PatienId select n).ToList();
                    if (ff.Count == 0)
                    {

                        yield return new OpdDTO
                        {

                            CompanyID = compid,
                            TransactionDate = fdate,
                            PatientID = PatienId,
                            PatientName = PatientName,
                            RfPercentage = Rfpercentage,
                            TestSerial = 0


                        };

                    }
                    else
                    {

                        var opd_data = (from t1 in db.HMS_OPD
                                        join t2 in db.HMS_OPDMST on t1.PATIENTID equals t2.PATIENTID
                                        join t3 in db.HMS_TEST on t1.TESTID equals t3.TESTID
                                        join t4 in db.HMS_TESTMST on t1.TCATID equals t4.TCATID
                                        where t1.COMPID == compid && t1.COMPID == t2.COMPID && t1.PATIENTID == PatienId
                                        select new
                                        {
                                            Id = t1.ID,

                                            compid = t1.COMPID,
                                            Transdate = t1.TRANSDT,
                                            Patientid = t1.PATIENTID,
                                            referid = t1.REFERID,
                                            testsl = t1.TESTSL,
                                            tcatid = t1.TCATID,
                                            tCatname = t4.TCATNM,
                                            testid = t1.TESTID,
                                            testname = t3.TESTNM,
                                            amount = t1.AMOUNT,
                                            pcntr = t1.PCNTR,
                                            pcntd = t1.PCNTD,
                                            discr = t1.DISCR,
                                            remarks = t1.REMARKS


                                        });

                        foreach (var item in opd_data)
                        {

                            yield return new OpdDTO
                            {
                                ID = item.Id,
                                CompanyID = item.compid,
                                PatientName = PatientName,
                                TransactionDate = item.Transdate,
                                PatientID = item.Patientid,
                                ReferID = item.referid,
                                TestSerial = item.testsl,
                                TestCategoryId = item.tcatid,
                                TestCategoryName = item.tCatname,
                                TestID = item.testid,
                                TestName = item.testname,
                                Amount = item.amount,
                                Pcntd = item.pcntd,
                                Pcntr = item.pcntr,
                                Discr = item.discr,
                                Remarks = item.remarks

                            };
                        }
                    }
                




           


        }

        [System.Web.Http.Route("api/Opd2ndPartdata_fetch")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdDTO> Opd2ndPartdata_fetch(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {

                Int64 compid = Convert.ToInt64(changedText2);


                var find_data =
                    (from n in db.HMS_OPDMST where n.COMPID == compid && n.PATIENTID == changedText select n).ToList();
                
                foreach (var opdMaster in find_data)
                {
                    string date = "";
                    if(opdMaster.DVDT!=null)
                    {
                        DateTime dd = Convert.ToDateTime(opdMaster.DVDT);
                         date = dd.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        DateTime dd = Convert.ToDateTime(td);
                        date = dd.ToString("dd-MMM-yyyy");
                    }
                   
                    string doctorname = "", refername = "";
                    var finddoctorname =
                        (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID == opdMaster.DOCTORID select n)
                            .ToList();
                    foreach (var refer in finddoctorname)
                    {
                        doctorname = refer.REFERNM;
                    }
                    var findrefername =
                       (from n in db.HMS_REFER where n.COMPID == compid && n.REFERID == opdMaster.REFERID select n)
                           .ToList();
                    foreach (var refer in findrefername)
                    {
                        refername = refer.REFERNM;
                    }
                    yield return new OpdDTO
                    {


                        Gender = opdMaster.GENDER,
                        Age =opdMaster.AGE,
                        Address = opdMaster.ADDRESS,
                        MObileNo = opdMaster.MOBNO,
                        DoctorID =opdMaster.DOCTORID,
                        DoctorName = doctorname,
                        ReferID = opdMaster.REFERID,
                        Refername = refername,
                        TDateBrother = date


                    };
                }
                   // return (from t1 in db.HMS_OPDMST

                   //         where t1.COMPID == compid && t1.PATIENTID == changedText 
                   //         select new
                   //         {
                   //             gender = t1.GENDER,
                   //             age = t1.AGE,
                   //             address = t1.ADDRESS,
                   //             mobileno = t1.MOBNO,
                   //             doctorid = t1.DOCTORID,
                   //             referid=t1.REFERID,
                   //             deliverydt=t1.DVDT



                   //         })
                   //.AsEnumerable().Select(a => new OpdDTO
                   //{

                   //    Gender = a.gender,
                   //    Age = a.age,
                   //    Address = a.address,
                   //    MObileNo = a.mobileno,
                   //    DoctorID = a.doctorid,
                   //    ReferID = a.referid,
                   //    DvDate = a.deliverydt


                   //}).ToList();


               



            }

        }

    }


}

