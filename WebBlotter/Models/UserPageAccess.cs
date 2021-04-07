using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class UserPageAccess
    {
        [StringLength(50)]
        [Required(ErrorMessage = "Display Name is Required. It cannot be empty")]
        public string DisplayName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Page Name is Required. It cannot be empty")]
        public string PageName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Controller Name is Required. It cannot be empty")]
        public string ControllerName { get; set; }
        public bool DateChaneAccess { get; set; }
        public bool EditAccess { get; set; }
        public bool DeleteAccess { get; set; }

    }
}