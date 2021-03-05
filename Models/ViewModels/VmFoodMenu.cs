using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models.ViewModels
{
    public class VmFoodMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<Menu> MenuList { get; set; }
        public List<VmFood> FoodList { get; set; }
    }
}