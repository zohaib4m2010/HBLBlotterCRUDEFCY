using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_SBPBlotter_Result
    {
        public int DEALNO { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }
        public Nullable<System.DateTime> ValueDate { get; set; }
        public Nullable<System.DateTime> MaturityDate { get; set; }
        public string Currency { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Inflow { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Outflow { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OpeningBalance { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string Customer { get; set; }
        public List<NameDropDown> DescriptionDD { get; set; }
        public List<NameDropDown> CustDD { get; set; }

    }
}