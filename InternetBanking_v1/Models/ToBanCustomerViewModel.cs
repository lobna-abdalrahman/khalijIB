using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ToBanCustomerViewModel
    {
        public string FromAccount { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string AccountTypeCode { get; set; }
        public string AccountTypeName { get; set; }
        public string CurrencyCode { get; set; }
        public string fromCurrencyCode { get; set; }
        public string toCurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        [Display(Name = "ToAccountNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ToAccountNo")]
        public string ToAccount { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "AmountRequired")]
        public string Amount { get; set; }

        [Display(Name = "SenderName", ResourceType = typeof(GlobalRes))]
        public string SenderName { get; set; }
        [Display(Name = "RecieverName", ResourceType = typeof(GlobalRes))]

        public string RecieverName { get; set; }
        [Display(Name = "TransferReference", ResourceType = typeof(GlobalRes))]

        public string TranReference { get; set; }

        [Display(Name = "Verifications Code")]
        public string VerificatioCode { get; set; }
        [Display(Name = "Additional Reference")]
        public string Additional_Reference { get; set; }
        [Display(Name = "Customer Accounts")]
        public List<SelectListItem> CustomerAccounts { get; set; }
        [Display(Name = "Phone number")]
        public string CustomerPhone { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Arabic customer name")]
        public string arabiccustomername { get; set; }
        [Display(Name = "Customer name")]
        public string Customername { get; set; }
        [Display(Name = "Customer account")]
        public string CustomerAccount { get; set; }
    }
}