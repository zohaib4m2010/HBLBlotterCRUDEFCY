using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterFundingRepo
    {
        public long SNo { get; set; }

        [Required]
        public string DataType { get; set; }
        [Required]
        public string Bank { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<double> Rate { get; set; }
        [Required]
        public string DealType { get; set; }
        [Required]
        public string Broker { get; set; }

        [DisplayName("Issue Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Issue_Date { get; set; }
        [Required]
        public string IssueType { get; set; }
        [Required]
        [Display(Name = "In Flow")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> InFlow { get; set; }
        [Required]
        [Display(Name = "Out Flow")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OutFLow { get; set; }
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