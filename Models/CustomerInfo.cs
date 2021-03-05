using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurentManagementSystem_EF.Models
{
    [Table("CustomerInfo")]
    public class CustomerInfo
    {

        [Key]
        public int CustomerID { get; set; }

        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string OrderDate { get; set; }

        [StringLength(500)]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        public CustomerInfo()
        {
            ImagePath = "~/AppFiles/Images/default.png";
        }
    }

}
