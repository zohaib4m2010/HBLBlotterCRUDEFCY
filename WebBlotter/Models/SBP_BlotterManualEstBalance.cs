using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterManualEstBalance
    {
        public long SNo { get; set; }
        public string DataType { get; set; }
        public Nullable<decimal> EstAdjBalance { get; set; }
        public Nullable<bool> isAdjusted { get; set; }
        public Nullable<System.DateTime> AdjDate { get; set; }
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