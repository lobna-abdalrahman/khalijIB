using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBanking_v1.Models
{
    public class SendUsViewModel
    {
        [Required(ErrorMessage = "Please fill your Message")]
        public string Message { get; set; }
    }
}