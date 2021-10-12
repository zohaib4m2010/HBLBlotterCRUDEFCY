using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_Get_SBPBlotterConversionRate_Result
    {
        public string CCY { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<decimal> SPOTRATE_8 { get; set; }
        public Nullable<decimal> USDRate { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
    }
}