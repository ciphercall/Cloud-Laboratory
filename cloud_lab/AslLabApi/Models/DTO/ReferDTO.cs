using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class ReferDTO
    {
        public Int64 ID { get; set; }

       
        public Int64 COMPID { get; set; }


        public Int64? ReferGroupId { get; set; }
       
        public Int64? ReferId { get; set; }
       
        public string ReferName { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string EmailId { get; set; }

        public Int64? TCatId { get; set; }
        public string TCatname { get; set; }
        public decimal? ReferPCNT { get; set; }
        public string Remarks { get; set; }
        public bool errorStatus { get; set; }

        //public IEnumerable<ReferPCNTDTO> ReferPCNTDTO { get; set; }

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