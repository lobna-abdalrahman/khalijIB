using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLogic
{
    class Menu
    {
        public class MenuModels
        {

            public string MainMenuName { get; set; }
            public int MainMenuId { get; set; }
            public string SubMenuName { get; set; }
            public int SubMenuId { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public int? RoleId { get; set; }
            public string RoleName { get; set; }
        }


    }
}
