using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WebBlotter.Models;

namespace WebBlotter.ViewModel
{
    public class BlotterCashFlowMultiModel
    {
        public SP_SBPBlotterRunningBal UpdateCasgFlow { get; set; }
        public SBP_BlotterDTLDB GetAllDTLB1 { get; set; }
    }
}
