using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace InternetBanking_v1.Models
{
    public class LockScreenViewModel
    {
        public String Username { get; set; }


        [Required(ErrorMessage = "Please enter the Password")]
        public String Password { get; set; }

    }
}
