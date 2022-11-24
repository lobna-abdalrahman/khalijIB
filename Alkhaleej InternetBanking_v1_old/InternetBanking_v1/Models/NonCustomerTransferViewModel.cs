using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;


namespace InternetBanking_v1.Models
{
   public class NonCustomerTransferViewModel
    {
        public int AccountID { get; set; }
         
        [Display(Name = "Verificatio Code")]
        //[Required(ErrorMessageResourceType = typeof(GlobalRes),
        //    ErrorMessageResourceName = "VerificationCodeRequired")]
        public string VerificatioCode { get; set; }
        public string FromAccount { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; } 

        [Display(Name = "PhoneNo", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "PhoneNoRequired")]
        public string ToPhoneNo { get; set; }
        [Display(Name = "Name", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }

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

        
    }
}
