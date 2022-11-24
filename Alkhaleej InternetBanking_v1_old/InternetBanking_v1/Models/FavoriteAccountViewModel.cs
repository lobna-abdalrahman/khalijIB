using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class FavoriteAccountViewModel
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string AccountTypeCode { get; set; }
        public string AccountTypeName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AccountNameRequired")]
        public string AccountName { get; set; }

        [Display(Name = "FullAccountNumber", ResourceType = typeof(GlobalRes))]
        public string FullAccountNumber { get; set; }

        [Display(Name = "ShortAccountNumber", ResourceType = typeof(GlobalRes))]
        public string ShortAccountNumber { get; set; }

        [Display(Name = "AccountDesc", ResourceType = typeof(GlobalRes))]
        public string AccountDesc { get; set; }

    }
}