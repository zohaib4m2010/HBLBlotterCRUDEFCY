using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_LoginInfo
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[RegularExpression(@"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$")]
        public string Password { get; set; }
        public string ContactNo { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Department { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<int> LoginFailedCount { get; set; }
        public string LoginIPAddress { get; set; }
        public string CustomerTimeZone { get; set; }
        public Nullable<System.DateTime> LastAccessedDate { get; set; }
        public Nullable<bool> AccountLocked { get; set; }
        public Nullable<int> BranchId { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public bool isConventional { get; set; }
        public bool isislamic { get; set; }
        public Nullable<bool> ChangePassword { get; set; }
        public Nullable<int> URID { get; set; }
        public string DefaultPage { get; set; }
        public string BlotterType { get; set; }
    }
}