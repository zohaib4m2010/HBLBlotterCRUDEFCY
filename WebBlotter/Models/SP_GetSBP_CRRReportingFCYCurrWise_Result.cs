using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBP_CRRReportingFCYCurrWise_Result
    {
        public string CCY { get; set; }

        public Nullable<decimal> Deposit { get; set; }

        public Nullable<decimal> CRRBal5PcrReq { get; set; }

        public Nullable<decimal> CRRBal10PcrReq { get; set; }

        public Nullable<decimal> EquivalentUSD { get; set; }
        public Nullable<decimal> PreWeek5PcrReq { get; set; }
        public Nullable<decimal> PreWeek10PcrReq { get; set; }
    }
}