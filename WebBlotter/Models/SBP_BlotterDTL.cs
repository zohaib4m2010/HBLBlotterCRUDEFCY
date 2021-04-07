using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBlotter.ViewModel;

namespace WebBlotter.Models
{
    public class SBP_BlotterDTL
    {
        public int SNo { get; set; }

        [DisplayName("DTL Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DTL_Date { get; set; }
        [Required]

        [DisplayName("DTL Code")]
        public string DTL_Code { get; set; }
        public int NO_OF_Days { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [DisplayName("DTL Amount")]
        public Nullable<double> DTL_Amount { get; set; } = 0;
        public string T_User_Id { get; set; }
        public System.DateTime T_Date { get; set; }
        public string Flag { get; set; }
        [DisplayName("Branch Code")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string br { get; set; }

    }
}
