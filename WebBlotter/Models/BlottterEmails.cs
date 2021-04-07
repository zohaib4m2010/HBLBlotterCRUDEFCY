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
    public class BlottterEmails
    {


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Inflow { get; set; } = 0;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Outflow { get; set; } = 0;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> Balance { get; set; } = 0;

    }
}
