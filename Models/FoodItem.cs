using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models
{
    public class FoodItem
    {
        [Key]
        public int ItemId { get; set; }
        public string FoodName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int Quantity { get; set; }
        public int MenuId { get; set; }
    }
}