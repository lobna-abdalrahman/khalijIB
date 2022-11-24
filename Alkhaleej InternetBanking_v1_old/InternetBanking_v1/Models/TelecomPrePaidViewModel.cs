using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class TelecomPrePaidViewModel
    {
        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        
        public string PAN { get; set; }

        [Display(Name = "BillerName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "BillerNameRequired")]
        public string BillerName { get; set; }
        public string BillerCode { get; set; }


        [Display(Name = "ToPhoneNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ToPhoneNoRequired")]
        public string ToPhoneNo { get; set; }
        public string CardExp { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AmountRequired")]
        public string Amount { get; set; }

        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        //[DataType(DataType.Password)]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
    }
}