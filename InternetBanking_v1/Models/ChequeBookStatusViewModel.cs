using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ChequeBookStatusViewModel
    {

        [Display(Name = "FromDate", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FromDateRequired")]        
        public string FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ToDateRequired")]
        public string ToDate { get; set; }

        //
        [Display(Name = "AccountNo", ResourceType = typeof(GlobalRes))]        
        public string AccountNumber { get; set; }

        [Display(Name = "RequestedSize", ResourceType = typeof(GlobalRes))]
        public string RequestedSize { get; set; }

        [Display(Name = "RequestedDate", ResourceType = typeof(GlobalRes))]
        public string Date { get; set; }

        [Display(Name = "Status", ResourceType = typeof(GlobalRes))]
        public string RequestStatus { get; set; }





    }
}