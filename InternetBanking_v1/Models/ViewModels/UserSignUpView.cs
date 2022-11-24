using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InternetBanking_v1.Models.DB;

namespace InternetBanking_v1.Models.ViewModels
{
    public class UserSignUpView
    {
        [Key]
        public int SYSUserID { get; set; }

        public int LOOKUPRoleID { get; set; }

        public string RoleName { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        public string Gender { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Addres")]
        public string Address { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Mobile #")]
        public string Mobile { get; set; }

        public virtual IB_KhaleejBankEntities BankEntities { get; set; }

    }
}