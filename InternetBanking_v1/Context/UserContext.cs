using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using InternetBanking_v1.Models.Login;

namespace InternetBanking_v1.Context
{
    public class UserContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}