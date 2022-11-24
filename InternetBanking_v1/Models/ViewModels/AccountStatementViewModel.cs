using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Models.ViewModels
{
    public class AccountStatementViewModel
    {

        [NotMapped]
        public SelectList AccountList { get; set; }  
        //[Required(ErrorMessage = "*")]
        //[Display(Name = "Account #")]
        public string AccountNo { get; set; }

        public string AdditionalRefrence { get; set; }

        public SelectList DropDownList { get; set; }

        public int? AccountID { get; set; }

   
        [Display(Name = "From Date")]
        public string fromDate { get; set; }

 
        [Display(Name = "To Date")]
        public string toDate { get; set; }

     //   [Remote("CheckType", "Home")]
        [Display(Name = "Number of Transaction")]
        public string tranno { get; set; }


        [Display(Name = "Transaction ID")]
        public int StateID { get; set; }


        [Display(Name = "Transaction Amount")]
        public string StateAmount { get; set; }

        [Display(Name = "Transaction Type")]
        public string TranscationDirection { get; set; }

        [Display(Name = "Transaction Date")]
        public string Date { get; set; }

        [Display(Name = "Balance")]
        public string BalanceAfterTransaction { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Statment Type")]
        public string StatementType { get; set; }

        [Display(Name = "Transcation Narration")]
        public string TranscationNarration { get; set; }

        [Display(Name = "Currency")]
        public string AccountCurrency { get; set; }

        [Display(Name = "Additional Reference")]
        public string Additional_Reference { get; set; }
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        public string AccountType { get; set; }
    }
}