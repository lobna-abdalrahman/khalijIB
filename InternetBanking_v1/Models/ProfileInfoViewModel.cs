using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ProfileInfoViewModel
    {
        public string FullName { get; set; }
        public string sts { get; set; }


        [Display(Name = "Email", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EmailRequired")]
        [RegularExpression(".+@.+\\..+", ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EmailInvalid")]
        public string Email { get; set; }


        //[Required(ErrorMessage = "Mobile # is Required")]
        [Display(Name = "Mobile", ResourceType = typeof(GlobalRes))]
        //[Required(ErrorMessageResourceType = typeof(GlobalRes),
        //    ErrorMessageResourceName = "MobileRequired")]
        [StringLength(12, ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MobileLong")]
        public string Mobile { get; set; }


        [Display(Name = "Address", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AddressRequired")]
        public string Address { get; set; }
    }
}