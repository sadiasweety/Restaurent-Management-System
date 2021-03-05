using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
}