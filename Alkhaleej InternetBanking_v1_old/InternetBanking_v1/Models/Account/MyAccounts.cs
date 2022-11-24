using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models.Account
{
    public class MyAccounts
    {
        //[Required(ErrorMessage = "Account Number is Required.")]
        public int AccountID { get; set; }

        [Display(Name = "AccountNo", ResourceType = typeof(GlobalRes))]    
        public string AccountNumber { get; set; }

        [NotMapped]
        public SelectList AccountList { get; set; }   
        //public List<SelectListItem> AccountNumbers { get; set; }
        public string AccountType { get; set; }

        //[Required(ErrorMessage = "Cheque Number is Required.")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChequeNumberRequired")]
        public string ChequeNumber1 { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChequeNumberRequired")]
        //[Required(ErrorMessage = "Cheque Number is Required.")]
        public string ChequeNumber2 { get; set; }


        [Display(Name = "CheqNum", ResourceType = typeof(GlobalRes))]
        public string CheqNum { get; set; }

        [Display(Name = "ChequeStatus", ResourceType = typeof(GlobalRes))]
        public string Status { get; set; }

        [Display(Name = "ChequeDate", ResourceType = typeof(GlobalRes))]
        public string Date { get; set; }



    }
}