﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using InternetBanking_v1.Models;
using System.Data.OracleClient;


namespace InternetBanking_v1.Repository
{
    public class MenuData
    {
        public static IList<Menu> GetMenus( string usernumber,string rolenumber)
        {
            /* using ado.net code */
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<Menu> menuList = new List<Menu>();
                OracleCommand cmd = new OracleCommand("usp_GetMenuData", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("usernumber", usernumber);
                cmd.Parameters.AddWithValue("rolenumber", rolenumber);
                cmd.Parameters.Add("menucur", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;

                con.Open();
                IDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Menu menu = new Menu();
                    menu.MID = Convert.ToInt32(sdr["MID"].ToString());
                    menu.MenuName = sdr["MenuName"].ToString();
                    menu.MenuURL = sdr["MenuURL"].ToString();
                    menu.MenuIMG = sdr["MenuIMG"].ToString();
                    menu.MenuParentID = Convert.ToInt32(sdr["MenuParentID"].ToString());
                    menuList.Add(menu);
                }
                return menuList;
            }

            /* use can also use dapper orm
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ToString()))
            {
                try
                {
                    var para = new DynamicParameters();
                    para.Add("@UserId", UserId);
                    var MenuList = con.Query<Menu>("usp_GetMenuData", para, null, true, 0, CommandType.StoredProcedure);
                    return MenuList.ToList();
                }
                catch
                {
                    return null;
                }
            }*/
        }

        public static IList<Menu> GetArabicMenus(string usernumber, string rolenumber)
        {
            /* using ado.net code */
            using (OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<Menu> menuList = new List<Menu>();
                OracleCommand cmd = new OracleCommand("usp_ar_getmenudata", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("usernumber", usernumber);
                cmd.Parameters.AddWithValue("rolenumber", rolenumber);
                cmd.Parameters.Add("menucur", OracleType.Cursor).Direction = System.Data.ParameterDirection.Output;

                con.Open();
                IDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Menu menu = new Menu();
                    menu.MID = Convert.ToInt32(sdr["MID"].ToString());
                    menu.MenuName = sdr["Menu_AR_Name"].ToString();
                    menu.MenuURL = sdr["MenuURL"].ToString();
                    menu.MenuIMG = sdr["MenuIMG"].ToString();
                    menu.MenuParentID = Convert.ToInt32(sdr["MenuParentID"].ToString());
                    menuList.Add(menu);
                }
                return menuList;
            }

  
        }
    }
}
