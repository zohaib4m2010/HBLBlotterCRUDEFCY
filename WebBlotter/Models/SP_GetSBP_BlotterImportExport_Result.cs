using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBP_BlotterImportExport_Result
    {
        public int SNo { get; set; }
        public string BlotterType { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public string BankCode { get; set; }
        public Nullable<int> CurId { get; set; }
        public string Branch { get; set; }
        public string Customer { get; set; }
        public Nullable<decimal> Inflow { get; set; }
        public Nullable<int> AgainstCurId { get; set; }
        public string AgainstBankCode { get; set; }
        public Nullable<decimal> Outflow { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Flag { get; set; }
        public Nullable<int> BID { get; set; }
        public Nullable<int> BR { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Currency { get; set; }
        public string BankName { get; set; }
        public string AgainstCurrency { get; set; }
        public string AgainstBank { get; set; }


    }
}