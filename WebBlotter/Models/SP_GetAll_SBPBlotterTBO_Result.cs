using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetAll_SBPBlotterTBO_Result
    {
        public long SNo { get; set; }
        public string DataType { get; set; }
        public int TTID { get; set; }
        public string TransactionType { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TBO_Date { get; set; }
        public string TBOCOde { get; set; } 
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> TBO_InFlow { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> TBO_OutFLow { get; set; }
        public string BankName { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        public Nullable<long> NostroBankId { get; set; }
    }
}