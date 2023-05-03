using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AslLabApi.Models.DTO
{
    public class OpdDTO
    {


        public Int64 ID { get; set; }


        public Int64 CompanyID { get; set; }



        public DateTime? TransactionDate { get; set; }
        public string TDateBrother { get; set; }
        public string PatientID { get; set; }

        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }

        [Required(ErrorMessage = "Your must provide a PhoneNumber")]
        //[Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
      
        public string MObileNo { get; set; }
        public string Address { get; set; }

        public Int64? DoctorID { get; set; }
        public string DoctorName { get; set; }
        public Int64? ReferID { get; set; }
        public string Refername { get; set; }

        public decimal? RfPercentage { get; set; }


        public DateTime? DvDate { get; set; }
        public string Dvtm { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? DiscountRefer { get; set; }

        public decimal? DiscountHos { get; set; }

        public decimal? Discountnet { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? ReceiveAmount { get; set; }

        public decimal DueAmount { get; set; }

       

        public string Type { get; set; }
        public Int64? TestSerial { get; set; }
        public Int64? TestCategoryId { get; set; }
        public string TestCategoryName { get; set; }

        public Int64? TestID { get; set; }
        public string TestName { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Pcntr { get; set; }

        public decimal? Pcntd { get; set; }

        public decimal? Discr { get; set; }
        public string Remarks { get; set; }

        public string LoggedUserTp { get; set; }


        public Int64 ForPrintOrPreview { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Header { get; set; }
        public string Top { get; set; }

       
        public string USERPC { get; set; }
        public Int64? INSUSERID { get; set; }

       
        public DateTime? INSTIME { get; set; }

      
        public string INSIPNO { get; set; }
        public string INSLTUDE { get; set; }
        public Int64? UPDUSERID { get; set; }

      
        public DateTime? UPDTIME { get; set; }

       
        public string UPDIPNO { get; set; }
        public string UPDLTUDE { get; set; }

        public decimal DueDiscountHos { get; set; }
        public decimal DueReceiveAmount { get; set; }
        public decimal DueDueAmount { get; set; }


    }
}