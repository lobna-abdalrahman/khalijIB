using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ManageCardsViewModel
    {
        [Display(Name = "CardName", ResourceType = typeof(GlobalRes))]
        public string CardName { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(GlobalRes))]
        public string CardNo { get; set; }

        [Display(Name = "CardID", ResourceType = typeof(GlobalRes))]
        public string CardID { get; set; }


        public string sts { get; set; }
    }
}