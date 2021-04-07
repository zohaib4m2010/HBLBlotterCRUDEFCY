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
    public class SBP_BlotterManualDeals
    {      
   
        public int SNo { get; set; }


        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }   
        public List<NameDropDown> DescriptionDD { get; set; }


        [DisplayName("Deal Date")]     
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> DealDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> InFlow { get; set; } = 0;
        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OutFlow { get; set; } = 0;
        [DisplayName("Current Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CurrentDate { get; set; }
        public string DealStatus { get; set; }
        [DisplayName("Branch Code")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string br { get; set; }


    }

    public class NameDropDown
    {
        public int SNo { get; set; }
        public string Desc { get; set; }

    }
}
