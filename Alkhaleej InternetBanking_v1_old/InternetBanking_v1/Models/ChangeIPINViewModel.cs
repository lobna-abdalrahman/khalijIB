using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ChangeIPINViewModel
    {


        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        public string PAN { get; set; }

        public string expDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "CurrentIPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "CurrentIPINRequired")]
        public string CurrentIPIN { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "NewIPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "NewIPINRequired")]
        public string NewIPIN { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ReNewIPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ReNewIPINRequired")]
        [Compare("NewIPIN", ErrorMessage = "Please make sure to write the IPIN correctly")]
        public string ReNewIPIN { get; set; }

    }
}