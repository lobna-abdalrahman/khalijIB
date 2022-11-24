using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using InternetBanking_v1.App_LocalResources;
using InternetBanking_v1.Models;
using MailMessage = System.Web.Mail.MailMessage;
using MailPriority = System.Web.Mail.MailPriority;

namespace InternetBanking_v1.Controllers
{
    public class ContactUsController : BaseController
    {
        //
        // GET: /ContactUs/
        public ActionResult ContactUs()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsViewModel vm)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                    //from contact us page 
                    msz.To.Add("h.hafiz@al-khaleejbank.com");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "Mail.Al-Khaleejbank.com",
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential
                            ("suggestionsandcomplains@al-khaleejbank.com", "abc@khaleej!bank"),
                        EnableSsl = true
                    };


                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";*/

//------------------------------------------------------------------------------------------------------------------------------

                    String Mymessage = vm.Message;
                    string fromEmail = Session["email"].ToString();//This field will contain the FROM email
                    string ToEmail = "suggestionsandcomplains@al-khaleejbank.com";//This field will contain destination email            


                    System.Web.Mail.SmtpMail.SmtpServer = "al-khaleejbank.com";//exchange or smtp server goes here.

                    MailMessage mailmessage = new MailMessage();


                    mailmessage.From = vm.Email;
                    //  mailmessage.Bcc = fromEmail; // Session("user_mail").ToString();
                    mailmessage.To = ToEmail;// "ib@alnilebank.com"

                    mailmessage.Subject = "Ebank - application Customer  " + vm.Name + " - " + vm.Subject;
                    mailmessage.Body = Mymessage;

                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "suggestionsandcomplains@al-khaleejbank.com");
                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "abc@khaleej!bank");

                    System.Web.Mail.SmtpMail.Send(mailmessage);
                    mailmessage.Priority = MailPriority.High;
                    TempData["Success"] = true;

                    ViewBag.ResponseStat = "Successfully Sent";
                    ViewBag.ResponseMSG = "Thank you for Contacting us";

                }
                catch (Exception e)
                {
                    /*ModelState.Clear();
                    ViewBag.Message = string.Format(" Sorry we are facing Problem here {0}", e.Message);*/

                    ModelState.Clear();
                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Error";
                    ViewBag.ResponseMSG = "Problem while sending email";
                    ViewBag.Message = string.Format(" Sorry we are facing Problem here {0}", GlobalRes.ConnectivityErrorMessage);
                    
                }
            }


            return View();
        }
	}
}