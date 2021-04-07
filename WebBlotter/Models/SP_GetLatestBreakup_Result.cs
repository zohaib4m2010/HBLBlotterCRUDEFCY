using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetLatestBreakup_Result
    {
        public long SNo { get; set; }
        public Nullable<decimal> OpeningBalActual { get; set; }
        public Nullable<decimal> FoodPayment_inFlow { get; set; }
        public Nullable<decimal> HOKRemittance_inFlow { get; set; }
        public Nullable<decimal> ERF_inflow { get; set; }
        public Nullable<decimal> SBPChequeDeposite_inflow { get; set; }
        public Nullable<decimal> Miscellaneous_inflow { get; set; }
        public Nullable<decimal> CashWithdrawbySBPCheques_outFlow { get; set; }
        public Nullable<decimal> ERF_outflow { get; set; }
        public Nullable<decimal> DSC_outFlow { get; set; }
        public Nullable<decimal> RemitanceToHOK_outFlow { get; set; }
        public Nullable<decimal> SBPCheqGivenToOtherBank_outFlow { get; set; }
        public Nullable<decimal> Miscellaneous_outflow { get; set; }
        public Nullable<decimal> EstimatedCLossingBal { get; set; }
        public int UserID { get; set; }
        public string BranchName { get; set; }
        public int CurID { get; set; }

        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BreakupDate { get; set; }
    }
}