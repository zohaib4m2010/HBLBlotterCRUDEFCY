using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WebBlotter.ViewModel;

namespace WebBlotter.Models
{
    public class SP_SBPBlotterRunningBal
    {
        public string BranchCode { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }
}
