using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class CustomsViewModel
    {

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        public string CardNumber { get; set; }

        public string BillerName { get; set; }


        [Display(Name = "Lable_PolicyNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "PolicyNoRequired")]
        //[RegularExpression(@"\d{11}",ErrorMessage = "Meter # must be 11 digits")]
        public string PolicyNo { get; set; }

        [Display(Name = "Lable_Declarnt", ResourceType = typeof(GlobalRes))]
        //[Required(ErrorMessageResourceType = typeof(GlobalRes),
        //    ErrorMessageResourceName = "DeclarntRequired")]
        //[RegularExpression(@"\d{11}",ErrorMessage = "Meter # must be 11 digits")]
        public string Declarnt { get; set; }

        public string CardExp { get; set; }

       
        public string TranAmount { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "IPIN", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "IPINRequired")]
        public string IPIN { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

        //
        public string BankCode { get; set; }
        public string BillAmount { get; set; }
        public string DeclarantNAME { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationSerial { get; set; }
        public string Declarant { get; set; }
        public string ProcError { get; set; }
        public string Transaction { get; set; }
        public string BillYear { get; set; }
        public string DECSER { get; set; }

        public string Status { get; set; }
        public string AmountToBePaid { get; set; }
        [Display(Name = "Lable_Declarnt", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "DeclarntRequired")]
        public string DeclarantCode { get; set; }
     
        public string InstanceID { get; set; }
        public string Office { get; set; }
        
        public string DECNBER { get; set; }
         
    }
}