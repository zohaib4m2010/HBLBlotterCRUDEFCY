using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class ChangePassword
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
       
        public string Password { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [DisplayName("New Password")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]

        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DisplayName("Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}