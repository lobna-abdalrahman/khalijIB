using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.App_LocalResources;

namespace InternetBanking_v1.Models
{
    public class ManagePhonesViewModel
    {
        [Display(Name = "FavoritePhoneName", ResourceType = typeof(GlobalRes))]
        public string FavoritePhoneName { get; set; }

        [Display(Name = "FavoritePhoneNumber", ResourceType = typeof(GlobalRes))]
        public string FavoritePhoneNo { get; set; }

        [Display(Name = "FavoritePhoneID", ResourceType = typeof(GlobalRes))]
        public string FavoritePhoneID { get; set; }

        public string sts { get; set; }
    }
}