using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetFCYOpeningBalance_Result
    {
        public long Id { get; set; }
        public decimal OpenBalActual { get; set; }
        public Nullable<System.DateTime> LastStatementDate { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        public decimal EstimatedOpenBal { get; set; }
    }
}