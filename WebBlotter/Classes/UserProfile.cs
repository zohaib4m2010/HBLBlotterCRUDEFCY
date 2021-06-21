using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Classes
{
    public class UserProfile
    {
        [Required]
        [StringLength(150)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email: ")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(18 ,ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$")]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
    }
}