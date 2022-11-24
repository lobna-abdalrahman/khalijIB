using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace InternetBanking_v1.Controllers
{
    public class forgetpasswordController : Controller
    {
        //
        // GET: /forgetpassword/
        public ActionResult ForgotPassword()
        {
            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {
            ////check user existance
            //var user = Membership.GetUser(UserName);
            //if (user == null)
            //{
            //    TempData["Message"] = "User Not exist.";
            //}
            //else
            //{
            //    //generate password token
            //    var token = WebSecurity.GeneratePasswordResetToken(UserName);
            //    //create url with above token
            //    var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = UserName, rt = token }, "http") + "'>Reset Password</a>";
            //    //get user emailid
            //    UsersContext db = new UsersContext();
            //    var emailid = (from i in db.UserProfiles
            //                   where i.UserName == UserName
            //                   select i.EmailId).FirstOrDefault();
            //    //send mail
            //    string subject = "Password Reset Token";
            //    string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it
            //    try
            //    {
            //        SendEMail(emailid, subject, body);
            //        TempData["Message"] = "Mail Sent.";
            //    }
            //    catch (Exception ex)
            //    {
            //        TempData["Message"] = "Error occured while sending email." + ex.Message;
            //    }
            //    //only for testing
            //    TempData["Message"] = resetLink;
            //}

            return View();
        }

        //[HttpPost]
        //public ActionResult ForgotPassword(string EmailID)
        //{
        //    string resetCode = Guid.NewGuid().ToString();
        //    var verifyUrl = "/Account/ResetPassword/" + resetCode;
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
        //    using (var context = new LoginRegistrationInMVCEntities())
        //    {
        //        var getUser = (from s in context.RegisterUsers where s.Email == EmailID select s).FirstOrDefault();
        //        if (getUser != null)
        //        {
        //            getUser.ResetPasswordCode = resetCode;

        //            //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 

        //            context.Configuration.ValidateOnSaveEnabled = false;
        //            context.SaveChanges();

        //            var subject = "Password Reset Request";
        //            var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

        //                 " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
        //                 "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

        //            SendEmail(getUser.Email, body, subject);

        //            ViewBag.Message = "Reset password link has been sent to your email id.";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "User doesn't exists.";
        //            return View();
        //        }
        //    }

        //    return View();
        //}

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("youremail@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("youremail@gmail.com", "YourPassword");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

	}
}