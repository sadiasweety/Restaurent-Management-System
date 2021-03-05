namespace RestaurentManagementSystem_EF.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<tblEmployee> tblEmployees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblEmployee>()
                .Property(e => e.EmployeeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblEmployee>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
