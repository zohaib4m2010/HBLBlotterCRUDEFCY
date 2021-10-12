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
    public class BlotterCRRFCYModel
    {
        public SBP_BlotterCRRReportFCY CRRReportFCY { get; set; }

        public List<SBP_BlotterCRRReportingCurrencyWise> CRRReportingCurrencyWise { get; set; }
    }
}