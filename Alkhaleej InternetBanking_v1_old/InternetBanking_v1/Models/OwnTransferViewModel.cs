using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class OwnTransferViewModel
    {
        public int AccountID { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }

        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }


        //[Required(ErrorMessage = "Please Enter the Verification Code")]
        [Display(Name = "Verificatio Code")]       
        public string VerificatioCode { get; set; }

    }
}