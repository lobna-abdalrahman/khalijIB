using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using IBLogic;
using InternetBanking_v1.Context;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace InternetBanking_v1.Controllers.Salary_Transfer
{
    public class SalaryTransferController : BaseController
    {
        public static int   countrow = 0, secccount = 0;
        public static double totalamount = 0.0;
        DataSource data = new DataSource();
        MyChequeStatus obj = new MyChequeStatus();
        //
        // GET: /SalaryTransfer/
        public ActionResult SalaryTransfer()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            countrow = 0; secccount = 0;
              totalamount = 0.0;
            string user_id = Session["UserID"].ToString();
            //model.AccountNo = dataObj.populateAccounts(user_id);
            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.DropStatementClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt32(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNo.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo.ToString()
                });
            }

            ViewBag.clientList = AccountList;
            ViewBag.showData = false;

            return View();
        }
        [HttpPost]
        public ActionResult SalaryTransfer(SalaryTransferViewModel model)
        {
            List<SalaryTransferViewModel> accas = new List<SalaryTransferViewModel>();

            string user_id = Session["UserID"].ToString();
            //model.AccountNo = dataObj.populateAccounts(user_id);
            List<AccountStatementViewModel> pcontent = new List<AccountStatementViewModel>();
            {
                pcontent = data.DropStatementClient(user_id);
            };
            List<SelectListItem> AccountList = new List<SelectListItem>();
            foreach (var item in pcontent)
            {
                var AccountNumber = Convert.ToInt32(item.AccountNo.Substring(13));
                var AccountType = data.getaccounttype(item.AccountNo.ToString().Substring(5, 5));
                var BranchName = data.getbranchnameenglish(item.AccountNo.ToString().Substring(2, 3));

                AccountList.Add(new SelectListItem
                {
                    Text = BranchName + " - " + AccountType + " - " + AccountNumber,
                    Value = item.AccountNo.ToString()
                });
            }

            ViewBag.clientList = AccountList;
            if (ModelState.IsValid)
            {
                ViewBag.showData = true;

                model.AccountNumber = Request["Clients"];
                string fileName = Request.Files["file"].FileName;
                Session["excelFileName"] = fileName;
                if (!fileName.Equals(""))
                {
                    DataSet ds = new DataSet();
                    if (Request.Files["file"].ContentLength > 0)
                    {
                        string fileExtension =
                            System.IO.Path.GetExtension(Request.Files["file"].FileName);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                            if (System.IO.File.Exists(fileLocation))
                            {
                                ModelState.AddModelError("", "File Already Uploaded");
                                //System.IO.File.Delete(fileLocation);

                            }
                            else
                            {
                                Request.Files["file"].SaveAs(fileLocation);
                                string excelConnectionString = string.Empty;
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                //connection String for xls file format.
                                if (fileExtension == ".xls")
                                {
                                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                            fileLocation +
                                                            ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                }
                                //connection String for xlsx file format.
                                else if (fileExtension == ".xlsx")
                                {
                                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                            fileLocation +
                                                            ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                }
                                //Create Connection to Excel work book and add oledb namespace
                                data.insertfilesalary(user_id, fileName);
                                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                                excelConnection.Open();
                                DataTable dt = new DataTable();

                                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                if (dt == null)
                                {
                                    return null;
                                }

                                String[] excelSheets = new String[dt.Rows.Count];
                                int t = 0;
                                //excel data saves in temp file here.
                                foreach (DataRow row in dt.Rows)
                                {
                                    excelSheets[t] = row["TABLE_NAME"].ToString();
                                    t++;
                                }
                                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                                {
                                    dataAdapter.Fill(ds);
                                }
                            }
                            
                        }
                        if (fileExtension.ToString().ToLower().Equals(".xml"))
                        {
                            string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }

                            Request.Files["FileUpload"].SaveAs(fileLocation);
                            XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                            // DataSet ds = new DataSet();
                            ds.ReadXml(xmlreader);
                            xmlreader.Close();
                        }
                        if(ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
                        {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //string conn = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                            //SqlConnection con = new SqlConnection(conn);
                            //string query = "Insert into Person(Name,Email,Mobile) Values('" +
                            //               ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                            //               "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                            //con.Open();
                            //SqlCommand cmd = new SqlCommand(query, con);
                            //cmd.ExecuteNonQuery();
                            //con.Close();
                            countrow = countrow + 1;
                           
                            int resp = -1;
                            String ACC = ds.Tables[0].Rows[i][0].ToString(); //Remeber to validate currency and acttype and branch...
                            String Amount = ds.Tables[0].Rows[i][1].ToString();
                            String tst = ds.Tables[0].Rows[i][2].ToString();
                            totalamount = totalamount + Convert.ToDouble(Amount);
                             resp= data.insertfilesalaryitems(user_id, fileName, ACC, Amount, model.AccountNumber);
                            if (resp ==1)
                            {
                                secccount = secccount + 1;

                            }

                        }
                        data.updatefilesalaryitems(user_id, fileName, countrow, totalamount, model.AccountNumber);
                    }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Error While processing File");
                        
                    }
                }
                else{
                    ModelState.AddModelError("", "Please Select Your File");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Check Your choise");

            }
            ViewBag.TotalAmount = totalamount;
            ViewBag.CountRows = countrow;
            ViewBag.SuccessfulCount = secccount;
            return View();
        }

        public ActionResult DoSalaryTransfer(SalaryTransferViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();
            string fileName = Session["excelFileName"].ToString();
            List<SalaryTransferViewModel> accas = new List<SalaryTransferViewModel>();
            accas = data.getSalaries(user_id, fileName);
            Session["salarylist"] = accas;
            return View(accas);
        }

        public ActionResult MultiTransfer(SalaryTransferViewModel model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string user_id = Session["UserID"].ToString();
            string fileName = Session["excelFileName"].ToString();
            List<SalaryTransferViewModel> accas = new List<SalaryTransferViewModel>();
            accas = (List<SalaryTransferViewModel>) Session["salarylist"];

            String actto = "", actfrom = "", amount = "";
            foreach (var item in accas)
            {
                actto = actto + "-" + item.salary_account_no;
                amount = amount + "-" + item.salary_amount;
                actfrom = item.salary_comp_act;
            }

            List<SalaryTransferViewModel> successfulTrans = new List<SalaryTransferViewModel>();
            try {
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
                        new SalaryTransferViewModel
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
                data.InsertTranLog(user_id, "SalaryTransfer", req, "", "failed", "", amount.Substring(1),"");
            }

            return View(successfulTrans);
        }
	}
}