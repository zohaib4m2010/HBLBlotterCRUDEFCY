using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class sp_GetAllUsers_Result
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string BranchName { get; set; }
        public Nullable<bool> isConventional { get; set; }
        public Nullable<bool> isislamic { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}