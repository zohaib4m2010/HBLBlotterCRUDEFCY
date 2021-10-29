using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBPBlotterGH_Result
    {
        public int GHID { get; set; }
        public string HolidayTitle { get; set; }
        public string GHDescription { get; set; }
        public System.DateTime GHDate { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int UserID { get; set; }
    }
}