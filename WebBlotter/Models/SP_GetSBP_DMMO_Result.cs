using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBP_DMMO_Result
    {
        public int sno { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> PakistanBalance { get; set; }
        public Nullable<decimal> SBPBalanace { get; set; }
        public Nullable<decimal> BalanceDifference { get; set; }
    }
}