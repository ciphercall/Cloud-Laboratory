using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class OpdReceiveDTO
    {
        public Int64 ID { get; set; }


        public Int64 COMPID { get; set; }


        public DateTime? TransactionDate { get; set; }
        public string TransMonthYear { get; set; }

        public Int64? TransNo { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }

        public decimal? PatientDueAmount { get; set; }

        public decimal? DiscountHos { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? ReceiveAmount { get; set; }


        public decimal? DueAmount { get; set; }
        public string Remarks { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public string USERPC { get; set; }
        public Int64? INSUSERID { get; set; }


        public DateTime? INSTIME { get; set; }


        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64? UPDUSERID { get; set; }


        public DateTime? UPDTIME { get; set; }


        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }

    }
}