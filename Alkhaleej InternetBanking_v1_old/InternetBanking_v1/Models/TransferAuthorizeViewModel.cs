using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class TransferAuthorizeViewModel
    {
        public string transferID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        [Display(Name = "From Account:")]
        public string FromAccountName { get; set; }

        [Display(Name = "To Account:")]
        public string ToAccountName { get; set; }
        [Display(Name = "From Account:")]
        public string FromAccountNumber { get; set; }

        [Display(Name = "To Account:")]
        public string ToAccountNumber { get; set; }
        public string TransferAmount { get; set; }
        public string TransferStatus { get; set; }
        public string TransferDate { get; set; }
 
        [Display(Name = "Verifications Code")]
        public string VerificatioCode { get; set; }
         [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; }
    }

}