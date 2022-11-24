using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace IBLogic
{
    public class Balance
    {

        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string BranchName { get; set; }
        public string AvailableBalance { get; set; }
        public string SumAccounts { get; set; }

    }

    public class BalanceLogic
    {
        public long AvailableBalance;
        public Balance balance = new Balance();

        private string mybal = "20";

        public long CheckUserBalance(User user)
        {
            string myUser = user.Username;

            if (myUser.Equals("hask"))
            {

                mybal = balance.AvailableBalance = "5720";
            }
            AvailableBalance = Convert.ToInt64(mybal);
            return AvailableBalance;
        }

        public static string configip = null, configport = null, configpath = null;
        public string BASE_URL;//= "http://" + configip + ":" + configport + configpath;
        //Base Url
        public void getconfig()
        {
            try
            {

                using (StreamReader sr = new StreamReader("C:/Configuration/api.txt"))
                {
                    string line;


                    while ((line = sr.ReadLine()) != null)
                    {
                        configip = line;
                        configport = sr.ReadLine();
                        configpath = sr.ReadLine();
                        BASE_URL = "http://" + configip + ":" + configport + "/" + configpath;
                    }
                }
            }
            catch (Exception e)
            {
                String s = e.Message;
            }
        }
        
        ////////////////////////////////
        public string GetUserBalance(string account)
        {
            getconfig();
            Uri requestUri = new Uri(BASE_URL+"/GetBalance"); //replace your Url  
            //Uri requestUri = new Uri("http://192.168.0.126:8080/IBMiddleware/webresources/IBWebservices/GetBalance");

            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.account = account;//"990042010593883".ToString();
            dynamicJson.lang = "0";
            dynamicJson.uuid = "someuuid";

            string json = "";
            json = JsonConvert.SerializeObject(dynamicJson);
            var responJsonText = "";
            JObject JResp = new JObject();

            using (var objClint = new HttpClient())
            {

                try
                {
                    // HttpResponseMessage respon = objClint.PostAsync(requestUri, json).Result;

                    HttpResponseMessage respon =  objClint.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json")).Result;



                    if (respon.IsSuccessStatusCode)
                    {
                        
                        responJsonText =  respon.Content.ReadAsStringAsync().Result;
                    }

                }
                catch (Exception e)
                {

                    //Console.WriteLine(e);
                    responJsonText = "0";
                    //throw;
                }
                return responJsonText;
            }


        }
    }


}
