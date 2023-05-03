using AslLabApi.Models.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace AslLabApi.Models
{
    public class PageModel
    {

        public PageModel()
        {
            this.aslMenumst = new ASL_MENUMST();
            this.aslMenu = new ASL_MENU();
            this.aslUserco = new AslUserco();
            this.aslCompany = new AslCompany();
            this.aslLog = new ASL_LOG();


            this.AGL_accharmst = new GL_ACCHARMST();
            this.AGL_acchart = new GL_ACCHART();

            this.AGlCostPMST = new GL_COSTPMST();
            this.AGlCostPool = new GL_COSTPOOL();
            this.AGlStrans = new GL_STRANS();
            this.AGlMaster = new GL_MASTER();
         


          
        }

        public ASL_MENUMST aslMenumst { get; set; }
        public ASL_MENU aslMenu { get; set; }
        public AslUserco aslUserco { get; set; }
        public AslCompany aslCompany { get; set; }
        public ASL_LOG aslLog { get; set; }



        public GL_ACCHARMST AGL_accharmst { get; set; }
        public GL_ACCHART AGL_acchart { get; set; }

        public GL_COSTPMST AGlCostPMST { get; set; }
        public GL_COSTPOOL AGlCostPool { get; set; }


        public GL_STRANS AGlStrans { get; set; }
        public GL_MASTER AGlMaster { get; set; }


        [Display(Name = "HeadType")]
        public string HeadType { get; set; }


        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }


        //Schedular Calendar
        public Int64? Userid { get; set; }

    }
}