using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class WebPages
    {
        public int WPID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Page Name is Required. It cannot be empty")]
        public string PageName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Controller Name is Required. It cannot be empty")]
        public string ControllerName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Display Name is Required. It cannot be empty")]
        public string DisplayName { get; set; }
        public string PageDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}