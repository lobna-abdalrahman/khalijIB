using InternetBanking_v1.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class FirstLoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(GlobalRes))]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(GlobalRes))]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(GlobalRes))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(GlobalRes))]
        public string ReNewPassword { get; set; }
    }
}