using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_SBPBlotter_FCY_Result
    {
        public int DealNo { get; set; }
        public string Description { get; set; } 
        public string Status { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }
        public Nullable<System.DateTime> ValueDate { get; set; }
        public Nullable<System.DateTime> MaturityDate { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> Inflow { get; set; }
        public Nullable<decimal> Outflow { get; set; }
        public decimal OpeningBalance { get; set; }
        public Nullable<bool> Recon_IsActive { get; set; }

    }
}