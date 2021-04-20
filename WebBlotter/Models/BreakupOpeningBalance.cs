using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class BreakupOpeningBalance
    {

        public long SNo { get; set; }
        [Display(Name = "Opening Balance Actual")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OpeningBalActual { get; set; }
        public int BID { get; set; }
        public string BranchName { get; set; }
    }
}