using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class UserRole
    {
        public int URID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Role Name is Required. It cannot be empty")]
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}