using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class BlotterSBP_Reserved
    {
        public int SNo { get; set; }


        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> CRRFinconID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Reserved Balance")]
        public Nullable<decimal> ReservedBalance { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "SBP Balanace")]
        public Nullable<decimal> SBPBalanace { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Balance Difference")]
        public Nullable<decimal> BalanceDifference { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CurID { get; set; }
        public Nullable<int> BR { get; set; }
        public Nullable<int> BID { get; set; }
        public string Flag { get; set; }
    }
}