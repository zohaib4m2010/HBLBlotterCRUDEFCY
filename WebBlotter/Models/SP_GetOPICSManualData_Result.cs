using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetOPICSManualData_Result
    {
        public long SNo { get; set; }
        public string DataType { get; set; }
        public Nullable<decimal> Inflow { get; set; }
        public Nullable<decimal> OutFlow { get; set; }
        public Nullable<decimal> NetBalance { get; set; }
        public Nullable<decimal> AdjBalance { get; set; }
        public Nullable<bool> isAdjusted { get; set; }
        public Nullable<System.DateTime> DateFor { get; set; }
    }
}