using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class CardlessTransferViewModel
    {
        [Display(Name = "PAN")]
        public string CardNumber { get; set; }

        public string BillerName { get; set; }


        [Display(Name = "PhoneNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PhoneNoRequired")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Phone # must be 10 digits")]
        public string VoucherNo { get; set; }
        public string CardExp { get; set; }

        [Display(Name = "Amount",ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),ErrorMessageResourceName = "AmountRequired")]
        public string TranAmount { get; set; }

        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        [DataType(DataType.Password)]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

        //
        public string voucherNumber { get; set; }
        public string voucherCode { get; set; }
        public string transactionAmount { get; set; }

    }
}