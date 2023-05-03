using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class TestDTO
    {

       
      
        public Int64 ID { get; set; }

        
        public Int64 COMPID { get; set; }

        
        public Int64? TestCatId { get; set; }


        public Int64? DepartmentId { get; set; }
       
        public Int64? TestId { get; set; }
        public string TestName { get; set; }

      
        public decimal? Rate { get; set; }

     
        public decimal? PcnTD { get; set; }
    }
}