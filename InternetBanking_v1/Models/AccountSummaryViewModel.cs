using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;
using Spire.Pdf.Exporting.XPS.Schema;

namespace InternetBanking_v1.Models
{
    public class AccountSummaryViewModel
    {
        //[Display(Name = "AccountNumber", ResourceType = typeof(GlobalRes))]
        
        /*[StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "FirstNameLong")]*/
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Balance { get; set; }
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }
        [Display(Name = "Currency")]
        public string Currency { get; set; }
        public string BranchName { get; set; }

        public string StateAmount { get; set; }
        public string TranscationDirection { get; set; }

        public string TranscationNarration { get; set; }

        
        public string Date { get; set; }
        public string BalanceAfterTransaction { get; set; }
        [Display(Name = "Transaction ID")]
        public int StateID { get; set; }

        [Display(Name = "Additional Reference")]
        public string Additional_Reference { get; set; }


    }
}