using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ContactUsViewModel
    {
        /*[Required]*/
        [StringLength(20, MinimumLength = 5)]

        [Display(Name = "UserName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "UserNameRequired")]
       
        public string Name { get; set; }


        [Display(Name = "Email", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        public string Email { get; set; }


        [Display(Name = "Subject", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "SubjectRequired")]
        public string Subject { get; set; }


        [Display(Name = "Message", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "MessageRequired")]
        public string Message { get; set; }
    }
}