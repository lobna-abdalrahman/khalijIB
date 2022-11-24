using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class FavoritePhoneModel
    {
        public int FavoritePhoneID { get; set; }

        [Display(Name = "FavoritePhoneName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FavoritePhoneNameRequired")]
        public string FavoritePhoneName { get; set; }


        [Display(Name = "FavoritePhoneNumber", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FavoritePhoneNumberRequired")]
        /*[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]*/
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        [StringLength(10, ErrorMessage = "The {0} must be  {2} characters long.", MinimumLength = 10)]
        public string FavoritePhoneNumber { get; set; }

        public int UserID { get; set; }
    }
}