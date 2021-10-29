using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBPBlotterRECON_Result
    {
        public long ID { get; set; }
        public string BankCode { get; set; }
        public Nullable<System.DateTime> LastStatementDate { get; set; }
        public Nullable<decimal> OurBooks { get; set; }
        public Nullable<decimal> TheirBooks { get; set; }
        public Nullable<decimal> ConversionRate { get; set; }
        public Nullable<decimal> EquivalentUSD { get; set; }
        public Nullable<decimal> LimitAvailable { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        public string BankName { get; set; }
    }
}