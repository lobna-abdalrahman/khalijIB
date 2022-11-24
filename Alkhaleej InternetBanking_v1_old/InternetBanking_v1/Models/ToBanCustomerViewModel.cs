using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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
    }
}