using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBPBlotterFR_Result
    {
        public long SNo { get; set; }
        public string DataType { get; set; }
        public string Bank { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<double> Rate { get; set; }
        public string DealType { get; set; }
        public string Broker { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Issue_Date { get; set; }
        public string IssueType { get; set; }
        public Nullable<decimal> InFlow { get; set; }
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