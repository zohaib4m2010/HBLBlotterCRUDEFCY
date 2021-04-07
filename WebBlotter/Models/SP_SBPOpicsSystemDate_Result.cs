using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace WebBlotter.Models
{
    public class SP_SBPOpicsSystemDate_Result
    {
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OpicsCurrentDate { get; set; }
    }
}
