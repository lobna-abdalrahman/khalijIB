using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        //[Display(Name = "Current Password")]
        [Display(Name = "CurrentPassowrd", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "CurrentPassowrdRequired")]
        public string CurrentPassword { get; set; }


        [DataType(DataType.Password)]
        //[Display(Name = "New Password")]
        [Display(Name = "NewPassowrd", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "NewPassowrdRequired")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        //[Display(Name = "Confirm New Password")]
        [Display(Name = "reNewPassowrd", ResourceType = typeof(GlobalRes))]
        [Compare("NewPassword", ErrorMessage = "Please make sure to Match written Password correctly")]
        public string ReNewPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "otp", ResourceType = typeof(GlobalRes))]
        public string otp { get; set; }
    }
}