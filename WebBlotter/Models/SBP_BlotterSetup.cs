using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace WebBlotter.Models
{
    public class SBP_BlotterSetup
    {
        public int SNo { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Name is Required. It cannot be empty")]
        public string Description { get; set; }
        public string status { get; set; }
        [DisplayName("Branch Code")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string br { get; set; }

    }
}
