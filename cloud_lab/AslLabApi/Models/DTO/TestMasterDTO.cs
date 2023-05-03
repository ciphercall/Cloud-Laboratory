using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class TestMasterDTO
    {
       
        public Int64 ID { get; set; }

      
        public Int64 COMPID { get; set; }

      
        public Int64? TestCatId { get; set; }
        public string TestCatName { get; set; }

        public Int64 DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string TopYesNo { get; set; }
        public Int64? TestId { get; set; }
        public string TestName { get; set; }


        public decimal? Rate { get; set; }


        public decimal? PcnTD { get; set; }

        //public IEnumerable<TestDTO> TestDTO { get; set; }
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