using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public partial class SBP_BlotterClearing
    {
        public long SNo { get; set; }
        public int TTID { get; set; }
        //[Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Clearing_Date { get; set; }
        
        [Display(Name = "Code")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string ClearingCOde { get; set; }

        [Required]
        [Display(Name = "InFlow")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Clearing_InFlow { get; set; }
        public Nullable<decimal> AdjClearing_InFlow { get; set; }

        [Required]
        [Display(Name = "OutFlow")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Clearing_OutFLow { get; set; }
        public Nullable<decimal> AdjClearing_OutFLow { get; set; }
        public string Note { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        
    }
}