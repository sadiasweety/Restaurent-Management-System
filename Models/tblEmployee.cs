namespace RestaurentManagementSystem_EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEmployee")]
    public partial class tblEmployee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public DateTime DoB { get; set; }

        public string ImageName { get; set; }

        public string ImageUrl { get; set; }
    }
}
