using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBPBlotterCRRReportFCY_Result
    {
        public int CRRID { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> PreWeek5PcrReq { get; set; }
        public Nullable<decimal> PreWeek10PcrReq { get; set; }
    }
}