using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models
{
    public class EmployeeCreateViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public System.DateTime DoB { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}