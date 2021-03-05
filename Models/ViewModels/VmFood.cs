using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models.ViewModels
{
    public class VmFood
    {
        public int ItemId { get; set; }
        public string FoodName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }
        public string MenuName { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }
        public List<Menu> MenuList { get; set; }
    }
}