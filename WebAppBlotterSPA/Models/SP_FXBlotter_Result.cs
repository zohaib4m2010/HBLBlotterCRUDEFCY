using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBlotterSPA.Models
{
    public class SP_SBPBlotter_Result
    {
        public string DealNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }
        public Nullable<System.DateTime> Vdate { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> DrAmt { get; set; }
        public Nullable<decimal> CrAmt { get; set; }
    }
}