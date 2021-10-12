using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterCRD
    {
        public long SNo { get; set; }
        public string BankCode { get; set; }
        //[Required]
        [DisplayName("Value Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ValueDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "InFlow")]
        public Nullable<decimal> CRD_InFlow { get; set; }
        [Required]
        [Display(Name = "OutFlow")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CRD_OutFlow { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
    }
}