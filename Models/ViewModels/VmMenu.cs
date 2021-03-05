using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models.ViewModels
{
    public class VmMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int Quantity { get; set; }
    }
}