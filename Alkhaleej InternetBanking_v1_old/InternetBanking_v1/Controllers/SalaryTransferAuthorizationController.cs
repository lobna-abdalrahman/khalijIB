using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers
{
    public class SalaryTransferAuthorizationController : BaseController//Controller
    {
        DataSource data = new DataSource();
        public static int countrow = 0, secccount = 0;
        public static double totalamount = 0.0;
        MyChequeStatus obj = new MyChequeStatus();
        //
        // GET: /SalaryTransferAuthorization/
        public ActionResult ViewSalaryTransfers()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<SalaryAuthViewModel> accas = new List<SalaryAuthViewModel>();
            accas = data.getSaleryFiles();

          

            return View(accas);
        }


        public ActionResult DoSalaryTransfer(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string fileID = id.ToString();
            Session["FileID"] = fileID;

            string user_id = Session["UserID"].ToString();
            string fileName = data.getSalaryFileName(user_id, fileID);   //Get FileName from DB
            Session["excelFileName"] = fileName;

            List<SalaryAuthViewModel> accas = new List<SalaryAuthViewModel>();
            accas = data.getSalariesAuth(user_id, fileName);
            Session["salarylist"] = accas;

            List<SalaryAuthViewModel> totalAndRows = new List<SalaryAuthViewModel>();
            totalAndRows = data.getTotalAndNoOfRows(fileID);

            foreach (var item in totalAndRows)
            {
                ViewBag.Total = item.FileTotal;
                ViewBag.Rows = item.NoOfRows;

            }

            return View(accas);

        }



        public ActionResult MultiTransfer(SalaryAuthViewModel model)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();
            string fileName = Session["excelFileName"].ToString();

            List<SalaryAuthViewModel> accas = new List<SalaryAuthViewModel>();
            accas = (List<SalaryAuthViewModel>)Session["salarylist"];

            String actto = "", actfrom = "", amount = "";
            foreach (var item in accas)
            {
                actto = actto + "-" + item.salary_account_no;
                amount = amount + "-" + item.salary_amount;
                actfrom = item.salary_comp_act;
            }

            List<SalaryAuthViewModel> successfulTrans = new List<SalaryAuthViewModel>();

            try
            {
                string res = obj.DomultiAccountTransfer(actfrom, actto.Substring(1), amount.Substring(1));
                JObject jobj = new JObject();
                jobj = JObject.Parse(res);
                dynamic result = jobj;




                var errorCode = result.errorcode;
                var errormsg = result.errormsg;
                var tranRes = result.status;

                string[] separators = { "-" };
                string value = tranRes;
                string[] acc = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);



                for (var i = 0; i < acc.Length; i++)
                {
                    String act, status;
                    act = acc[i];
                    status = acc[i + 1];
                    successfulTrans.Add(
                        new SalaryAuthViewModel
                        {
                            successfulAccount = act,
                            successfulStatus = status

                        });
                    data.updatesalaryitems(user_id, fileName, act, status);
                    i = i + 1;
                }

                //insert into TranLog
                string req = actfrom + actto.Substring(1) + amount.Substring(1);
                data.InsertTranLog(user_id, "SalaryTransfer", req, res, errormsg.ToString(), tranRes.ToString(), amount.Substring(1),"N/A");



                if (tranRes != null)
                {
                    ModelState.AddModelError("", tranRes.ToString());
                }



            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                ModelState.AddModelError("", e.Message);
                //insert into TranLog
                string req = actfrom + actto.Substring(1) + amount.Substring(1);
                data.InsertTranLog(user_id, "SalaryTransfer", req, "", "failed", "",amount.Substring(1),"");
            }


            return View(successfulTrans);
        }

        public ActionResult RejectTransfer()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();
            string fileName = Session["excelFileName"].ToString();

            List<SalaryAuthViewModel> accas = new List<SalaryAuthViewModel>();
            accas = (List<SalaryAuthViewModel>)Session["salarylist"];

            int res = data.Rejectsalary(user_id, fileName);

            if (InternetBanking_v1.Helper.CultureHelper.IsRighToLeft())
            {
                return RedirectToAction("ViewSalaryTransfers", "SalaryTransferAuthorization",new{lang = "ar"});
            }
            else
            {
                return RedirectToAction("ViewSalaryTransfers", "SalaryTransferAuthorization");
            }

           

        }
	}
}