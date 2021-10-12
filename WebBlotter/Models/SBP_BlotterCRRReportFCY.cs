using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterCRRReportFCY
    {
        public int CRRID { get; set; }
        [Required]
        [DisplayName("Start Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Pre Week 5Pcr Req")]
        public Nullable<decimal> PreWeek5PcrReq { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Pre Week 10Pcr Req")]
        public Nullable<decimal> PreWeek10PcrReq { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> BR { get; set; }
        public Nullable<int> BID { get; set; }
        public string Flag { get; set; }
    }
}