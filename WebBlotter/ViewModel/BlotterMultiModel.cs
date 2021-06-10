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
    public class BlotterMultiModel
    {
       
        public BlottterEmails SendEmail01 { get; set; }
        public List<SP_SBPBlotter_Result> GetAllBlotter01 { get; set; }
        public List<SP_SBPBlotter_FCY_Result> GetAllBlotterFCY01 { get; set; }
    

    }
}

