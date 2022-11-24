using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class AllTransfersViewModel
    {
        [Display(Name = "ID")]
        public int TranId { get; set; }

        [Display(Name = "Service")]
        public string TranName { get; set; }

        [Display(Name = "Amount")]
        public string TranAmount { get; set; }
        [Display(Name = "TOKEN")]
        public string TranToken { get; set; }

        [Display(Name = "Status")]
        public string TranStatus { get; set; }

        [Display(Name = "Response")]
        public string TranResult { get; set; }

        [Display(Name = "Date")]
        public string TranDate { get; set; }

        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Display(Name = "Reciver Name")]
        public string ReciverName { get; set; }
    }
}