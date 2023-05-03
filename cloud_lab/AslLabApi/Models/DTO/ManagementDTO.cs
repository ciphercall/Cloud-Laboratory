using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class ManagementDTO
    {
      
        public Int64 ID { get; set; }

     
        public Int64 COMPID { get; set; }

     
        public Int64? ManagementId { get; set; }

        public string ManagementName { get; set; }

        public string Designation { get; set; }
        public string Address { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string EmailId { get; set; }

        
    
        public Int64?ReferId { get; set; }

        public string Refername { get; set; }
        public decimal? ReferPCNT { get; set; }
        public string Remarks { get; set; }
        //public IEnumerable<ManagementRFDTO> ManagementRFDTO { get; set; }

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