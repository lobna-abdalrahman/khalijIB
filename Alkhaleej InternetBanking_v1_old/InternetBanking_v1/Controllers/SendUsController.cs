using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using InternetBanking_v1.Models;
using MailMessage = System.Web.Mail.MailMessage;
using MailPriority = System.Web.Mail.MailPriority;


namespace InternetBanking_v1.Controllers
{
    public class SendUsController : BaseController
    {
        //
        // GET: /SendUs/
        public ActionResult SendUs()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            SendUsViewModel model = new SendUsViewModel();
            return View();
        }



        [HttpPost]
        public ActionResult SendUs(SendUsViewModel model)
        {

           
       
            try {  
                if (ModelState.IsValid) { 
 
                    //String Mymessage = model.Message.ToString();

                    //WebMail.SmtpServer = "Mail.Al-Khaleejbank.com";   
                //gmail port to send emails  
                //WebMail.SmtpPort = 587;  
                //WebMail.SmtpUseDefaultCredentials = true;  
                //sending emails with secure protocol  
                //WebMail.EnableSsl = false ;   
                //EmailId used to send emails from application  
                //WebMail.UserName = "suggestionsandcomplains@al-khaleejbank.com";
                //WebMail.Password = "abc@khaleej!bank";  
                
                //Sender email address.  
                   // WebMail.From = Session["email"].ToString();

                    //WebMail.Send(to: "suggestionsandcomplains@al-khaleejbank.com", subject: "InternetBankingSuggestion from:" + Session["email"].ToString(), body: Mymessage, cc: "h.hafiz@al-khaleejbank.com", isBodyHtml: false);
                    //ViewBag.Status = "Email Sent Successfully.";  
                  /*  var receiverEmail = new MailAddress("h.hafiz@al-khaleejbank.com", "Receiver");
                    var password = "abc@khaleej!bank";  
                    var sub = "Suggestion";
                    var body = Mymessage;  
                    var smtp = new SmtpClient {
                        Host = "Mail.Al-Khaleejbank.com",  
                        Port = 587,  
                        EnableSsl = true,  
                        DeliveryMethod = SmtpDeliveryMethod.Network,  
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("suggestionsandcomplains@al-khaleejbank.com", password)  
                    };  
                    using(var mess = new MailMessage(senderEmail, receiverEmail) {  
                        Subject = sub,  
                        Body = body  
                    }) {
                        ServicePointManager.ServerCertificateValidationCallback =
                            delegate(object s, X509Certificate certificate,
                                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            { return true; };
                        smtp.Send(mess);  
                    }  */


                    //////////////////////////////////
                    //string to = "suggestionsandcomplains@al-khaleejbank.com";

                    ////It seems, your mail server demands to use the same email-id in SENDER as with which you're authenticating. 
                    ////string from = "sender@domain.com";
                    //string from = Session["email"].ToString();

                    //string subject = "Suggestion";
                    //string body = Mymessage;
                    //MailMessage message = new MailMessage(from, to, subject, body);
                    //SmtpClient client = new SmtpClient("Mail.Al-Khaleejbank.com");
                    //client.UseDefaultCredentials = false;
                    //client.Credentials = new NetworkCredential("test@domain.com", "password");
                    //client.Send(message);

                    //TempData["Success"] = true;
                    ////ModelState.AddModelError("", tranRes.ToString());
                    //ViewBag.ResponseStat = "Successfully Sent";
                    //ViewBag.ResponseMSG = "Thank you for your Suggestion..";

//----------------------------------------------------------------------------------------------------------------------

                    String Mymessage = model.Message.ToString();

                    string fromEmail = Session["email"].ToString();//This field will contain the FROM email
                    string ToEmail = "suggestionsandcomplains@al-khaleejbank.com";//This field will contain destination email            


                    System.Web.Mail.SmtpMail.SmtpServer = "al-khaleejbank.com";//exchange or smtp server goes here.

                    MailMessage mailmessage = new MailMessage();



                    mailmessage.From = fromEmail; // Session("user_mail").ToString();
                    mailmessage.To = ToEmail;// "ib@alnilebank.com"

                    mailmessage.Subject = "Ebank - application Customer  ";
                    mailmessage.Body = Mymessage;

                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "suggestionsandcomplains@al-khaleejbank.com");
                    mailmessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "abc@khaleej!bank");

                    System.Web.Mail.SmtpMail.Send(mailmessage);
                    mailmessage.Priority = MailPriority.High;
                    ViewBag.Status = "Email Sent Successfully.";


                    TempData["Success"] = true;
                    //ModelState.AddModelError("", tranRes.ToString());
                    ViewBag.ResponseStat = "Successfully Sent";
                    ViewBag.ResponseMSG = "Thank you for your Suggestion..";

                    return View();  
                }
            }
            catch (Exception)
            {
                TempData["Success"] = true;
                //ModelState.AddModelError("", tranRes.ToString());
                ViewBag.ResponseStat = "Error";
                ViewBag.ResponseMSG =   "Problem while sending email, Please check details.";

            }  
            return View();  
        }  
    }	
}