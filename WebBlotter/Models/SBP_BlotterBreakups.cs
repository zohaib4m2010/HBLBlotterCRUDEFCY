using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterBreakups
    {
        public long SNo { get; set; }
        [Required]
        [Display(Name = "Opening Balance Actual")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OpeningBalActual { get; set; }
        [Required]
        [Display(Name = "Food Payment")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> FoodPayment_inFlow { get; set; }
        [Required]
        [Display(Name = "HOK Remittance")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> HOKRemittance_inFlow { get; set; }
        [Required]
        [Display(Name = "ERF")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> ERF_inflow { get; set; }
        [Required]
        [Display(Name = "SBP Cheque Deposite")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> SBPChequeDeposite_inflow { get; set; }
        [Required]
        [Display(Name = "Miscellaneous")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Miscellaneous_inflow { get; set; }
        [Required]
        [Display(Name = "Cash Withdraw by SBP Cheques")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> CashWithdrawbySBPCheques_outFlow { get; set; }
        [Required]
        [Display(Name = "ERF")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> ERF_outflow { get; set; }
        [Required]
        [Display(Name = "DSC")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> DSC_outFlow { get; set; }
        [Required]
        [Display(Name = "Remitance To HOK")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> RemitanceToHOK_outFlow { get; set; }
        [Required]
        [Display(Name = "SBP Cheq Given To Other Bank")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> SBPCheqGivenToOtherBank_outFlow { get; set; }
        [Required]
        [Display(Name = "Miscellaneous")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Miscellaneous_outflow { get; set; }
        [Required]
        [Display(Name = "Estimated Clossing Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> EstimatedCLossingBal { get; set; }
        public Nullable<System.DateTime> BreakupDate { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
    }
}