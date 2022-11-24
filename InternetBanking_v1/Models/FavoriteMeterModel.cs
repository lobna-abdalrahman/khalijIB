using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class FavoriteMeterModel
    {
        public int MeterID { get; set; }

        [Display(Name = "MeterName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MeterNameRequired")]
        public string MeterName { get; set; }


        [Display(Name = "MeterNumber", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MeterNumberRequired")]
        /*[RegularExpression("([0-9][1-9]*)", ErrorMessage = "Enter only numeric number")]*/
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NumericValidationError")]
        [StringLength(11, ErrorMessage = "The {0} must be  {2} characters long.", MinimumLength = 11)]
        public string MeterNumber { get; set; }

        public int UserID { get; set; }
    }
}