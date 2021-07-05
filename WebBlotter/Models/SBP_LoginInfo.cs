using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //[Required]
        //[DataType(DataType.Password)]
        //[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[RegularExpression(@"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$")]
        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Contact No# is required")]
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