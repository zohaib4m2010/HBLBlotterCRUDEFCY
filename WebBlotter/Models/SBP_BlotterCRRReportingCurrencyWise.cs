using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterCRRReportingCurrencyWise
    {
        public int SNo { get; set; }
        public Nullable<int> CRRID { get; set; }

        [Display(Name = "Currency")]
        public string CCY { get; set; }
        public Nullable<int> CurID { get; set; }
        //[Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Deposit")]
        public Nullable<decimal> Deposit { get; set; }
        //[Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Pre Week 5 Pcr Req")]
        public Nullable<decimal> CRRBal5PcrReq { get; set; }
        //[Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Pre Week 10Pcr Req")]
        public Nullable<decimal> CRRBal10PcrReq { get; set; }
        //[Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Equivalent USD")]
        public Nullable<decimal> EquivalentUSD { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> BR { get; set; }
        public Nullable<int> BID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Flag { get; set; }
    }
}