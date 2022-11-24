using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using IBLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(InternetBanking_v1.Startup))]

namespace InternetBanking_v1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createRolesandUsers();

           // ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new MultiLanguageControllerActivator()));

            //Removed for brevity

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }




    }
}
