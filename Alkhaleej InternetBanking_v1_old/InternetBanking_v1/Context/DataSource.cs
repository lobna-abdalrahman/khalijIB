using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using InternetBanking_v1.Models.Account;
using System.Data.OracleClient;
using InternetBanking_v1.Models;
using InternetBanking_v1.Models.DasboardModels;
using InternetBanking_v1.Models.ViewModels;
using OracleCommand = System.Data.OracleClient.OracleCommand;

namespace InternetBanking_v1.Context
{
    public class DataSource
    {
        //ConnectionString....
        private string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //-----------------------GET Accounts------------------------------------------------------
        //
        public String getaccount(string user_id)
        {
            String Accounts = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {

                        if (dataReader["acc_id"] != DBNull.Value)
                        {

                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                Accounts = Accounts + "-" + (string) dataReader["acc_no"];
                                //Accounts = Accounts.Substring(2);
                            }

                        }
                    }
                    Accounts = Accounts.Substring(1);
                    return Accounts;

                }

            }

        }

//---------------------------------------------------------get act --------------------------------//
        public String getspfaccount(string user_id, string act)
        {
            String Accounts = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where user_id =" + user_id +
                               " and substr(acc_no,14)=" + act;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {

                        if (dataReader["acc_id"] != DBNull.Value)
                        {

                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                Accounts = (string) dataReader["acc_no"];
                                //Accounts = Accounts.Substring(2);
                            }

                        }
                    }

                    return Accounts;

                }

            }

        }

        //-----------------------DropDownGET Branchs------------------------------------------------------
        //
        public List<ToBanCustomerViewModel> GetBranchs()
        {
            string branchs = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " select branch_code,branch_name from branchs where branch_sts = '1'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["branch_code"] != DBNull.Value)
                        {
                            obj.BranchCode = (string) dataReader["branch_code"];

                            if (dataReader["branch_name"] != DBNull.Value)
                            {
                                obj.BranchName = (string) dataReader["branch_name"];

                            }

                            list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return list;
                }
            }
        }

        //----------------------DropDownGet Account Type---------------------------------
        public List<ToBanCustomerViewModel> GetAccountType()
        {
            string branchs = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "select act_type_code,act_name from Act_types";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["act_type_code"] != DBNull.Value)
                        {
                            obj.AccountTypeCode = (string) dataReader["act_type_code"];

                            if (dataReader["act_name"] != DBNull.Value)
                            {
                                obj.AccountTypeName = (string) dataReader["act_name"];

                            }

                            list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return list;
                }
            }
        }
        public List<ToBanCustomerViewModel> InvGetAccountType()
        {
            string branchs = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "select act_type_code,act_name from invAct_types";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["act_type_code"] != DBNull.Value)
                        {
                            obj.AccountTypeCode = (string)dataReader["act_type_code"];

                            if (dataReader["act_name"] != DBNull.Value)
                            {
                                obj.AccountTypeName = (string)dataReader["act_name"];

                            }

                            list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return list;
                }
            }
        }

        //------------------DropDown Get Currency---------------------------
        public List<ToBanCustomerViewModel> GetCurrency()
        {
            string branchs = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "select curr_code,curr_name from currency";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["curr_code"] != DBNull.Value)
                        {
                            obj.CurrencyCode = (string) dataReader["curr_code"];

                            if (dataReader["curr_name"] != DBNull.Value)
                            {
                                obj.CurrencyName = (string) dataReader["curr_name"];

                            }

                            list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return list;
                }
            }
        }

        //------------------DropDown Get SDGCurrency---------------------------
        public List<ToBanCustomerViewModel> GetSDGCurrency()
        {
            string branchs = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "select curr_code,curr_name from currency where curr_code = '001'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["curr_code"] != DBNull.Value)
                        {
                            obj.CurrencyCode = (string)dataReader["curr_code"];

                            if (dataReader["curr_name"] != DBNull.Value)
                            {
                                obj.CurrencyName = (string)dataReader["curr_name"];

                            }

                            list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return list;
                }
            }
        }
        public String GetCurrencyName(string currencycode)
        {
            string branchs = "", CurrencyName="", CurrencyCode="";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "select curr_code,curr_name from currency where curr_code = '" + currencycode + "'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                //List<ToBanCustomerViewModel> list = new List<ToBanCustomerViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ToBanCustomerViewModel obj = new ToBanCustomerViewModel();

                        if (dataReader["curr_code"] != DBNull.Value)
                        {
                             CurrencyCode = (string)dataReader["curr_code"];

                            if (dataReader["curr_name"] != DBNull.Value)
                            {
                                CurrencyName = (string)dataReader["curr_name"];

                            }

                            //list.Add(obj);

                        }
                    }
                    //branchs = branchs.Substring(1);
                    return CurrencyName;
                }
            }
        }

        //-----------------------GET AccountTypes------------------------------------------------------
        //
        public String getaccounttype(string acctype)
        {
            //String acctypename = "NULL";
            //using (OracleConnection con = new OracleConnection(conString))
            //{
            //    string query = " select act_name from act_types where act_type_code=" + acctype;

            //    OracleCommand cmd = new OracleCommand(query, con);

            //    con.Open();


            //    using (IDataReader dataReader = cmd.ExecuteReader())
            //    {
            //        while (dataReader.Read())
            //        {



            //            if (dataReader["act_name"] != DBNull.Value)
            //            {
            //                acctypename = (string) dataReader["act_name"];
            //            }

            //        }
            //    }

            //    return acctypename;
            String acctypename = "NULL";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " select act_name from act_types where act_type_code=" + acctype;
                string query2 = "select act_name from invact_types where act_type_code=" + acctype;



                OracleCommand cmd = new OracleCommand(query, con);
                OracleCommand cmd2 = new OracleCommand(query2, con);
                con.Open();


                OracleDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["act_name"] != DBNull.Value)
                        {
                            acctypename = (string)dataReader["act_name"];
                        }

                    }
                }
                else
                {
                    dataReader = cmd2.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["act_name"] != DBNull.Value)
                            {
                                acctypename = (string)dataReader["act_name"];
                            }

                        }
                    }
                    else
                    { acctypename = "Account Type Not Found"; }
                }



                return acctypename;

            }

        }


        //-----------------------GET BRANCH NAME English------------------------------------------------------
        //
        public String getbranchnameenglish(string brcode)
        {
            String brname = "NULL";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " select  branch_name from branchs where branch_sts='1' and branch_code=" + brcode;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {



                        if (dataReader["branch_name"] != DBNull.Value)
                        {
                            brname = (string) dataReader["branch_name"];
                        }

                    }
                }

                return brname;

            }

        }




        //-------------------------------------DropClient for ChequeStatus Controller DropDownList--------------------------------
        //
        public List<MyAccounts> DropClient(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<MyAccounts> list = new List<MyAccounts>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        MyAccounts obj = new MyAccounts();
                        if (dataReader["acc_id"] != DBNull.Value)
                        {
                            if (dataReader["acc_id"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.AccountID = Convert.ToInt32(dataReader["acc_id"]);
                            }
                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                obj.AccountNumber = (string) dataReader["acc_no"];
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }



        //---------------------test pr--------------------------------------------------------------//
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="FILE_NAME"></param>
        /// <returns></returns>
        public int insertfilesalary(string user_id, string FILE_NAME)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO salary_files (FILE_ID,FILE_NAME,NO_OF_ROWS,STATUS,NO_OF_PROCESS_ROWS,FILE_DATE,USER_ID,FILE_TOTAL) " +
                    "VALUES(salaryfile.nextval,'" + FILE_NAME + "','0','P','0',sysdate," + user_id + ",'0')";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }

        //----------------------- INSERT FiLE SALARY ITEMS----------------------------------------------
        //
        public int insertfilesalaryitems(string user_id, string FILE_NAME, string acc, string amount, string acccomp)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO salary_temp (SALARY_ID,SALARY_USER_ID,SALARY_ACCOUNT_NO,SALARY_AMOUNT,SALARY_STATUS,SALARY_FILE_NAME,SALARY_COMP_ACT,SALARY_PROCESS_DATE,SALARY_REQ_DATE)" +
                    "VALUES(salarytemp.nextval," + user_id + ",'" + acc + "','" + amount + "','P','" + FILE_NAME +
                    "','" + acccomp + "',sysdate,sysdate)";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }

        //----------------------- update FiLE SALARY ITEMS----------------------------------------------
        //
        public int updatesalaryitems(string user_id, string FILE_NAME, string acc, string sts)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update salary_temp  set SALARY_STATUS ='" + sts +
                               "', SALARY_PROCESS_DATE=sysdate where SALARY_USER_ID=" + user_id +
                               " and SALARY_ACCOUNT_NO='" + acc + "' and SALARY_FILE_NAME='" + FILE_NAME + "'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }

        //----------------------------------DropClient for Statement Controller DropDownList----------------------------------------
        //
        public List<AccountStatementViewModel> DropStatementClient(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<AccountStatementViewModel> list = new List<AccountStatementViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        AccountStatementViewModel obj = new AccountStatementViewModel();
                        if (dataReader["acc_id"] != DBNull.Value)
                        {
                            if (dataReader["acc_id"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.AccountID = Convert.ToInt32(dataReader["acc_id"]);
                            }
                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                obj.AccountNo = (string) dataReader["acc_no"];
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }


        //----------------------------------getCurrentAccounts----------------------------------------
        //
        public List<AccountStatementViewModel> getCurrentAccounts(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "SELECT acc_id,acc_no from user_acc_link where substr(acc_no,6,5) in ('20105','20101') and user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<AccountStatementViewModel> list = new List<AccountStatementViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        AccountStatementViewModel obj = new AccountStatementViewModel();
                        if (dataReader["acc_id"] != DBNull.Value)
                        {
                            if (dataReader["acc_id"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.AccountID = Convert.ToInt32(dataReader["acc_id"]);
                            }
                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                obj.AccountNo = (string)dataReader["acc_no"];
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }


        //--------------------------------------DropClient for Cards---------------------------------
        //
        public List<CardTransferViewModel> DropCardClient(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "SELECT card_name,card_number from user_card_link where user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<CardTransferViewModel> list = new List<CardTransferViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        CardTransferViewModel obj = new CardTransferViewModel();
                        if (dataReader["card_name"] != DBNull.Value)
                        {
                            if (dataReader["card_name"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.CardName = (string) dataReader["card_name"];
                            }
                            if (dataReader["card_number"] != DBNull.Value)
                            {
                                obj.CardNumber = (string) dataReader["card_number"];
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }

        //----------------------------------------------------------------------------------
        public List<NonCustomerTransferViewModel> NonCustomerTransferAccount(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where ACC_CURR='001' and user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<NonCustomerTransferViewModel> list = new List<NonCustomerTransferViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        NonCustomerTransferViewModel obj = new NonCustomerTransferViewModel();
                        if (dataReader["acc_id"] != DBNull.Value)
                        {
                            if (dataReader["acc_id"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.AccountID = Convert.ToInt32(dataReader["acc_id"]);
                            }
                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                obj.FromAccount = (string)dataReader["acc_no"];
                                 
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }

        //-----------------------------------DropClient for OwnTransfer---------------------
        public List<OwnTransferViewModel> DropFromOwnTransferClient(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT acc_id,acc_no from user_acc_link where  ACC_CURR='001' and  user_id =" + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<OwnTransferViewModel> list = new List<OwnTransferViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        OwnTransferViewModel obj = new OwnTransferViewModel();
                        if (dataReader["acc_id"] != DBNull.Value)
                        {
                            if (dataReader["acc_id"] != DBNull.Value)
                            {
                                //obj.AccountID = (int)dataReader["acc_id"];
                                obj.AccountID = Convert.ToInt32(dataReader["acc_id"]);
                            }
                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                obj.FromAccount = (string) dataReader["acc_no"];
                                obj.ToAccount = (string) dataReader["acc_no"];
                            }
                            list.Add(obj);
                        }
                    }

                    return list;

                }

            }

        }


        /// <summary>
        /// This will insert the user-login activity
        /// If it is a succesful activity 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public int insertSuccessUserLoginActivity(string username, string password, string IPAddress, string userID)
        {

            using (OracleConnection con = new OracleConnection(conString))
            {
                string query,query2;
                query = "insert into admin_login values" + " ('" + IPAddress + "','" +
                               DateTime.Today.ToString() + "','" + username + "','-', 'S', '" + userID + "' )";

                OracleCommand cmd = new OracleCommand(query, con);
                if (con.State.Equals(ConnectionState.Closed))
                {
                    con.Open();
                }
                int result = -1;

                result = cmd.ExecuteNonQuery();
                cmd.Dispose();
               
                query2=" update   users set login_status ='1' where user_id='"+userID+ "'";
                  cmd = new OracleCommand(query2, con);

                if (con.State.Equals(ConnectionState.Closed))
                {
                    con.Open();
                }
                  result = -1;

                result = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                return result;
            }

        }

        public int  UserLogout(string userID)
        {

            using (OracleConnection con = new OracleConnection(conString))
            {
                string query;
                
                OracleCommand cmd;

                query = " update   users set login_status ='0' where user_id='" + userID + "'";
                cmd = new OracleCommand(query, con);

                if (con.State.Equals(ConnectionState.Closed))
                {
                    con.Open();
                }
               int result = -1;

                result = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                return result;
            }

        }

        /// <summary>
        /// This will insert the user-login activity
        /// If it is a Failed activity 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public int insertFailedUserLoginActivity(string username, string password, string IPAddress)
        {

            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "insert into admin_login values" + " ('" + IPAddress + "','" +
                               DateTime.Today.ToString() + "','" + username + "','-', 'F', 'N/A')";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;

                result = cmd.ExecuteNonQuery();



                return result;
            }

        }



        /// updates file sallary
        /// items in a table
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="countrow"></param>
        /// <param name="totalamount"></param>
        /// <param name="modelAccountNumber"></param>
        /// <returns></returns>
        /// //-----------------------------------updatefilesalaryitems---------------------
        public int updatefilesalaryitems(string userId, string fileName, int countrow, double totalamount,
            string modelAccountNumber)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update  salary_files  set  NO_OF_ROWS=" + countrow + ",STATUS='RWS',FILE_TOTAL=" +
                               totalamount +
                               " where FILE_NAME='" + fileName + "' and user_id=" + userId;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }

        /// <summary>
        /// /Get Salaries item by item 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// //-----------------------------------getSalaries---------------------
        public List<SalaryTransferViewModel> getSalaries(string user_id, string fileName)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select salary_account_no,salary_amount,salary_file_name,salary_comp_act from salary_temp WHERE " +
                    "salary_status = 'P' and salary_user_id = '" + user_id + "' and salary_file_name = '" + fileName +
                    "'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<SalaryTransferViewModel> list = new List<SalaryTransferViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        SalaryTransferViewModel obj = new SalaryTransferViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                        if (dataReader["salary_account_no"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.salary_account_no = (string) dataReader["salary_account_no"];
                        }
                        if (dataReader["salary_amount"] != DBNull.Value)
                        {
                            obj.salary_amount = (string) dataReader["salary_amount"];
                        }
                        if (dataReader["salary_file_name"] != DBNull.Value)
                        {
                            obj.salary_file_name = (string) dataReader["salary_file_name"];
                        }
                        if (dataReader["salary_comp_act"] != DBNull.Value)
                        {
                            obj.salary_comp_act = (string) dataReader["salary_comp_act"];
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }



        /// //-----------------------------------getSalariesAuth---------------------
        public List<SalaryAuthViewModel> getSalariesAuth(string user_id, string fileName)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select salary_account_no,salary_amount,salary_file_name,salary_comp_act from salary_temp WHERE " +
                    "salary_status = 'P' and salary_user_id = '" + user_id + "' and salary_file_name = '" + fileName +
                    "'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<SalaryAuthViewModel> list = new List<SalaryAuthViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        SalaryAuthViewModel obj = new SalaryAuthViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                        if (dataReader["salary_account_no"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.salary_account_no = (string)dataReader["salary_account_no"];
                        }
                        if (dataReader["salary_amount"] != DBNull.Value)
                        {
                            obj.salary_amount = (string)dataReader["salary_amount"];
                        }
                        if (dataReader["salary_file_name"] != DBNull.Value)
                        {
                            obj.salary_file_name = (string)dataReader["salary_file_name"];
                        }
                        if (dataReader["salary_comp_act"] != DBNull.Value)
                        {
                            obj.salary_comp_act = (string)dataReader["salary_comp_act"];
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }




        //-----------------------------------InsertTranLog---------------------
        /// <summary>
        /// Insert into Log 
        /// all the info about each transaction
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="tranName"></param>
        /// <param name="req"></param>
        /// <param name="resp"></param>
        /// <param name="status"></param>
        /// <param name="respResult"></param>
        /// <returns></returns>
        public int InsertTranLog(string user_id, string tranName, string req, string resp, string status,
            string respResult, string tranAmount, string Token)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO trans_log (TRAN_ID,TRAN_REQ,TRAN_RESP,TRAN_REQ_DATE,TRAN_RESP_DATE,TRAN_STATUS,TRAN_RESP_RESULT,USER_ID,TRAN_NAME, TRAN_AMOUNT,TRAN_TOKEN)" +
                    "VALUES(tranlog.nextval,'" + req + "','" + resp + "',sysdate,sysdate,'" + status + "','" +
                    respResult + "','" + user_id + "','" + tranName + "','" + tranAmount + "','" + Token + "')";
                //string query = "INSERT INTO trans_log (TRAN_ID,TRAN_REQ,TRAN_RESP,TRAN_REQ_DATE,TRAN_RESP_DATE)" +
                //               "VALUES(tranlog.nextval,'" + req + "','"+ resp +"',sysdate,sysdate,sysdate)";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;

                result = cmd.ExecuteNonQuery();



                return result;

            }
        }
        public int NECTranLog(string user_id, string tranName, string req, string resp, string status, string respResult, string cardNumber, string meterNumber, string nec_token, string tranAmount, string waterFees, string meterFees, string netAmount, string customerName, string unitsInKwh, string operatorMsg)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                /*string query =
                    "INSERT INTO trans_log (TRAN_ID,TRAN_REQ,TRAN_RESP,TRAN_REQ_DATE,TRAN_RESP_DATE,TRAN_STATUS,TRAN_RESP_RESULT,USER_ID,TRAN_NAME)" +
                    "VALUES(tranlog.nextval,'" + req + "','" + resp + "',sysdate,sysdate,'" + status + "','" +
                    respResult + "','" + user_id + "','" + tranName + "')";*/

                string query = "INSERT INTO trans_log_nec (TRAN_ID,TRAN_REQ,TRAN_RESP,TRAN_REQ_DATE,TRAN_RESP_DATE,TRAN_STATUS,TRAN_RESP_RESULT,USER_ID,TRAN_NAME,TRAN_PAN,TRAN_METER,TRAN_TOKEN,TRAN_AMOUNT,TRAN_WATERFEES,TRAN_METERFEES,TRAN_NETAMOUNT,TRAN_CUSTOMERNAME,TRAN_UNITSINKWH,TRAN_OPERTORMESSSAGE)"
                                            + "VALUES(tran_nec.nextval,'" + req + "','" + resp + "', sysdate, sysdate, '" + status + "','" + respResult + "','" + user_id + "','" + tranName + "','" + cardNumber + "','" + meterNumber + "','" + nec_token + "','" + tranAmount + "','" + waterFees + "','" + meterFees + "','" + netAmount +
                                            "','" + customerName + "','" + unitsInKwh + "','" + operatorMsg + "')";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;

                result = cmd.ExecuteNonQuery();



                return result;

            }
        }


        public int InsertTransferToTranLog(String user_id, String tranName, String req, String resp, String status,
            String respResult, String tranAmount, String Token, String sender, String reciever)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO trans_log (TRAN_ID,TRAN_REQ,TRAN_RESP,TRAN_REQ_DATE,TRAN_RESP_DATE,TRAN_STATUS,TRAN_RESP_RESULT,USER_ID,TRAN_NAME, TRAN_AMOUNT,TRAN_TOKEN, SENDER_NAME, RECIEVER_NAME)" +
                    "VALUES(tranlog.nextval,'" + req + "','" + resp + "',sysdate,sysdate,'" + status + "','" +
                    respResult + "','" + user_id + "','" + tranName + "','" + tranAmount + "','" + Token + "','"+ sender + "','" + reciever +"')";
             

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;

                result = cmd.ExecuteNonQuery();



                return result;

            }
        }

        //-----------------------------------getTransactions---------------------
        /// <summary>
        /// Get top 5 Transactions
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<LatestTransactions> getTransactions(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                //string query = "select tran_id,tran_name,tran_status,tran_resp_result from trans_log where user_id =" + user_id + "and ROWNUM <= 5 order by rownum desc";
                string query =
                    "select * from ( select tran_id , tran_name,tran_status,tran_resp_result,TRAN_AMOUNT from trans_log where user_id = '" +
                    user_id + "' order by tran_id desc ) where rownum <= 5";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<LatestTransactions> list = new List<LatestTransactions>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        LatestTransactions obj = new LatestTransactions();

                        if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.TranId = Convert.ToInt32(dataReader["tran_id"]);
                        }
                        if (dataReader["tran_name"] != DBNull.Value)
                        {
                            obj.TranName = (string) dataReader["tran_name"];
                        }
                        if (dataReader["tran_status"] != DBNull.Value)
                        {
                            obj.TranStatus = (string) dataReader["tran_status"];
                        }
                        if (dataReader["tran_resp_result"] != DBNull.Value)
                        {
                            obj.TranResult = (string) dataReader["tran_resp_result"];
                        }
                        if (dataReader["TRAN_AMOUNT"] != DBNull.Value)
                        {
                            obj.TranAmount = (string)dataReader["TRAN_AMOUNT"];
                        }
                        list.Add(obj);
                    }

                    return list;

                }

            }

        }



        //-----------------------GET Transfer Count------------------------------------------------------
        //
        public String GetTransferCount(string user_id)
        {
            String count = "NULL";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " select count(*) from trans_log where  user_id = " + user_id +
                               " and (tran_name = 'Own Transfer' or tran_name = 'To Bank Customer Transfer')";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {



                        if (dataReader["count(*)"] != DBNull.Value)
                        {
                            count = dataReader["count(*)"].ToString();
                        }

                    }
                }

                return count;

            }
        }


        //-----------------------GET Accounts Count------------------------------------------------------
        //
        public String GetAccountsCount(string user_id)
        {
            String count = "NULL";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = " SELECT count(*) from user_acc_link where user_id = " + user_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {



                        if (dataReader["count(*)"] != DBNull.Value)
                        {
                            count = dataReader["count(*)"].ToString();
                        }

                    }
                }

                return count;

            }
        }



        //---------------------Get Transfers Only---------------------------------
        /// <summary>
        ///
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<AllTransfersViewModel> getMyTransfers(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                //string query = "select tran_id,tran_name,tran_status,tran_resp_result from trans_log where user_id =" + user_id + "and ROWNUM <= 5 order by rownum desc";
                //string query =
                //    "select * from ( select tran_id , tran_name,tran_status,tran_resp_result from trans_log where user_id = '" +
                //    user_id + "' order by tran_id desc ) where rownum <= 5";

                string query = "select tran_id, tran_name, tran_status, tran_resp_result, tran_amount,SENDER_NAME,RECIEVER_NAME,tran_req_date from trans_log where  user_id =" + user_id +
                    " and (tran_name = 'Own Transfer' or tran_name = 'To Bank Customer Transfer') order by tran_id desc";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<AllTransfersViewModel> list = new List<AllTransfersViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        AllTransfersViewModel obj = new AllTransfersViewModel();

                        if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.TranId = Convert.ToInt32(dataReader["tran_id"]);
                        }
                        if (dataReader["tran_name"] != DBNull.Value)
                        {
                            obj.TranName = (string)dataReader["tran_name"];
                        }
                        if (dataReader["tran_status"] != DBNull.Value)
                        {
                            obj.TranStatus = (string)dataReader["tran_status"];
                        }
                        if (dataReader["tran_resp_result"] != DBNull.Value)
                        {
                            obj.TranResult = (string)dataReader["tran_resp_result"];
                        }
                        if (dataReader["tran_amount"] != DBNull.Value)
                        {
                            obj.TranAmount = (string)dataReader["tran_amount"];
                        }

                        if (dataReader["SENDER_NAME"] != DBNull.Value)
                        {
                            obj.SenderName = (string)dataReader["SENDER_NAME"];
                        }
                        if (dataReader["RECIEVER_NAME"] != DBNull.Value)
                        {
                            obj.ReciverName = (string)dataReader["RECIEVER_NAME"];
                        }
                        if (dataReader["tran_req_date"] != DBNull.Value)
                        {
                            obj.TranDate = obj.TranDate = ((DateTime)dataReader["tran_req_date"]).ToString("dd/MM/yyyy");
                        }
                        
                        list.Add(obj);
                    }

                    return list;

                }

            }

        }

        //---------------------Get All Transactions---------------------------------
        public List<AllTransfersViewModel> getAllTransactions(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                //string query = "select tran_id,tran_name,tran_status,tran_resp_result from trans_log where user_id =" + user_id + "and ROWNUM <= 5 order by rownum desc";
                //string query =
                //    "select * from ( select tran_id , tran_name,tran_status,tran_resp_result from trans_log where user_id = '" +
                //    user_id + "' order by tran_id desc ) where rownum <= 5";

                string query = "select tran_id, tran_name,tran_amount,tran_token, tran_status, tran_resp_result,tran_req_date from trans_log where  user_id =" + user_id +
                               "order by tran_id desc";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<AllTransfersViewModel> list = new List<AllTransfersViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        AllTransfersViewModel obj = new AllTransfersViewModel();

                        if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.TranId = Convert.ToInt32(dataReader["tran_id"]);
                        }
                        if (dataReader["tran_name"] != DBNull.Value)
                        {
                            obj.TranName = (string)dataReader["tran_name"];
                        }
                        if (dataReader["tran_amount"] != DBNull.Value)
                        {
                            obj.TranAmount = (string)dataReader["tran_amount"];
                        }
                        if (dataReader["tran_token"] != DBNull.Value)
                        {
                            obj.TranToken = (string)dataReader["tran_token"];
                        }
                        if (dataReader["tran_status"] != DBNull.Value)
                        {
                            obj.TranStatus = (string)dataReader["tran_status"];
                        }
                        if (dataReader["tran_resp_result"] != DBNull.Value)
                        {
                            obj.TranResult = (string)dataReader["tran_resp_result"];
                        }
                        if (dataReader["tran_req_date"] != DBNull.Value)
                        {
                            obj.TranDate = ((DateTime)dataReader["tran_req_date"]).ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        list.Add(obj);
                    }

                    return list;

                }

            }

        }

        //----------------------------------Update User Information-----------------------------------------
        public int updateUserInfo(string user_id, string email,string address)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update users " +// set user_name = '" + userName + 
                    "set user_email ='" + email + 
                    //"', user_mobile = '" + mobile + 
                    "', user_adrs = '" + address + 
                    "' where user_id=" + user_id;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }


        //----------------------------------Update User Password-----------------------------------------
        /// <summary>
        /// ///
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>if success(1).. or not (-1)</returns>
        public int updateUserPassword(string user_id, string oldPassword, string newPassword)
        { int result = -1;
            try 
	                   {	        
		
            using (OracleConnection con = new OracleConnection(conString))
            {
                int id = Convert.ToInt32(user_id);
                string query = "update users " +// set user_name = '" + userName + 
                               "set user_pwd ='" + newPassword +
                               "' where user_id='" + id +
                               "' and user_pwd = '" + oldPassword+"'";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
               
                result = cmd.ExecuteNonQuery();

            }
	}
	catch (Exception ex)
	{
		
		   result = -1;
	}

                return result;

            
        }


        //------------------------First Login Update Password---------------------------------------

        public int FirstLoginUpdateUserPassword(string user_id, string oldPassword, string newPassword)
        {
            int result = -1;
            try
            {
                using (OracleConnection con = new OracleConnection(conString))
                {
                    int id = Convert.ToInt32(user_id);
                    string query = "update users " +// set user_name = '" + userName + 
                                   "set user_pwd ='" + newPassword +
                                   "', first_login = 'F' " +
                                   "where user_id='" + id +
                                   "' and user_pwd = '" + oldPassword + "'";


                    OracleCommand cmd = new OracleCommand(query, con);

                    con.Open();

                    result = cmd.ExecuteNonQuery();





                }
            }
            catch (Exception ex)
            {

                result = -1;
            }
            return result;
        }



        public int InsertChequeReq(string user_id, string accountNo, string size)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO cheque_reqs (REQUEST_ID,ACCOUNT_NO,USER_ID,REQUESTED_SIZE,REQ_DATE,REQ_STATUS,REQ_REASON) " +
                    "VALUES(cheque_req_seq.nextval,'" + accountNo + "','" + user_id + "','" + size +
                    "',sysdate,'process', '')";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;

                result = cmd.ExecuteNonQuery();



                return result;

            }
        }



        //----------------------- INSERT Corprate Transfer ITEMS----------------------------------------------
        //
        public int insertTransferTemp(string user_id, string fromAccount, string toAccount, string amount, string username)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                int UserID = Convert.ToInt32(user_id);
                string query =
                    "INSERT INTO transfer_temp (TRAN_ID,TRANSFER_USER_ID,FROM_ACCOUNT,TO_ACCOUNT,TRANSFER_AMOUNT,TRANSFER_DATE,TRANSFER_STATUS,USERNAME)" +
                    "VALUES(transfer_temp_seq.nextval," + UserID + ",'" + fromAccount + "','" + toAccount + "','" +
                    amount + "', sysdate,'Pending', '" + username + "')";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }

        }


        /// //-----------------------------------get Un-Authorized Transfers---------------------
        public List<TransferAuthorizeViewModel> getUnAuthorizedTransfers()
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select tran_id,transfer_user_id,username,from_account,to_account,transfer_amount,transfer_status,transfer_date from transfer_temp where transfer_status = 'Pending'";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<TransferAuthorizeViewModel> list = new List<TransferAuthorizeViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        TransferAuthorizeViewModel obj = new TransferAuthorizeViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                        if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.transferID = dataReader["tran_id"].ToString();
                        }
                        if (dataReader["transfer_user_id"] != DBNull.Value)
                        {
                            obj.UserID = dataReader["transfer_user_id"].ToString(); 
                        }
                        if (dataReader["username"] != DBNull.Value)
                        {
                            obj.UserName = (string)dataReader["username"];
                        }
                        if (dataReader["from_account"] != DBNull.Value)
                        {
                            obj.FromAccountNumber = (string)dataReader["from_account"];
                            String fromact=(string)dataReader["from_account"];
                            var FromAccountNumber =  fromact.Substring(13);
                            var FromAccountType = getaccounttype(fromact.Substring(5, 5));
                            var FromBranchName = getbranchnameenglish(fromact.Substring(2, 3));
                            var  Fromcurrname=GetCurrencyName(fromact.Substring(10,3));

                            obj.FromAccountName = FromBranchName + "-" + FromAccountType + "-" + Fromcurrname+"-"+FromAccountNumber;
                        }
                        if (dataReader["to_account"] != DBNull.Value)
                        {
                            obj.ToAccountNumber = (string)dataReader["to_account"];
                            String toact = (string)dataReader["to_account"];
                            var ToAccountNumber =toact.Substring(13);
                            var ToAccountType = getaccounttype(toact.Substring(5, 5));
                            var ToBranchName =  getbranchnameenglish(toact.Substring(2, 3));
                            var TocurrName = GetCurrencyName(toact.Substring(10, 3));
                            obj.ToAccountName = ToBranchName + "-" + ToAccountType + "-" + TocurrName + "-" + ToAccountNumber;
                        }
                        if (dataReader["transfer_amount"] != DBNull.Value)
                        {
                            obj.TransferAmount = (string)dataReader["transfer_amount"];
                        }
                        if (dataReader["transfer_status"] != DBNull.Value)
                        {
                            obj.TransferStatus = (string)dataReader["transfer_status"];
                        }
                        if (dataReader["transfer_date"] != DBNull.Value)
                        {
                            obj.TransferDate = ((DateTime)dataReader["transfer_date"]).ToString("dd/MM/yyyy");
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }

        //---------------------Get All Transactions---------------------------------
        public List<ElectricityViewModel> getElectricityTransactions(string user_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {


                string query = "select tran_id,tran_pan,tran_meter,tran_amount,tran_netamount,tran_status,tran_token,tran_waterfees,tran_meterfees,tran_customername,tran_unitsinkwh,tran_opertormesssage  from trans_log_nec where user_id =" + user_id +
                               "order by tran_id desc";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ElectricityViewModel> list = new List<ElectricityViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ElectricityViewModel obj = new ElectricityViewModel();

                        if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.tranID = Convert.ToInt32(dataReader["tran_id"]);
                        }
                        if (dataReader["tran_pan"] != DBNull.Value)
                        {
                            obj.CardNumber = (string)dataReader["tran_pan"];
                        }
                        if (dataReader["tran_meter"] != DBNull.Value)
                        {
                            obj.MeterNo = (string)dataReader["tran_meter"];
                        }
                        if (dataReader["tran_amount"] != DBNull.Value)
                        {
                            obj.TranAmount = (string)dataReader["tran_amount"];
                        }
                        if (dataReader["tran_netamount"] != DBNull.Value)
                        {
                            obj.netAmount = (string)dataReader["tran_netamount"];
                        }
                        if (dataReader["tran_status"] != DBNull.Value)
                        {
                            obj.tranStatus = (string)dataReader["tran_status"];
                        }
                        if (dataReader["tran_token"] != DBNull.Value)
                        {
                            obj.token = (string)dataReader["tran_token"];
                        }
                        if (dataReader["tran_waterfees"] != DBNull.Value)
                        {
                            obj.waterFees = (string)dataReader["tran_waterfees"];
                        }
                        if (dataReader["tran_meterfees"] != DBNull.Value)
                        {
                            obj.meterFees = (string)dataReader["tran_meterfees"];
                        }
                        if (dataReader["tran_customername"] != DBNull.Value)
                        {
                            obj.customerName = (string)dataReader["tran_customername"];
                        }
                        if (dataReader["tran_unitsinkwh"] != DBNull.Value)
                        {
                            obj.unitsInKwh = (string)dataReader["tran_unitsinkwh"];
                        }
                        if (dataReader["tran_opertormesssage"] != DBNull.Value)
                        {
                            obj.operatorMessage = (string)dataReader["tran_opertormesssage"];
                        }

                        list.Add(obj);
                    }

                    return list;

                }

            }

        }


        //---------------------------------------------------------get Specific Transfer Record --------------------------------//
        public List<TransferAuthorizeViewModel> getSpecificTransfer(string tranID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select transfer_user_id,username,from_account,to_account,transfer_amount,transfer_status,transfer_date from transfer_temp where tran_id =" +
                    tranID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<TransferAuthorizeViewModel> list = new List<TransferAuthorizeViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        TransferAuthorizeViewModel obj = new TransferAuthorizeViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                       /* if (dataReader["tran_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.transferID = dataReader["tran_id"].ToString();
                        }*/
                        if (dataReader["transfer_user_id"] != DBNull.Value)
                        {
                            obj.UserID = dataReader["transfer_user_id"].ToString();
                        }
                        if (dataReader["username"] != DBNull.Value)
                        {
                            obj.UserName = (string)dataReader["username"];
                        }
                        if (dataReader["from_account"] != DBNull.Value)
                        {
                            obj.FromAccountNumber = (string)dataReader["from_account"];
                        }
                        if (dataReader["to_account"] != DBNull.Value)
                        {
                            obj.ToAccountNumber = (string)dataReader["to_account"];
                        }
                        if (dataReader["transfer_amount"] != DBNull.Value)
                        {
                            obj.TransferAmount = (string)dataReader["transfer_amount"];
                        }
                        if (dataReader["transfer_status"] != DBNull.Value)
                        {
                            obj.TransferStatus = (string)dataReader["transfer_status"];
                        }
                        if (dataReader["transfer_date"] != DBNull.Value)
                        {
                            obj.TransferDate = ((DateTime)dataReader["transfer_date"]).ToString("dd/MM/yyyy");
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }
        /*public String getSpecificTransfer(string tranID)
        {
            String Accounts = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    " select tran_id,transfer_user_id,username,from_account,to_account,transfer_amount,transfer_status,transfer_date from transfer_temp where tran_id =" +
                    tranID;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {

                        if (dataReader["acc_id"] != DBNull.Value)
                        {

                            if (dataReader["acc_no"] != DBNull.Value)
                            {
                                Accounts = (string)dataReader["acc_no"];
                                //Accounts = Accounts.Substring(2);
                            }

                        }
                    }

                    return Accounts;

                }

            }

        }*/

      /*  public int insertfilesalaryitems(string user_id, string FILE_NAME, string acc, string amount, string acccomp)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "INSERT INTO salary_temp (SALARY_ID,SALARY_USER_ID,SALARY_ACCOUNT_NO,SALARY_AMOUNT,SALARY_STATUS,SALARY_FILE_NAME,SALARY_COMP_ACT,SALARY_PROCESS_DATE,SALARY_REQ_DATE)" +
                    "VALUES(salarytemp.nextval," + user_id + ",'" + acc + "','" + amount + "','P','" + FILE_NAME +
                    "','" + acccomp + "',sysdate,sysdate)";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }*/

        //----------------------- updateTransferStatus----------------------------------------------
        //
        public int updateTransferStatus(string tranID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
               /* string queryy = "update salary_temp  set SALARY_STATUS ='" + sts +
                               "', SALARY_PROCESS_DATE=sysdate where SALARY_USER_ID=" + user_id +
                               " and SALARY_ACCOUNT_NO='" + acc + "' and SALARY_FILE_NAME='" + FILE_NAME + "'";*/


                string query = "update transfer_temp set transfer_status = 'completed' where tran_id = " + tranID;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }




        //----------------------- update FiLE SALARY ITEMS----------------------------------------------
        //
        public int updateLastUnsuccessfulLogin(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                /* string queryy = "update salary_temp  set SALARY_STATUS ='" + sts +
                                "', SALARY_PROCESS_DATE=sysdate where SALARY_USER_ID=" + user_id +
                                " and SALARY_ACCOUNT_NO='" + acc + "' and SALARY_FILE_NAME='" + FILE_NAME + "'";*/


                string query = "update users set last_unsuccess_login = sysdate where user_id = " + userID;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }


        //----------------------- getUserInfo----------------------------------------------
        //
        public ProfileInfoViewModel getUserInfo(int userID)
        {
            ProfileInfoViewModel model = new ProfileInfoViewModel();
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlrt;
            sqlrt = "select  user_email,user_adrs from users where user_id=" + userID;
            using (OracleConnection conObj = new OracleConnection(conString))
            { conObj.Open();
                cmd=new OracleCommand(sqlrt,conObj);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while ((dr.Read()))
                    {
                        model.Email = dr[0].ToString();
                        model.Address = dr[1].ToString();
                        model.sts = "true";
                    }

                }
                else
                {

                    model.Email ="";
                    model.Address = "";
                    model.sts = "false";
                }
            conObj.Close();
            }

            return model;
            
        }

        /// //-----------------------------------get-ChequeBook-Request-Status---------------------
        public List<ChequeBookStatusViewModel> getChequeBookRequestsStatus(string userID, string fromDate, string toDate)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select account_no,requested_size,req_date,req_status from cheque_reqs where user_id=" + userID + " and req_date between to_date('" + fromDate + "','dd/MM/yyyy') and to_date('" + toDate + "','dd/MM/yyyy')";
                    //"select tran_id,transfer_user_id,username,from_account,to_account,transfer_amount,transfer_status,transfer_date from transfer_temp where transfer_status = 'Pending'";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ChequeBookStatusViewModel> list = new List<ChequeBookStatusViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ChequeBookStatusViewModel obj = new ChequeBookStatusViewModel();


                        if (dataReader["account_no"] != DBNull.Value)
                        {
                            obj.AccountNumber = dataReader["account_no"].ToString();
                        }
                        if (dataReader["requested_size"] != DBNull.Value)
                        {
                            obj.RequestedSize = (string)dataReader["requested_size"];
                        }
                        if (dataReader["req_date"] != DBNull.Value)
                        {
                            obj.Date = ((DateTime)dataReader["req_date"]).ToString("dd/MM/yyyy");
                        }

                        if (dataReader["req_status"] != DBNull.Value)
                        {
                            obj.RequestStatus = (string)dataReader["req_status"].ToString();
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }

        /*8888888888888888888888888888888888888888888888888---Phone CRUD----88888888888888888888888888888888888888888888888888*/

        /// //-----------------------------------getFavPhones---------------------
        public List<TelecomPostPaidViewModel> getFavPhones(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select phone_id,phone_name,phone_number from user_phone_link where user_id = " + userID;



                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<TelecomPostPaidViewModel> list = new List<TelecomPostPaidViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        TelecomPostPaidViewModel obj = new TelecomPostPaidViewModel();


                        if (dataReader["phone_id"] != DBNull.Value)
                        {
                            obj.FavoritePhoneID = dataReader["phone_id"].ToString();
                        }
                        if (dataReader["phone_name"] != DBNull.Value)
                        {
                            obj.FavoritePhoneName = (string)dataReader["phone_name"];
                        }
                        if (dataReader["phone_number"] != DBNull.Value)
                        {
                            obj.ToPhoneNo = (string)dataReader["phone_number"];
                        }
                        list.Add(obj);

                    }

                    return list;

                }

            }

        }

        /// //-----------------------------------getUserPhones---------------------
        public List<ManagePhonesViewModel> getUserPhones(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select phone_id,phone_name,phone_number from user_phone_link where user_id = " + userID;



                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ManagePhonesViewModel> list = new List<ManagePhonesViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ManagePhonesViewModel obj = new ManagePhonesViewModel();


                        if (dataReader["phone_id"] != DBNull.Value)
                        {
                            obj.FavoritePhoneID = dataReader["phone_id"].ToString();
                        }
                        if (dataReader["phone_name"] != DBNull.Value)
                        {
                            obj.FavoritePhoneName = (string)dataReader["phone_name"];
                        }
                        if (dataReader["phone_number"] != DBNull.Value)
                        {
                            obj.FavoritePhoneNo = (string)dataReader["phone_number"];
                        }
                        list.Add(obj);

                    }

                    return list;

                }

            }

        }


        //----------------------- getPhoneInfo----------------------------------------------
        //
        public ManagePhonesViewModel getPhoneInfo(int PhoneID)
        {
            ManagePhonesViewModel model = new ManagePhonesViewModel();
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlrt;
            sqlrt = "select phone_name,phone_number from user_phone_link where phone_id = " + PhoneID;
            using (OracleConnection conObj = new OracleConnection(conString))
            {
                conObj.Open();
                cmd = new OracleCommand(sqlrt, conObj);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while ((dr.Read()))
                    {
                        model.FavoritePhoneName = dr[0].ToString();
                        model.FavoritePhoneNo = dr[1].ToString();
                        model.sts = "true";
                    }

                }
                else
                {

                    model.FavoritePhoneName = "";
                    model.FavoritePhoneNo = "";
                    model.sts = "false";
                }
                conObj.Close();
            }

            return model;

        }

        //----------------------------------Update phone Information-----------------------------------------
        public int updatePhoneInfo(string PhoneID, string PhoneName, string PhoneNumber)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update user_phone_link " +
                               "set phone_name ='" + PhoneName +
                               "', phone_number = '" + PhoneNumber +
                               "' where phone_id=" + PhoneID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }

        //----------------------------------Delete phone Information-----------------------------------------
        public int DeletePhoneInfo(string PhoneID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "delete  from user_phone_link where phone_id = " + PhoneID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }

        /*8888888888888888888888888888888888888888888888888---METER CRUD----88888888888888888888888888888888888888888888888888*/

        /// //-----------------------------------getFavMeters---------------------
        public List<ElectricityViewModel> getFavMeters(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select meter_id,meter_name,meter_number from user_meter_link where user_id = " + userID;



                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ElectricityViewModel> list = new List<ElectricityViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ElectricityViewModel obj = new ElectricityViewModel();


                        if (dataReader["meter_id"] != DBNull.Value)
                        {
                            obj.MeterID = dataReader["meter_id"].ToString();
                        }
                        if (dataReader["meter_name"] != DBNull.Value)
                        {
                            obj.MeterName = (string)dataReader["meter_name"];
                        }
                        if (dataReader["meter_number"] != DBNull.Value)
                        {
                            obj.MeterNo = (string)dataReader["meter_number"];
                        }
                        list.Add(obj);

                    }

                    return list;

                }

            }

        }

        /// //-----------------------------------getUserMeters---------------------
        public List<ManageMetersViewModel> getUserMeters(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select meter_id,meter_name,meter_number from user_meter_link where user_id = " + userID;



                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ManageMetersViewModel> list = new List<ManageMetersViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ManageMetersViewModel obj = new ManageMetersViewModel();


                        if (dataReader["meter_id"] != DBNull.Value)
                        {
                            obj.MeterID = dataReader["meter_id"].ToString();
                        }
                        if (dataReader["meter_name"] != DBNull.Value)
                        {
                            obj.MeterName = (string)dataReader["meter_name"];
                        }
                        if (dataReader["meter_number"] != DBNull.Value)
                        {
                            obj.MeterNo = (string)dataReader["meter_number"];
                        }
                        list.Add(obj);

                    }

                    return list;

                }

            }

        }


        //----------------------- getMeterInfo----------------------------------------------
        //
        public ManageMetersViewModel getMeterInfo(int MeterID)
        {
            ManageMetersViewModel model = new ManageMetersViewModel();
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlrt;
            sqlrt = "select meter_name,meter_number from user_meter_link where meter_id = " + MeterID;
            using (OracleConnection conObj = new OracleConnection(conString))
            {
                conObj.Open();
                cmd = new OracleCommand(sqlrt, conObj);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while ((dr.Read()))
                    {
                        model.MeterName = dr[0].ToString();
                        model.MeterNo = dr[1].ToString();
                        model.sts = "true";
                    }

                }
                else
                {

                    model.MeterName = "";
                    model.MeterNo = "";
                    model.sts = "false";
                }
                conObj.Close();
            }

            return model;

        }

        //----------------------------------Update Meter Information-----------------------------------------
        public int updateMeterInfo(string MeterID, string MeterName, string MeterNumber)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update user_meter_link " +
                               "set meter_name ='" + MeterName +
                               "', meter_number = '" + MeterNumber +
                               "' where meter_id=" + MeterID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }

        //----------------------------------Delete Meter Information-----------------------------------------
        public int DeleteMeterInfo(string MeterID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "delete  from user_meter_link where meter_id = " + MeterID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }




        /// //-----------------------------------getUserCards---------------------
        public List<ManageCardsViewModel> getUserCards(string userID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query =
                    "select card_id,card_name,card_number from user_card_link where user_id = " + userID ;
                


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<ManageCardsViewModel> list = new List<ManageCardsViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ManageCardsViewModel obj = new ManageCardsViewModel();


                        if (dataReader["card_id"] != DBNull.Value)
                        {
                            obj.CardID = dataReader["card_id"].ToString();
                        }
                        if (dataReader["card_name"] != DBNull.Value)
                        {
                            obj.CardName = (string)dataReader["card_name"];
                        }
              /*          if (dataReader["req_date"] != DBNull.Value)
                        {
                            obj.Date = ((DateTime)dataReader["req_date"]).ToString("dd/MM/yyyy");
                        }*/

                        if (dataReader["card_number"] != DBNull.Value)
                        {
                            obj.CardNo = (string)dataReader["card_number"];
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }



        //----------------------- getCardInfo----------------------------------------------
        //
        public ManageCardsViewModel getCardInfo(int CardID)
        {
            ManageCardsViewModel model = new ManageCardsViewModel();
            OracleCommand cmd;
            OracleDataReader dr;
            String sqlrt;
            sqlrt = "select card_name,card_number from user_card_link where card_id = " + CardID;
            using (OracleConnection conObj = new OracleConnection(conString))
            {
                conObj.Open();
                cmd = new OracleCommand(sqlrt, conObj);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while ((dr.Read()))
                    {
                        model.CardName = dr[0].ToString();
                        model.CardNo = dr[1].ToString();
                        model.sts = "true";
                    }

                }
                else
                {

                    model.CardName = "";
                    model.CardNo = "";
                    model.sts = "false";
                }
                conObj.Close();
            }

            return model;

        }


        //----------------------------------Update Card Information-----------------------------------------
        public int updateCardInfo(string CardID, string CardName, string CardNumber)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update user_card_link " +// set user_name = '" + userName + 
                               "set card_name ='" + CardName +
                               //"', user_mobile = '" + mobile + 
                               "', card_number = '" + CardNumber +
                               "' where card_id=" + CardID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }


        //----------------------------------Delete Card Information-----------------------------------------
        public int DeleteCardInfo(string CardID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "delete  from user_card_link where card_id = " + CardID;


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }




        //----------------------------------Reject Transfer-----------------------------------------
        public int RejectTransfer(string tranID)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                /* string queryy = "update salary_temp  set SALARY_STATUS ='" + sts +
                                "', SALARY_PROCESS_DATE=sysdate where SALARY_USER_ID=" + user_id +
                                " and SALARY_ACCOUNT_NO='" + acc + "' and SALARY_FILE_NAME='" + FILE_NAME + "'";*/


                string query = "update transfer_temp set transfer_status = 'Rejected' where tran_id = " + tranID;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }



        }

        /// //-----------------------------------getSalaries---------------------
        public List<SalaryAuthViewModel> getSaleryFiles()//(string user_id, string fileName)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                /*string query =
                    "select salary_account_no,salary_amount,salary_file_name,salary_comp_act from salary_temp WHERE " +
                    "salary_status = 'P' and salary_user_id = '" + user_id + "' and salary_file_name = '" + fileName +
                    "'";*/
                string query = "select file_id,file_name,no_of_rows,file_date,file_total from salary_files where no_of_process_rows < no_of_rows and status = 'RWS'";

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<SalaryAuthViewModel> list = new List<SalaryAuthViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        SalaryAuthViewModel obj = new SalaryAuthViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                        if (dataReader["file_id"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.FileId = dataReader["file_id"].ToString();
                        }
                        if (dataReader["file_name"] != DBNull.Value)
                        {
                            obj.FileName = (string)dataReader["file_name"];
                        }
                        if (dataReader["no_of_rows"] != DBNull.Value)
                        {
                            obj.NoOfRows = (string)dataReader["no_of_rows"];
                        }
                        if (dataReader["file_date"] != DBNull.Value)
                        {
                            obj.FileDate = ((DateTime)dataReader["file_date"]).ToString("dd/MM/yyyy");
                        }
                        if (dataReader["file_total"] != DBNull.Value)
                        {
                            obj.FileTotal = (string)dataReader["file_total"];
                        }
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }


        //---------------------------------------------------------get FileName --------------------------------//
        public String getSalaryFileName(string user_id, string fileID)
        {
            String fileName = "";
            using (OracleConnection con = new OracleConnection(conString))
            {
               /* string query = " SELECT acc_id,acc_no from user_acc_link where user_id =" + user_id +
                               " and substr(acc_no,14)=" + act;*/

                string query = "select file_name from salary_files where file_id =" + fileID;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();


                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {


                        if (dataReader["file_name"] != DBNull.Value)
                        {
                            fileName = (string)dataReader["file_name"];
                            
                        }

                        
                    }

                    return fileName;

                }

            }

        }


        //----------------------- Reject FiLE SALARY Status----------------------------------------------
        //
        public int Rejectsalary(string user_id, string FILE_NAME)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update salary_temp  set SALARY_STATUS ='Rj', SALARY_PROCESS_DATE=sysdate, salary_authorizer_id = '" + user_id + "' where SALARY_FILE_NAME='" + FILE_NAME + "'";

                OracleCommand cmd = new OracleCommand(query, con);

                string query2 = "update salary_files  set STATUS ='Rj', salary_authorizer_id = '" + user_id + "' where FILE_NAME='" + FILE_NAME + "'";

                OracleCommand cmd2 = new OracleCommand(query2, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();
               result = cmd2.ExecuteNonQuery();



                return result;

            }



        }

        /// //-----------------------------------getSalaries---------------------
        public List<SalaryAuthViewModel> getTotalAndNoOfRows(string file_id)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {

                string query = "select file_total,no_of_rows from salary_files where file_id =" + file_id;

                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();

                List<SalaryAuthViewModel> list = new List<SalaryAuthViewModel>();
                using (IDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        SalaryAuthViewModel obj = new SalaryAuthViewModel();
                        //if (dataReader["acc_id"] != DBNull.Value)
                        //{
                        if (dataReader["file_total"] != DBNull.Value)
                        {
                            //obj.AccountID = (int)dataReader["acc_id"];
                            obj.FileTotal = dataReader["file_total"].ToString();
                        }
                        if (dataReader["no_of_rows"] != DBNull.Value)
                        {
                            obj.NoOfRows = (string)dataReader["no_of_rows"];
                        }
                       
                        list.Add(obj);
                        //}
                    }

                    return list;

                }

            }

        }








        //----------------------- update UnlockUserSession----------------------------------------------
        //
       

/*END OF CLASS*/
        public int UnlockUserSession(string username, string password)
        {
            using (OracleConnection con = new OracleConnection(conString))
            {
                string query = "update users set login_status = '0' where user_log = '" + username +
                               "' and user_pwd='" + password + "'";


                OracleCommand cmd = new OracleCommand(query, con);

                con.Open();
                int result = -1;
                result = cmd.ExecuteNonQuery();



                return result;

            }
        }
    }






}