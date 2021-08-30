using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class BlotterDataColor_Result
    {
        public int Sno { get; set; }
        public Nullable<int> DealNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Inflow { get; set; }
        public Nullable<decimal> Outflow { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<bool> Recon_isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}