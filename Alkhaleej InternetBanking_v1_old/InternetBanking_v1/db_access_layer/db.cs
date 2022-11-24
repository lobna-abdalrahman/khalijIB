using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using InternetBanking_v1.Models.Account;
using Oracle.DataAccess.Client;

namespace InternetBanking_v1.db_access_layer
{
    public class db
    {
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        //GET TOP MENU
        public DataSet getCategory()
        {
            OracleCommand com = new OracleCommand("Select * from ");
            return null;
        }

        //GET Accounts
        public List<Bal> GetAccounts()
        {
            List<Bal> balances = new List<Bal>();
            //balances.Add(new Bal{AccountNumber = });
            return null;
        }

  
    }
}