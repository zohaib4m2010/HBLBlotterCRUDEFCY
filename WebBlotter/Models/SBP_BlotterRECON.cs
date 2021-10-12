using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterRECON
    {
        public long ID { get; set; }
        public string BankCode { get; set; }
        [DisplayName("Last Statement Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastStatementDate { get; set; }
        //[Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Our Books")]
        public Nullable<decimal> OurBooks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Their Books")]
        public Nullable<decimal> TheirBooks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Conversion Rate")]
        public Nullable<decimal> ConversionRate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Equivalent USD")]
        public Nullable<decimal> EquivalentUSD { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //[Display(Name = "Nostro Limit")]
        //public Nullable<decimal> NostroLimit { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Limit Available")]
        public Nullable<decimal> LimitAvailable { get; set; }

        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        public string BankName { get; set; }
    }
}