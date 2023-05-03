using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslLabApi.Models;
using AslLabApi.Models.DTO;

namespace AslLabApi.Controllers.Api
{
    public class ApiOpdReceiveController : ApiController
    {
        [System.Web.Http.Route("api/TransNo")]
        [System.Web.Http.HttpGet]
        public IEnumerable<OpdReceiveDTO> GetTransNo(string query, string query2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(query2);
              
                return String.IsNullOrEmpty(query) ? db.HMS_OPDRCV.AsEnumerable().Select(b => new OpdReceiveDTO { }).ToList() :
                db.HMS_OPDRCV.Where(p => p.TRANSNO.ToString().StartsWith(query) && p.COMPID == compid).Select(
                          x =>
                          new
                          {
                             transno=x.TRANSNO,
                              patientid = x.PATIENTID,
                              date=x.TRANSDT,
                              month=x.TRANSMY,
                              patientdueamount=x.DUEAMTP,
                              discountlab=x.DISCHOS,
                              netamount=x.NETAMT,
                              receiveamount=x.RCVAMT,
                              dueamount=x.DUEAMT,
                              remarks=x.REMARKS

                             

                          })
                .AsEnumerable().Select(a => new OpdReceiveDTO
                {
                   TransNo = a.transno,
                    PatientID = a.patientid,
               
                    TransactionDate = a.date,
                    TransMonthYear = a.month,
                    PatientDueAmount = a.patientdueamount,
                    DiscountHos = a.discountlab,
                    NetAmount = a.netamount,
                    ReceiveAmount = a.receiveamount,
                    DueAmount = a.dueamount,
                    Remarks = a.remarks

                   

                }).ToList();
            }
        }



         [System.Web.Http.Route("apiOpdReceive/PatientName")]
        [System.Web.Http.HttpGet]

        public string GetPatient(string changedText, string changedText2)
        {
            using (var db = new AslLabApiContext())
            {
                Int64 compid = Convert.ToInt64(changedText2);
                Int64 tno = Convert.ToInt64(changedText);
                var find_patient = (from n in db.HMS_OPDRCV
                                    join p in db.HMS_OPDMST on n.PATIENTID equals p.PATIENTID
                                    where n.COMPID == compid && n.TRANSNO == tno
                                    select p).ToList();
                string patientname = "";
                foreach (var x in find_patient)
                {
                    patientname = x.PATIENTNM;
                }
                return patientname;
            }
           
        }


    }
}
