using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class TestNVDTO
    {
        
        public Int64 ID { get; set; }

       
        public Int64 COMPID { get; set; }

       
        public Int64? TestId { get; set; }
        public string TestName { get; set; }
       
        public Int64? RestId { get; set; }
        
        public string RestName { get; set; }
        public string RestGroupName { get; set; }
        public string RestMU { get; set; }
        public string ShowType { get; set; }
        public Int64? Length { get; set; }
        public Int64? Decimal { get; set; }
        public string NValue { get; set; }
        public string DValue { get; set; }

        public Int64?SerialNo { get; set; }
        public string GroupShow { get; set; }

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