using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class Branches
    {
        public int BID { get; set; }

        public string BranchCode { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Branch Name is Required. It cannot be empty")]
        public string BranchName { get; set; }
        public string BrachDescription { get; set; }
        public string BranchLocation { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}