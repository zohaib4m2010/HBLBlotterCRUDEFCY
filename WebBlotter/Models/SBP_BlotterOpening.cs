using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebBlotter.Models
{
    public class SBP_BlotterOpening
    {
        public int SNo { get; set; }
        public Nullable<System.DateTime> CurrentDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> TodayAmount { get; set; }
        [DisplayName("Branch Code")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string br { get; set; }

    }
}
