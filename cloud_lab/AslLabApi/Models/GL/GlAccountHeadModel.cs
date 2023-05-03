using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AslLabApi.Models;

namespace AslLabApi.Models.GL
{
    public class GlAccountHeadModel
    {
        public GlAccountHeadModel()
        {
           
            this.GLACCHARMSTModel = new GL_ACCHARMST();
            this.AcchartModel = new GL_ACCHART();
        }

        public GL_ACCHARMST GLACCHARMSTModel { get; set; }
        public GL_ACCHART AcchartModel { get; set; }
        //public RMS_TRANSMST RmsTransMst { get; set; }
        //public RMS_TRANS RmsTrans { get; set; }
        
        public decimal? Total { get; set; }
        public string Empty { get; set; }//It used for readonly value(HtmlTextBoxfor) hold.

        [Required(ErrorMessage = "Select a Head Type")]
        [Display(Name = "Head Type")]
        public int HEADTP { get; set; }

        [Required(ErrorMessage = "Head name can not be empty!")]
        [Display(Name = "Head Name")]
        public string HEADNM { get; set; }

        //[Required(ErrorMessage = "Remarks field can not be empty!")]
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}