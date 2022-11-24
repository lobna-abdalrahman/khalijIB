using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models.Login
{
    public class UserAccount
    {
        //[Key]
        public int UserID { get; set; }

        //[Required(ErrorMessage = "UserName is Required.")]\
        [Display(Name = "UserName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "UserNameRequired")]
        public String UserName { get; set; }


        /*[Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]*/
        [Display(Name = "password", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "passwordRequired")]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = "New Password")]
        [Display(Name = "NewPassowrd", ResourceType = typeof(GlobalRes))]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        //[Display(Name = "Confirm New Password")]
        [Display(Name = "reNewPassowrd", ResourceType = typeof(GlobalRes))]
        [Compare("NewPassword", ErrorMessage = "Please make sure to Match written Password correctly")]
        public string ReNewPassword { get; set; }


        [Display(Name = "RememberMe", ResourceType = typeof(GlobalRes))]
        public bool RememberMe { get; set; }


        //[Required(ErrorMessage = "Address Cannot Be Empty.")]
        public String Address { get; set; }


        //[Required(ErrorMessage = "Email canot be Empty.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public String Email { get; set; }


    }
}