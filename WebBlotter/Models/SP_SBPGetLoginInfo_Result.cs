using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebBlotter.Models
{
   public class SP_SBPGetLoginInfo_Result
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public bool isConventional { get; set; }
        public bool isislamic { get; set; }
        public string UserExists { get; set; }
        public string DefaultPage { get; set; }
        public string BlotterType { get; set; }
        public string Currencies { get; set; }
        public string Pages { get; set; }
    }
}
