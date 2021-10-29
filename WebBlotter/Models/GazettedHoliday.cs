using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class GazettedHoliday
    {
        public int GHID { get; set; }
        public string HolidayTitle { get; set; }
        [Required]
        public string GHDescription { get; set; }

        [Required]
        [DisplayName("Value Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime GHDate { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int UserID { get; set; }
    }
}