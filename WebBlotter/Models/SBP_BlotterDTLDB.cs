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
    public  class SBP_BlotterDTLDB
    {

        public int Id { get; set; }
        public int DTL_Days { get; set; }
        [DisplayName("DTL Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DTL_Date { get; set; }
        [DisplayName("Next DTL Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime NextDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [DisplayName("DTL Amount")]
        public Nullable<decimal> DTL_Amount { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> MinAmount_3P { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> MaxAmount_5P { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Friday_01 { get; set; } = 0;
        [DisplayName("Date 01")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_01 { get; set; }  
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_01 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_01 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Saturday_02 { get; set; } = 0;
        [DisplayName("Date 02")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_02 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_02 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_02 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Sunday_03 { get; set; } = 0;
        [DisplayName("Date 03")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_03 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_03 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_03 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Monday_04 { get; set; } = 0;
        [DisplayName("Date 04")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_04 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_04 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_04 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Tuesday_05 { get; set; } = 0;
        [DisplayName("Date 05")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_05 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_05 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_05 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Wednesday_06 { get; set; } = 0;
        [DisplayName("Date 06")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_06 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_06 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_06 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Thursday_07 { get; set; } = 0;
        [DisplayName("Date 07")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_07 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_07 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_07 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Friday_08 { get; set; } = 0;
        [DisplayName("Date 08")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_08 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_08 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_08 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Saturday_09 { get; set; } = 0;
        [DisplayName("Date 09")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_09 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_09 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_09 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Sunday_10 { get; set; } = 0;
        [DisplayName("Date 10")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_10 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_10 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_10 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Monday_11 { get; set; } = 0;
        [DisplayName("Date 11")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_11 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_11 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_11 { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Tuesday_12 { get; set; } = 0;
        [DisplayName("Date 12")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_12 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_12 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_12 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Wednesday_13 { get; set; } = 0;
        [DisplayName("Date 13")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_13 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_13 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_13 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Thursday_14 { get; set; } = 0;
        [DisplayName("Date 13")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Date_14 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashFlow_14 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashOutFlow_14 { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string BR { get; set; }

    }
}
