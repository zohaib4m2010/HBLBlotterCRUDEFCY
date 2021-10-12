using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBP_Reserved_Result
    {
        public int sno { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Reserved Balance")]
        public Nullable<decimal> ReservedBalance { get; set; }
        public Nullable<decimal> SBPBalanace { get; set; }
        public Nullable<decimal> BalanceDifference { get; set; }
    }
}